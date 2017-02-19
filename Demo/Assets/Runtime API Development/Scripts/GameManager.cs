/*
    _____  _____  _____  _____  ______
        |  _____ |      |      |  ___|
        |  _____ |      |      |     |
    
     U       N       I       T      Y
                                         
    
    TerraUnity Co. - Earth Simulation Tools - 2016
    
    http://terraunity.com
    info@terraunity.com
    
    This script is written for Unity 3D Engine.
    Unity 3D Version: Unity 5.x
    
    
    INFO: Game Manger script for TerraLand Run-Time Demo

    Written by: Amir Badamchi
    
*/


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityStandardAssets.Vehicles.Aeroplane;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private static Runtime runTime;

    public GameObject worldGenerator;
    public GameObject player;
    public Camera cam;
    public Light sun;
    public GameObject clouds;
    public Material sky;
    public GameObject pauseMenu;
    public GameObject fadeIn;
    public GameObject loadingScreen;
    public GameObject loadingUI;
    public GameObject loadingHeightmap;
    public GameObject loadingImage;
    public GameObject heightmapPercent;
    public GameObject ImagePercent;

    public float startHeight = 300f; // From beneath terrain surface
    public bool startOnGround = false;
    public float heightLimit = 1000f; // From highest elevation in active world
    public LayerMask terrainLayer;
    public float cloudsHeight = 1000f; // From highest elevation in active world

    public bool enableClouds = true;
    public bool enableCloudShadows = false;
    public bool enableDetailTextures = true;
    public Texture2D detailTexture;
    public Texture2D detailNormal;
    public Texture2D detailNormalFar;
    [Range(0, 100)] public float detailBelending = 25f;
    public float detailTileSize = 25f;
    public bool volumetricLighting = true;
    public bool atmosphericScattering = true;

    public bool debugMode = false;

    private Terrain terrain;
    private RaycastHit hit;
    private Vector3 rayPosition;
    private bool playerIsSet = false;
    private float currentHighestPoint;

    private kode80.Clouds.kode80CloudShadows cloudShadows;
    private Ray ray;
    private float terrainHeight;
    private float playerHeight;
    private Rigidbody rigidBodyAircraft;
    private kode80.Clouds.kode80Clouds kodeClouds;

    private double initialLat;
    private double initialLon;
    private double playerLat;
    private double playerLon;
    private static double earthRadius = 6378137;
    private Vector3 realWorldPosition;
    private AeroplaneController aeroplaneController;
    private float takeoffPower;
    private bool brakesReleased = false;
    private float initialMass;
    private bool tookOff = false;
    private float effectiveHeight = 0.1f;
    private float tookOffHeight = 5f;
    private VolumetricLightRenderer volumetricLightRenderer;
    private AtmosphericScattering atmosphericScatter;
    private LandingGear landingGear;
    private bool engineSet = false;
    private float initialFixedTimeStep;

    private GlobalFog globalFog;

    public static bool volumetricLightingMenu;
    public static bool atmosphericScatteringMenu;
    public static bool enableCloudsMenu;
    public static bool enableCloudShadowsMenu;
    public static bool enableDetailTexturesMenu;

    private bool isSlowMotion = false;
    private bool isPaused = false;
    private AudioSource[] allAudioSources;
    private Image fade;
    private float startTime;
    private float fadeTime = 6f;
    private Image loading;

    private int downloadedHeightmaps;
    private int downloadedImages;
    private int totalchunks;

    private RectTransform barRectHeightmap;
    private RectTransform barRectImage;

    private AudioClip[] audioTracks;
    private AudioSource musicPlayer;
    private int trackIndex;

    private LensFlare sunFlare;

    private bool adjustPlanePhysics = false;
    private bool gameLoaded = false;

    private Fisheye fishEye;


    void Awake ()
    {
        if(pauseMenu != null)
            pauseMenu.SetActive(false);

        barRectHeightmap = loadingHeightmap.GetComponent<RectTransform>();
        barRectImage = loadingImage.GetComponent<RectTransform>();

        barRectHeightmap.sizeDelta = Vector2.zero;
        barRectImage.sizeDelta = Vector2.zero;

        fade = fadeIn.GetComponent<Image>();
        fade.color = new UnityEngine.Color(fade.color.r, fade.color.g, fade.color.b, 1f);

        loading = loadingScreen.GetComponent<Image>();
        loading.color = new UnityEngine.Color(loading.color.r, loading.color.g, loading.color.b, 1f);

        if(!MainMenu.latitude.Equals(""))
            SetFromMenu();

        runTime = worldGenerator.GetComponent<Runtime>();

        player.gameObject.SetActive(false);

        sunFlare = sun.GetComponent<LensFlare>();
        sunFlare.enabled = false;

        rayPosition = new Vector3(1, 99000, 1);

        if(enableClouds)
        {
            clouds.SetActive(true);
            cloudShadows = sun.GetComponent<kode80.Clouds.kode80CloudShadows>();
            cloudShadows.enabled = false;
            kodeClouds = clouds.GetComponent<kode80.Clouds.kode80Clouds>();
            kodeClouds.coverageOffsetX = UnityEngine.Random.Range(0f, 1f);
            kodeClouds.coverageOffsetY = UnityEngine.Random.Range(0f, 1f);
        }
        else
            clouds.SetActive(false);

        atmosphericScatter = cam.GetComponent<AtmosphericScattering>();

        if(atmosphericScattering)
            atmosphericScatter.enabled = true;
        else
            atmosphericScatter.enabled = false;
        
        rigidBodyAircraft = player.GetComponent<Rigidbody>();
        aeroplaneController = player.GetComponent<AeroplaneController>();
        takeoffPower = aeroplaneController.MaxEnginePower * 0.75f;
        initialMass = rigidBodyAircraft.mass;
        volumetricLightRenderer = cam.GetComponent<VolumetricLightRenderer>();
        volumetricLightRenderer.enabled = false;
        landingGear = player.GetComponent<LandingGear>();
        QualitySettings.shadowDistance = (runTime.areaSize * 1000f) / 4f;

        if(atmosphericScattering)
            RenderSettings.skybox = sky;
        
        initialFixedTimeStep = Time.fixedDeltaTime;

        globalFog = cam.GetComponent<GlobalFog>();
        fishEye = cam.gameObject.GetComponent<Fisheye>();

        CheckDetailTextures();

        if(startOnGround)
        {
            //rigidBodyAircraft.isKinematic = true;
            tookOff = false;
        }
        else
        {
            //rigidBodyAircraft.isKinematic = false;
            tookOff = true;
        }

        if(debugMode)
        {
            worldGenerator.SetActive(false);
            CreateTempTerrain();
            SetPlayer();
        }

        SetupMusic();
    }

    void Update ()
    {
        GetLatLon();

        if(Input.GetKeyDown(KeyCode.F))
            TerraLand.TerraLandRuntime.worldIsGenerated = true;

        if(!debugMode)
            if(!playerIsSet && TerraLand.TerraLandRuntime.worldIsGenerated)
                SetPlayer();
        
        if(!playerIsSet)
            LoadingScreen();
        else
        {
            if(!debugMode)
            {
                if(enableClouds)
                {
                    //if(!InfiniteTerrain.inProgressNorth && !InfiniteTerrain.inProgressSouth && !InfiniteTerrain.inProgressEast && !InfiniteTerrain.inProgressWest)
                    //{
                        currentHighestPoint = TerraLand.TerraLandRuntime.highestPoints.Max();

                        clouds.transform.position = Vector3.Lerp
                            (
                                clouds.transform.position,
                                new Vector3(clouds.transform.position.x, currentHighestPoint + cloudsHeight, clouds.transform.position.z),
                                Time.deltaTime * 0.2f
                            );
                    //}
                }

//                if(player.transform.position.y > currentHighestPoint + heightLimit)
//                    player.transform.position = Vector3.Lerp
//                    (
//                        player.transform.position,
//                            new Vector3(player.transform.position.x, currentHighestPoint + heightLimit, player.transform.position.z),
//                        Time.deltaTime * 0.25f
//                    );
            }

            ray = new Ray(new Vector3(player.transform.position.x, 1000f, player.transform.position.z), Vector3.down);

            if (Physics.Raycast (ray, out hit, Mathf.Infinity, terrainLayer))
                terrain = hit.transform.gameObject.GetComponent<Terrain>();

            if(terrain != null)
            {
                terrainHeight = terrain.SampleHeight(player.transform.position);
                playerHeight = player.transform.position.y - terrainHeight;

                if(!startOnGround && !engineSet)
                {
                    try
                    {
                        if(aeroplaneController.EnginePower <= takeoffPower)
                            aeroplaneController.Move(0, 0, 0, 1, false);
                        else
                            engineSet = true;
                    }
                    catch{}
                }

                if(aeroplaneController.EnginePower >= takeoffPower && playerHeight > tookOffHeight)
                    tookOff = true;
                else if(aeroplaneController.EnginePower == 0 && aeroplaneController.ForwardSpeed == 0f)
                    tookOff = false;

                if(adjustPlanePhysics)
                {
                    //if(aeroplaneController.EnginePower >= takeoffPower)
                    //rigidBodyAircraft.isKinematic = false;

                    if(!tookOff)
                    {
                        if(aeroplaneController.EnginePower <= takeoffPower && playerHeight < effectiveHeight)
                        {
                            //rigidBodyAircraft.isKinematic = true;
                            rigidBodyAircraft.constraints = RigidbodyConstraints.FreezeAll;
                            rigidBodyAircraft.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                        }
                        else if(aeroplaneController.EnginePower >= takeoffPower && playerHeight < effectiveHeight)
                        {
                            rigidBodyAircraft.constraints =
                                RigidbodyConstraints.FreezeRotationX |
                                RigidbodyConstraints.FreezeRotationY |
                                RigidbodyConstraints.FreezeRotationZ |
                                RigidbodyConstraints.FreezePositionX;

                            //rigidBodyAircraft.mass = initialMass / 4f;
                            rigidBodyAircraft.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        }
                        else
                        {
                            rigidBodyAircraft.constraints = RigidbodyConstraints.None;
                            //rigidBodyAircraft.mass = initialMass;
                            rigidBodyAircraft.collisionDetectionMode = CollisionDetectionMode.Discrete;
                        }
                    }
                    else
                    {
                        if(aeroplaneController.EnginePower == 0 && aeroplaneController.ForwardSpeed == 0f)
                        {
                            //rigidBodyAircraft.isKinematic = true;
                            rigidBodyAircraft.constraints = RigidbodyConstraints.FreezeAll;
                            rigidBodyAircraft.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                        }
                        else if(aeroplaneController.EnginePower <= takeoffPower && playerHeight < effectiveHeight)
                        {
                            rigidBodyAircraft.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                        }
                        else
                        {
                            rigidBodyAircraft.constraints = RigidbodyConstraints.None;
                            //rigidBodyAircraft.mass = initialMass;
                            rigidBodyAircraft.collisionDetectionMode = CollisionDetectionMode.Discrete;
                        }
                    }

                    // Use with caustion, can cause camera issues
                    //if(playerHeight < 1f)
                        //Time.fixedDeltaTime = 0.01f;
                    //else
                        //Time.fixedDeltaTime = initialFixedTimeStep;
                }
            }

            if(enableClouds)
            {
                kodeClouds.cloudBaseColor = RenderSettings.ambientLight * RenderSettings.ambientIntensity;
                kodeClouds.cloudTopColor = sun.color * sun.intensity;
            }

            if(Input.GetKeyDown(KeyCode.R))
            {
                player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
                player.transform.position += player.transform.up * startHeight;
                startOnGround = false;
                engineSet = false;
            }

            // Set Global Fog's "Affect SkyBox" blending factor to mix terrains with sky at horizon
            if(globalFog != null)
                globalFog.skyFogBlending =  Mathf.Lerp(globalFog.skyFogBlending, Mathf.Clamp(Mathf.InverseLerp(100f, 5000f, playerHeight), 0.075f, 0.75f), Time.deltaTime);

            // Set special Fish Eye's x coordinate based on player height to simulate curved sphere-like Earth surface
            if(fishEye != null)
                fishEye.strengthX = Mathf.InverseLerp(0.0f, 10000.0f, player.transform.position.y) / 4f;

            // Display Game Menu by pressing Escape button
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;

                if(isPaused)
                    PauseGame();
                else
                    ResumeGame();
            }

            if(Time.timeSinceLevelLoad > startTime + 2 && fade.enabled)
            {
                float fadeAmount = 1f - Mathf.InverseLerp(0f, fadeTime, Time.timeSinceLevelLoad - (startTime + 2));
                fade.color = new UnityEngine.Color(fade.color.r, fade.color.g, fade.color.b, fadeAmount);
                musicPlayer.volume = fadeAmount;
            }

            if(fade.color.a == 0f)
            {
                fade.enabled = false;
                musicPlayer.enabled = false;
                gameLoaded = true;
            }
        }
    }

    private void LoadingScreen ()
    {
        totalchunks = (int)Mathf.Pow((int)runTime.terrainGridSize, 2);
        downloadedHeightmaps = TerraLand.TerraLandRuntime.downloadedHeightmapIndex;
        downloadedImages = TerraLand.TerraLandRuntime.downloadedImageIndex;

        float progressHeightmap = (float)downloadedHeightmaps / (float)totalchunks;
        float progressImage = (float)downloadedImages / (float)totalchunks;

        float progressHeightmapPercent = progressHeightmap * 100f;
        float progressImagePercent = progressImage * 100f;

        float barWidthHeightmap = progressHeightmap * 940f;
        float barHeightHeightmap = progressHeightmap * 27f;

        float barWidthImage = progressImage * 940f;
        float barHeightImage = progressImage * 27f;

        barRectHeightmap.sizeDelta = new Vector2(barWidthHeightmap, barHeightHeightmap);
        barRectImage.sizeDelta = new Vector2(barWidthImage, barHeightImage);

        heightmapPercent.GetComponent<Text>().text = (int)progressHeightmapPercent + "%";
        ImagePercent.GetComponent<Text>().text = (int)progressImagePercent + "%";

        PlayNextSong();
    }

    private void SetupMusic ()
    {
        audioTracks = new AudioClip[]
        {
            Resources.Load("Menu/Music/BenSound-SciFi") as AudioClip,
            Resources.Load("Menu/Music/Exclusion-Earthshine") as AudioClip,
            Resources.Load("Menu/Music/Exclusion-Unity") as AudioClip,
            Resources.Load("Menu/Music/Machinimasound-SeptemberSky") as AudioClip,
            Resources.Load("Menu/Music/Mnykin-ElfSwamp") as AudioClip
        };

        musicPlayer = GetComponent<AudioSource>();
        trackIndex = UnityEngine.Random.Range(0, audioTracks.Length);
        musicPlayer.clip = audioTracks[trackIndex];
        musicPlayer.Play();
    }

    private void PlayNextSong ()
    {
        if (!musicPlayer.isPlaying)
        {
            trackIndex++;

            if(trackIndex > audioTracks.Length)
                trackIndex = 0;

            musicPlayer.clip = audioTracks[trackIndex];
            musicPlayer.Play();
        }
    }

    public void PauseGame ()
    {
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; //0.025f
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        foreach(AudioSource audio in allAudioSources)
        {
            if(!audio.gameObject.name.Equals("GameManager"))
                audio.mute = true;
        }

        System.GC.Collect();
    }

    public void ResumeGame ()
    {
        isPaused = false;

        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        foreach(AudioSource audio in allAudioSources)
        {
            if(!audio.gameObject.name.Equals("GameManager"))
                audio.mute = false;
        }
    }

    public void SetFromMenu ()
    {
        volumetricLighting = volumetricLightingMenu;
        atmosphericScattering = atmosphericScatteringMenu;
        enableClouds = enableCloudsMenu;
        enableCloudShadows = enableCloudShadowsMenu;
        enableDetailTextures = enableDetailTexturesMenu;

        debugMode = false;
    }

    private void GetLatLon ()
    {
        realWorldPosition = FloatingOriginAdvanced.staticPosition;

        double offsetLat = (realWorldPosition.z / earthRadius) * 180 / Math.PI;
        playerLat = initialLat + offsetLat; // Moving NORTH/SOUTH

        double offsetLon = (realWorldPosition.x / (earthRadius * Math.Cos(Math.PI * playerLat / 180))) * 180 / Math.PI;
        playerLon = initialLon + offsetLon; // Moving EAST/WEST
    }

    public void SetPlayer ()
    {
        if(!debugMode)
        {
            if(!MainMenu.latitude.Equals(""))
            {
                initialLat = double.Parse(runTime.latitudeUser);
                initialLon = double.Parse(runTime.longitudeUser);
            }
            else
            {
                initialLat = double.Parse(Runtime.latitudeMenu);
                initialLon = double.Parse(Runtime.longitudeMenu);
            }
        }

        if(startHeight <= 0f)
            startHeight = 0.01f;
        
        ray = new Ray(rayPosition, Vector3.down);

        if (Physics.Raycast (ray, out hit, Mathf.Infinity, terrainLayer))
            terrain = hit.transform.gameObject.GetComponent<Terrain>();

        if(terrain != null)
        {
            terrainHeight = terrain.SampleHeight(player.transform.position);

            if(!startOnGround)
                player.transform.position = new Vector3(0, terrainHeight + terrain.gameObject.transform.position.y + startHeight, 0);
            else
                player.transform.position = new Vector3(0, terrainHeight + terrain.gameObject.transform.position.y + 0.01f, 0);

            player.gameObject.SetActive(true);

            UnityEngine.Debug.Log("Player Loaded");
        }
        else
            UnityEngine.Debug.LogWarning("No Terrains Detected Underneath!");

        if(enableClouds && enableCloudShadows)
             cloudShadows.enabled = true;

        if(volumetricLighting)
            volumetricLightRenderer.enabled = true;

        sunFlare.enabled = true;

        if(!startOnGround)
            landingGear.gearSwitch = !landingGear.gearSwitch;

        if(!debugMode && enableDetailTextures)
            AddDetailTexturesToTerrains();

        //#if UNITY_EDITOR
            //UnityEditor.PlayerSettings.runInBackground = Runtime.initialRunInBackground;
        //#endif

        loadingScreen.SetActive(false);
        loadingUI.SetActive(false);

        startTime = Time.timeSinceLevelLoad;

        playerIsSet = true;
    }

    private void CreateTempTerrain ()
    {
        float terrainSize = 100000;

        GameObject terrainGameObject = new GameObject("Debug Terrain");
        terrainGameObject.transform.position = new Vector3(-(terrainSize / 2f), 0, -(terrainSize / 2f));
        terrainGameObject.AddComponent<Terrain>();

        TerrainData data = new TerrainData();
        data.size = new Vector3(terrainSize, 1, terrainSize);

        Terrain terrain = terrainGameObject.GetComponent<Terrain>();
        terrain.terrainData = data;

        Material terrainMaterial = new Material(Shader.Find("Nature/Terrain/Standard"));
        terrain.materialType = Terrain.MaterialType.Custom;
        terrain.materialTemplate = terrainMaterial;

        terrainGameObject.AddComponent<TerrainCollider>();
        terrainGameObject.GetComponent<TerrainCollider>().terrainData = data;

        terrainGameObject.layer = 8;

        if(enableDetailTextures)
            AddDetailTextures(terrain, detailBelending, false);
    }

    private void CheckDetailTextures ()
    {
        #if UNITY_EDITOR
            Texture2D[] detailTextures = new Texture2D[2] {detailTexture, detailNormal};

            foreach (Texture2D currentImage in detailTextures)
            {
                UnityEditor.TextureImporter imageImport = UnityEditor.AssetImporter.GetAtPath(UnityEditor.AssetDatabase.GetAssetPath(currentImage)) as UnityEditor.TextureImporter;

                if (imageImport != null && !imageImport.isReadable)
                {
                    imageImport.isReadable = true;
                    UnityEditor.AssetDatabase.ImportAsset(UnityEditor.AssetDatabase.GetAssetPath(currentImage), UnityEditor.ImportAssetOptions.ForceUpdate);
                }
            }
        #endif
    }

    private void AddDetailTexturesToTerrains ()
    {
        List<Terrain> terrains = TerraLand.TerraLandRuntime.croppedTerrains;

        foreach (Terrain t in terrains)
            AddDetailTextures(t, detailBelending, false);

        if(runTime.farTerrain)
        {
            Terrain terrain1 = TerraLand.TerraLandRuntime.firstTerrain;
            Terrain terrain2 = TerraLand.TerraLandRuntime.secondaryTerrain;
            AddDetailTextures(terrain1, Mathf.Clamp(detailBelending * 1f, 0f, 100f), true);
            AddDetailTextures(terrain2, Mathf.Clamp(detailBelending * 1f, 0f, 100f), true);
        }
    }

    private void AddDetailTextures (Terrain terrain, float blend, bool farTerrain)
    {
        int texturesNO = (int)terrain.terrainData.splatPrototypes.Count() + 1;
        int startIndex = terrain.terrainData.splatPrototypes.Count();
        float terrainSize = (runTime.areaSize * 1000f) / (float)runTime.terrainGridSize;

        SplatPrototype[] terrainTextures = new SplatPrototype[texturesNO];

        for (int i = 0; i < texturesNO; i++)
        {
            try
            {
                if(i < startIndex)
                {
                    SplatPrototype currentSplatPrototye = terrain.terrainData.splatPrototypes[i];

                    terrainTextures[i] = new SplatPrototype();
                    if(currentSplatPrototye.texture != null) terrainTextures[i].texture = currentSplatPrototye.texture;

                    if(!farTerrain)
                    {
                        if(detailNormal != null)
                        {
                            terrainTextures[i].normalMap = detailNormal;
                            terrainTextures[i].normalMap.Apply();
                        }
                    }
                    else
                    {
                        if(detailNormalFar != null)
                        {
                            terrainTextures[i].normalMap = detailNormalFar;
                            terrainTextures[i].normalMap.Apply();
                        }
                    }

                    terrainTextures[i].tileSize = new Vector2(currentSplatPrototye.tileSize.x, currentSplatPrototye.tileSize.y);
                    terrainTextures[i].tileOffset = new Vector2(currentSplatPrototye.tileOffset.x, currentSplatPrototye.tileOffset.y);
                    if(currentSplatPrototye.texture != null) terrainTextures[i].texture.Apply();
                }
                else
                {
                    terrainTextures[i] = new SplatPrototype();
                    if(detailTexture != null) terrainTextures[i].texture = detailTexture;

                    if(!farTerrain)
                    {
                        if(detailNormal != null)
                        {
                            terrainTextures[i].normalMap = detailNormal;
                            terrainTextures[i].normalMap.Apply();
                        }
                    }
                    else
                    {
                        if(detailNormalFar != null)
                        {
                            terrainTextures[i].normalMap = detailNormalFar;
                            terrainTextures[i].normalMap.Apply();
                        }
                    }

                    if(!farTerrain)
                        terrainTextures[i].tileSize = new Vector2(detailTileSize, detailTileSize);
                    else
                        terrainTextures[i].tileSize = new Vector2(detailTileSize * 200f, detailTileSize * 200f);

                    terrainTextures[i].tileOffset = Vector2.zero;
                    if(detailTexture != null) terrainTextures[i].texture.Apply();
                }
            }
            catch(Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
        }

        terrain.terrainData.splatPrototypes = terrainTextures;

        int length = terrain.terrainData.alphamapResolution;
        float[,,] smData = new float[length, length, texturesNO];

        try
        {
            for(int y = 0; y < length; y++)
            {
                for(int z = 0; z < length; z++)
                {
                    if(texturesNO > 1)
                    {
                        smData[y, z, 0] = 1f - (blend / 100f);
                        smData[y, z, 1] = blend / 100f;
                    }
                    else
                        smData[y, z, 0] = 1f;
                }
            }

            terrain.terrainData.SetAlphamaps(0, 0, smData);
        }
        catch(Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }

        terrain.terrainData.RefreshPrototypes();
        terrain.Flush();

        terrainTextures = null;
        smData = null;
    }

    public void QuitToMainMenu ()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    void OnGUI ()
    {
        if(gameLoaded)
        {
            GUI.backgroundColor = new UnityEngine.Color(0.3f, 0.3f, 0.3f, 0.3f);
            GUI.Box(new Rect(10, Screen.height - 35, 220, 22), "Lat: " + playerLat.ToString("0.000000") + "   Lon: " + playerLon.ToString("0.000000"));
        }
    }

    private void UnloadResources ()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    public void OnEnable()
    {
        UnloadResources();
    }

    public void OnDisable()
    {
        UnloadResources();
    }
}

