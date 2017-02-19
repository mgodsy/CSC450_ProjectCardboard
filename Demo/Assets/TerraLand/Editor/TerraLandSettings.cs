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
    
    
    INFO: Modifies Project Settings to match up with the original setup so that Terraland operates properly.

    Written by: Amir Badamchi
    
*/


using UnityEngine;
using UnityEditor;
using System;
using System.IO;

[InitializeOnLoad]
public class TerraLandSettings : EditorWindow
{
	const bool forceShow = false;
    private static bool isDebugging = false;

    const ApiCompatibilityLevel recommended_APICompatibilityLevel = ApiCompatibilityLevel.NET_2_0;
    const string recommended_CompanyName = "TerraUnity";
    const string recommended_ProductName = "TerraLand";
    private static Texture2D dialogBanner;
    private static Texture2D[] icon;

    private static bool platformIsWindows;
    private static bool platformIsMac;
    private static string machineConfigPath_Project;
    private static string webConfigPath_Project;
    private static string installationPath;
    private static string wsdlPath;
    private static string machineConfigPath;
    private static string webConfigPath;
    private static string machineConfigPath_Backup;
    private static string webConfigPath_Backup;

    private static MovieTexture videoSlide;
    private static bool isPlayed = false;
    private static Vector2 windowSize = new Vector2(540, 680);
    private static Texture2D logo;
    private static UnityEngine.Color statusColor = UnityEngine.Color.red;
    private static string statusStr = "Project Is Not Setup";
    private static Rect statusRect = new Rect(170, 470, 200, 25);

    static TerraLandSettings window;

    static TerraLandSettings()
	{
		EditorApplication.update += Update;
	}

    static void Initialize ()
    {
        videoSlide = Resources.Load("TerraUnity/Video/TerraLandSlide") as MovieTexture;
        videoSlide.loop = true;
        videoSlide.Play();

        logo = Resources.Load("TerraUnity/Images/Button/Landmap") as Texture2D;
        dialogBanner = Resources.Load("TerraUnity/Images/Logo/TerraLand-Downloader_Logo") as Texture2D;
        icon = new Texture2D[]{Resources.Load("TerraUnity/Images/Logo/Logo") as Texture2D};

        isPlayed = true;
    }

	static void Update ()
	{
		bool show =
            PlayerSettings.apiCompatibilityLevel != recommended_APICompatibilityLevel ||
            !IsConfigSetup() ||
			forceShow;
        
		if (show)
		{
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

            window = GetWindow<TerraLandSettings>(true, "Tournament Settings", true);
            window.position = new Rect
                (
                    (Screen.currentResolution.width / 2) - (windowSize.x / 2),
                    (Screen.currentResolution.height / 2) - (windowSize.y / 2),
                    windowSize.x,
                    windowSize.y
                );
            
            window.minSize = new Vector2(windowSize.x, windowSize.y);
            window.maxSize = new Vector2(windowSize.x, windowSize.y);
		}
        else
        {
            statusColor = UnityEngine.Color.green;
            statusStr = "Everything Is Ok";
        }

		EditorApplication.update -= Update;
	}

	public void OnGUI ()
	{
        Repaint();

        if(!isPlayed)
            Initialize();
		
        if (videoSlide)
            GUI.DrawTexture(new Rect(0, 0, 540, 304), videoSlide, ScaleMode.ScaleToFit);

        GUILayout.Space(330);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Welcome to TerraLand", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Before you start using TerraLand, some settings needs to be modified.");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Press ACCEPT to setup project settings.");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(30);

		GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
		if (GUILayout.Button("ACCEPT"))
		{
            SetSettings();
		}
        GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

        GUI.color = statusColor;

        GUIStyle myStyle = new GUIStyle(GUI.skin.box);
        myStyle.fontSize = 15;
        myStyle.normal.textColor = UnityEngine.Color.black;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.Box(statusRect, new GUIContent(statusStr), myStyle);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUI.color = UnityEngine.Color.white;

        GUILayout.Space(80);

        GUI.backgroundColor = new UnityEngine.Color(1,1,1,0.1f);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(logo))
            UnityEditor.Help.BrowseURL("http://terraunity.com/product/terraland/");
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUI.backgroundColor = UnityEngine.Color.white;
	}

    static void SetSettings ()
    {
        PlayerSettings.apiCompatibilityLevel = recommended_APICompatibilityLevel;
        PlayerSettings.companyName = recommended_CompanyName;
        PlayerSettings.productName = recommended_ProductName;
        PlayerSettings.resolutionDialogBanner = dialogBanner;
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, icon);

        ReplaceConfig();
        Recompile();
    }

    private static void ReplaceConfig ()
    {
        SetupConfigPaths();

        bool isUpdated = false;

        try
        {
            if(File.Exists(machineConfigPath_Backup) && File.Exists(webConfigPath_Backup))
                isUpdated = false;
            else
                isUpdated = true;

            if(!File.Exists(machineConfigPath_Backup))
            {
                File.Move(machineConfigPath, machineConfigPath_Backup);
                File.Copy(machineConfigPath_Project, machineConfigPath, true);
            }

            if(!File.Exists(webConfigPath_Backup))
            {
                File.Move(webConfigPath, webConfigPath_Backup);
                File.Copy(webConfigPath_Project, webConfigPath, true);
            }

            if(isUpdated)
                EditorUtility.DisplayDialog("RESTART REQUIRED", "Please restart Unity. Changes will take effect after restart.", "OK");

            statusColor = UnityEngine.Color.green;
            statusStr = "Everything Is Ok";
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }
    }

    private static bool IsConfigSetup ()
    {
        SetupConfigPaths();

        try
        {
            if(File.Exists(machineConfigPath_Backup) && File.Exists(webConfigPath_Backup))
                return true;
            else
                return false;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }

        return false;
    }

    private static void SetupConfigPaths ()
    {
        platformIsWindows = Application.platform == RuntimePlatform.WindowsEditor;
        platformIsMac = Application.platform == RuntimePlatform.OSXEditor;

        machineConfigPath_Project = Application.dataPath + "/TerraLand/Resources/TerraUnity/Satellite Image Downloader/machine.config";
        webConfigPath_Project = Application.dataPath + "/TerraLand/Resources/TerraUnity/Satellite Image Downloader/web.config";

        String[] appPath = EditorApplication.applicationPath.Split(char.Parse("/"));
        appPath[appPath.Length - 1] = "";
        appPath[appPath.Length - 2] = "";
        installationPath = String.Join("/", appPath).Replace("//", "/");

        if(platformIsWindows) // We are in a Windows system
            wsdlPath = installationPath + "Editor/Data/Mono/etc/mono/2.0/";
        else if(platformIsMac) // We are in a Mac system
            wsdlPath = installationPath + "Applications/Unity/Unity.app/Contents/Frameworks/Mono/etc/mono/2.0/";

        machineConfigPath = wsdlPath + "machine.config";
        webConfigPath = wsdlPath + "web.config";
        machineConfigPath_Backup = wsdlPath + "machine_backup.config";
        webConfigPath_Backup = wsdlPath + "web_backup.config";
    }

    private static void Recompile ()
    {
        string[] assetPaths = AssetDatabase.GetAllAssetPaths();

        foreach(string assetPath in assetPaths)
        {
            if(isDebugging)
            {
                if(assetPath.EndsWith(".cs"))
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            }
            else
            {
                if(assetPath.EndsWith("TerraLand 2.dll"))
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            }
        }

        AssetDatabase.Refresh();
    }
}

