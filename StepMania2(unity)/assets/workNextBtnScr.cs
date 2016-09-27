using UnityEngine;
using System.Collections;

public class workNextBtnScr : MonoBehaviour {
    GameObject newBtn,modifyBtn;
    GameObject songTitle, previousBtn;
    bool clickable = true;
    // Use this for initialization


    void Start () {
        newBtn = GameObject.Find("newMakeBtn");
        modifyBtn = GameObject.Find("modifyBtn");
        songTitle = GameObject.Find("songTitle");
        previousBtn = GameObject.Find("previousBtn");
        modifyBtn.GetComponent<UnityEngine.UI.Button>().interactable = false;
        if(songNextBtnScr.fileName != "")
            songTitle.GetComponent<UnityEngine.UI.Text>().text = songNextBtnScr.fileName;
        else
            songTitle.GetComponent<UnityEngine.UI.Text>().text = "노래를 선택 안하셨습니다.";
        clickable = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (clickable) {
            newBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                Application.LoadLevel("selectGameMode");
            });
            /*
            modifyBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {

            });
            */
            previousBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                Application.LoadLevel("selectSong");
            });
            clickable = false;
        }
    }

    public void OnClick()
    {
        Application.LoadLevel("selectGameMode");
        Debug.Log("test");
    }
    
    
    
}
