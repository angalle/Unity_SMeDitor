using UnityEngine;
using System.Collections;

public class gameModeNextBtnSrc : MonoBehaviour {
    GameObject ddrBtn, pumpBtn, previousBtn;
    GameObject songTitle;
    public static string gameMode = "";
    bool clickable = true;
    // Use this for initialization

    void Start () {
        ddrBtn = GameObject.Find("ddrBtn");
        pumpBtn = GameObject.Find("pumpBtn");
        songTitle = GameObject.Find("songTitle");
        previousBtn = GameObject.Find("previousBtn");
        pumpBtn.GetComponent<UnityEngine.UI.Button>().interactable = false;        
        clickable = true;
        if (songNextBtnScr.fileName != "")
            songTitle.GetComponent<UnityEngine.UI.Text>().text = songNextBtnScr.fileName;
        else
            songTitle.GetComponent<UnityEngine.UI.Text>().text = "노래를 선택 안하셨습니다.";
    }
	
	// Update is called once per frame
	void Update () {
        if (clickable) {
            ddrBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                gameMode = "ddr";
                Application.LoadLevel("selectLevel");
            });
            /*
            pumpBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            });
            */
            previousBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {                
                Application.LoadLevel("selectWork");
            });            
            clickable = false;
        }
        
        
    }

    public void OnClick()
    {
        Application.LoadLevel("selectLevel");
        Debug.Log("test");
    }
}
