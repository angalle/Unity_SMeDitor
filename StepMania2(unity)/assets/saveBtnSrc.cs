using UnityEngine;
using System.Collections;
using System.IO;

public class saveBtnSrc : MonoBehaviour {
    public static string SaveContents = "";
    bool clickable = true;
    GameObject btn, previousBtn;
    bool drawGUI = false;


    private Rect windowRect = new Rect(200, 180, 400, 100);
    // Use this for initialization
    void Start () {
        btn = GameObject.Find("exitBtn");
        previousBtn = GameObject.Find("previousBtn");
        clickable = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (clickable)
        {
            btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                Application.Quit();
            });
            previousBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                Application.LoadLevel("selectLevel");
            });
            clickable = false;
        }
    }
    
    public void save()
    {
        string filePath = songNextBtnScr.filePath;        
        string fileName = songNextBtnScr.fileName;
        string level = levelNextBtnSrc.level;
        string gameMode = gameModeNextBtnSrc.gameMode;
        string fileNameExtension = songNextBtnScr.fileNameExtension;
        try
        {

            System.IO.Directory.CreateDirectory(fileName);
            System.IO.File.Copy(filePath, fileName /* = folderName*/+ "/" + fileNameExtension);

            string filepath = fileName + "/" + fileName + ".nj"; //dialog.filename은 경로를 나타낸다.

            string sum =
            "#TITLE:" + fileName + ";\r\n" +
            "#SUBTITLE:" + ";\r\n" +
            "#ARTIST:" +  ";\r\n" +
            "#TITLETRANSLIT:" + ";\r\n" +
            "#SUBTITLETRANSLIT:" + ";\r\n" +
            "#ARTISTTRANSLIT:" + ";\r\n" +
            "#GENRE:" + /*"YES;" + 여기다 뭐 넣어야 하지 ? */ ";\r\n" +
            "#CREDIT:" + ";\r\n" +
            "#BANNER:" + ";\r\n" +
            //System.IO.File.Copy(sourceFilePath, destFilePath);
            "#BACKGROUND:" + "" + ";\r\n" +
            //System.IO.File.Copy(sourceFilePath, destFilePath);
            "#LYRICSPATH:" +";\r\n" +
            //System.IO.File.Copy(sourceFilePath, destFilePath);
            "#CDTITLE:" + ";\r\n" +
            "#MUSIC:" + ";\r\n" +
            //System.IO.File.Copy(sourceFilePath, destFilePath);

            "#OFFSET:" + ";\r\n" +
            "#SAMPLESTART:" + ";\r\n" +
            "#SAMPLELENGTH:" + ";\r\n" +
            "#SELECTABLE:" + "YES;" + ";\r\n" + /*No" 로 바꿀경우 노래 고르는 창에서 못고르게 하고 "Roulette 혹은 엑스트라 스테이지" 에서만 플레이 가능*/
            "#BPMS:0.000=" + "240.000"+";\r\n" + // 0.000=180.000 0비트에서 180bpm속도로 진행한다. 분당 180회 의 ex) 초당 4번  240bpm
            "#STOPS:" + ";\r\n" +
            "#BGCHANGE:" + ";\r\n" + 
            /*예: #BGCHANGE: 60.000=space of soul.avi,80.000=space of soul-bg.png; = 60번째 비트에서 영상으로 바꾸고 80번째 비트에서 이미지로 바꿈.*/
            //System.IO.File.Copy(sourceFilePath, destFilePath);
            "#KEYSOUNDS:" + ";\r\n" +


            "\r\n//---------------dance-single - Blank----------------//\r\n" +
            //Note 기본정보
            "#NOTES:\r\n" +
            "\tdance-single:\r\n" +
            "\t:\r\n" +
            "\t"+level+":\r\n" +
            "\t4:\r\n" +
            "\t1.000,0.585,1.000,0.051,0.890:\r\n";


            StreamWriter sw = new StreamWriter(filepath);
            sw.Write("" + sum);
            sw.Close();
        }
        catch (IOException e)
        {
            drawGUI = true;
        }
    }
    
    void OnGUI(){
        if (drawGUI)
        {
            windowRect = GUI.Window(0, windowRect, WindowFunction, "\n\n입-출력 오류 : 이미 존재하는 폴더 명입니다.");
        }
        else
        {
            windowRect = GUI.Window(0, new Rect(0,0,0,0), WindowFunction, "");
        }
        //windowRect = GUI.Window(0, windowRect, WindowFunction, "\n\n입-출력 오류 : 이미 존재하는 폴더 명입니다.");
    }

    void WindowFunction(int windowID)
    {
        if(windowID == 0)
        {

        }
        // Draw any Controls inside the window here
    }
}
