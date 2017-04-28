using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class loadWiki : MonoBehaviour
{
    private GameObject targetData;
    private GameObject megaPoint;
    private GameObject wikiCanvas;
    private GameObject wikiImage;
    private DataPoint dataPoint;
    private TargetDataPoint targetDataPoint;
    private string currentDataPoint;
    private string birdCode;
    private int birdCount;
    private int birdNum;
    private Texture2D birdTexture;


    void prepBirdData()
    {
        currentDataPoint = targetDataPoint.GetDataPoint();
        megaPoint = GameObject.Find(currentDataPoint);
        dataPoint = megaPoint.GetComponent<DataPoint>();
        birdNum = dataPoint.BirdTypeQuantity;
    }

    void pullBirdCodeStart()
    {
        birdCode = dataPoint.GetBirdCode(0);
    }

    void pullBirdCountStart()
    {
        birdCount = dataPoint.GetBirdCount(0);
    }

    void fillWiki()
    {
        wikiCanvas.SetActive(true);
        birdTexture = (Texture2D)Resources.Load("WIKI/" + birdCode + ".png");
        wikiImage.GetComponent<RawImage>().texture = birdTexture;
    }

    void Start()
    {
        targetData = GameObject.Find("TargetData");
        wikiCanvas = GameObject.Find("WikiCanvas");
        wikiImage = GameObject.Find("Wiki");
        targetDataPoint = targetData.GetComponent<TargetDataPoint>();
        currentDataPoint = null;

        prepBirdData();
        pullBirdCodeStart();
        pullBirdCountStart();
        fillWiki();

    }
}




