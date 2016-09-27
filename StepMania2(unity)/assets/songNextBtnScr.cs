using UnityEngine;
using System.Collections;
using System.Windows.Forms;

public class songNextBtnScr: MonoBehaviour {
    private string extension = "wav";
    public static string filePath = "",fileName="", fileNameExtension="";
    GameObject loadBtn,nextBtn;
    bool clickable = true;
    // Use this for initialization
    void Start () {
        loadBtn = GameObject.Find("loadBtn");
        nextBtn = GameObject.Find("nextBtn");
        nextBtn.GetComponent<UnityEngine.UI.Button>().interactable = false;
    }
	
	// Update is called once per frame
	void Update () {           
            loadBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {                
                if (clickable)
                {
                     //LoadFile();
                     nextBtn.GetComponent<UnityEngine.UI.Button>().interactable = true;
                }
            });              
    }

    public void Clicked()
    {        
            UnityEngine.Application.LoadLevel("selectWork");
            Debug.Log("test");                          
    }

    public void LoadFile()
    {
        OpenFileDialog openPanel = new OpenFileDialog();
        openPanel.InitialDirectory = @"d:\";
        openPanel.Filter = "CustomFile (*." + extension + ")|*." + extension + "| All files (*.*)|(*.*)";
        DialogResult result = openPanel.ShowDialog();
        if (result == DialogResult.OK)
        {
            Debug.Log("OK      " + openPanel.FileName + " ");
            filePath = openPanel.FileName;
            fileName = getFileName(getFileNameExtension(filePath));
            fileNameExtension = getFileNameExtension(filePath);
            UnityEngine.Application.LoadLevel("selectWork");
        }
        else 
        {
            
        }              
    }

    string getFileNameExtension(string path)
    {
        string[] str = new string[100];
        str = filePath.Split('\\'); // 노래제목 + 확장명 가져오기 위한 배열
        return str[str.Length - 1];
    }

    string getFileName(string nameExtension)
    {
        string[] title = new string[10];
        title = nameExtension.Split('.');
        return title[0]; // 노래 제목만 반환
    }

}
