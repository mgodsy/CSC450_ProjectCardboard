using UnityEngine;
using UnityEngine.UI;


public class loadWiki : MonoBehaviour
{
    private GameObject targetData;
    private GameObject megaPoint;
    private GameObject wikiCanvas;
    private GameObject wikiImage;
    private GameObject wikiNum;
    private DataPoint dataPoint;
    private TargetDataPoint targetDataPoint;
    private string currentDataPoint;
    private string birdCode;
    private int birdCount;
    private Text textField;
    private string fileName;
    Texture2D thisTexture;
    private RawImage img;
    private Texture2D birdTexture;


    void prepBirdData()
    {
        currentDataPoint = targetDataPoint.GetDataPoint();
        megaPoint = GameObject.Find(currentDataPoint);
        dataPoint = megaPoint.GetComponent<DataPoint>();
    }

    void pullBirdCodeStart()
    {
        birdCode = dataPoint.GetBirdCode(0);
    }

    void pullBirdCountStart()
    {
        birdCount = dataPoint.GetBirdCount(0);
    }

    public void fillWiki()
    {
        wikiCanvas.SetActive(true);
        birdTexture = (Texture2D)Resources.Load("WIKI/" + birdCode);
        wikiImage.GetComponent<RawImage>().texture = birdTexture;
        textField.text = birdCount.ToString();
    }

    void Start()
    {
        targetData = GameObject.Find("TargetData");
        wikiCanvas = GameObject.Find("WikiCanvas");
        wikiImage = GameObject.Find("Wiki");
        wikiNum = GameObject.Find("NumText");
        textField = wikiNum.GetComponent<Text>();
        targetDataPoint = targetData.GetComponent<TargetDataPoint>();
        currentDataPoint = null;

        prepBirdData();
        pullBirdCodeStart();
        pullBirdCountStart();
        wikiCanvas.SetActive(false);

    }
}




