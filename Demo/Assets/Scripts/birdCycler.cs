using UnityEngine;
using UnityEngine.UI;

public class birdCycler : MonoBehaviour {
 
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
    private int birdNum;
    private Texture2D birdTexture;
    private Text textField;
    int i = 0;

    void Awake() {
        
    }

    void prepBirdData(){
          currentDataPoint = targetDataPoint.GetDataPoint();
          megaPoint = GameObject.Find(currentDataPoint);      
          dataPoint = megaPoint.GetComponent<DataPoint>();
          birdNum = dataPoint.BirdTypeQuantity;
    }

    void pullBirdCode(int num){
          birdCode = dataPoint.GetBirdCode(num);
    }

    void pullBirdCount(int num){
          birdCount = dataPoint.GetBirdCount(num);
    }

    public void loadWiki(){
        birdTexture = (Texture2D)Resources.Load("WIKI/"+ birdCode);
        wikiImage.GetComponent<RawImage>().texture = birdTexture;
        textField.text = birdCount.ToString();
        wikiCanvas.SetActive(true);
    }

    public void nextWiki(){
        //Debug.Log(i);
        if (i < (birdNum - 1)){
            i++;
            pullBirdCode(i);
            pullBirdCount(i);
            loadWiki();
          }
        else if(i == (birdNum - 1)){
            i = 0;
            pullBirdCode(i);
            pullBirdCount(i);
            loadWiki();
        }
    }

    public void prevWiki(){
        //Debug.Log(i);
        if (i > (0))
        {
            i--;
            pullBirdCode(i);
            pullBirdCount(i);
            loadWiki();
        }
        else if (i == 0)
        {
            i = (birdNum - 1);
            pullBirdCode(i);
            pullBirdCount(i);
            loadWiki();
        }

    }

    public void closeWiki()
    {
        wikiCanvas.SetActive(false);
    }

    void Start() {
        targetData = GameObject.Find("TargetData");
        wikiCanvas = GameObject.Find("WikiCanvas");
        wikiImage = GameObject.Find("Wiki");
        wikiNum = GameObject.Find("NumText");
        textField = wikiNum.GetComponent<Text>();
        targetDataPoint = targetData.GetComponent<TargetDataPoint>();
	    currentDataPoint = null;

        prepBirdData();

    }
}