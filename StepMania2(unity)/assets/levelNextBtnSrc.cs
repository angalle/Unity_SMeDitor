using UnityEngine;
using System.Collections;
//using songNextBtnSrc;

public class levelNextBtnSrc : MonoBehaviour {
    public static string level = "";
    // 4번째 페이지
    bool clickable = true;
    GameObject easyBtn, normalBtn, hardBtn, songTitle, previousBtn;
    void Start () {
        easyBtn = GameObject.Find("easyBtn");
        normalBtn = GameObject.Find("normalBtn");
        hardBtn = GameObject.Find("hardBtn");
        songTitle = GameObject.Find("songTitle");
        previousBtn = GameObject.Find("previousBtn");
        if (songNextBtnScr.fileName != "")
            songTitle.GetComponent<UnityEngine.UI.Text>().text = songNextBtnScr.fileName;
        else
            songTitle.GetComponent<UnityEngine.UI.Text>().text = "노래를 선택 안하셨습니다.";
        clickable = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (clickable)
        {
            easyBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                level = "easy";
                Application.LoadLevel("makeNode");
            });
            normalBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                level = "normal";
                Application.LoadLevel("makeNode");
            });
            hardBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                level = "hard";
                Application.LoadLevel("makeNode");
            });
            previousBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {                
                Application.LoadLevel("selectGameMode");
            });

            clickable = false;
        }
        
    }

    public void OnClick()
    {
        if(level != "")
        {
            Application.LoadLevel("makeNode");
            Debug.Log("test");
        }
        else
        {
            Debug.Log("난이도를 선택 안하셨습니다.");
        }
        
    }
}
