//using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEditor;

using System;
using System.IO;

using System.Windows.Forms;

public class ClickEvent : MonoBehaviour {

	Scrollbar scroll;
	GameObject panel;
	public GameObject up,down,left,right;
	public GameObject clone,wave;
	public GameObject checkButton,BannerImageButton,LyricsButton,CDLogoButton,MusicFileButton;
	int resolution = 60; // bpm 단위시간당 측정하는 데이터 값. bpm이라 할 수 있음.
	int EntitySong =0;
	float[] waveForm;
	float[] samples;
	AudioSource au;
	int[,] NoteSave;
	public GameObject plane;
	//float BPM = 0.001f; // song velocity
	int UPkey = 1,DOWNkey = 1,RIGHTkey = 1,LEFTkey = 1,UP = 1,DOWN = 1; 
	// keyflag 이 값으로 키의 중복을 막는다.
	// 이 값이 없이 할 경우 한번누를때 여러번 눌러지는 버그가 있음.
	int upDown = 0; // initialing notesave variable 
	static int NoteLength = 1024;
	//notesave vriable로 파일을 저장시킨다.
	string r,d,u,l; //right,down,up,left => 0,1,2,3
	
	public string filePath = "";
	public Texture2D texture;

	void Start () {
		componentInit ();
		GameObject.Find ("BPMText").GetComponent<InputField> ().text = "240.0000";
	}

	// Update is called once per frame
	void Update () {
		try{
			double current = au.time / au.clip.length;
			//Debug.Log (current+"//"+(float)(current));
			scroll.value = (float)(1 - current);
            Debug.Log("scroll value : "+scroll.value);
            Debug.Log("EntitySong : " + EntitySong);
            //시간당 스크롤 값이 진행 하는 속도.
            // 전체 스크롤 길이에 알맞는 배열 증가 속도
            // (int)((1 - scroll.value) * EntitySong//전체 노래 길이)
            upDown = (int)((1 - scroll.value) * EntitySong);
        }
        catch(Exception e){
			Debug.Log(e);
		}
	}


	void OnGUI(){
		InputKeyEvnet ();

		if (GUI.Button (new Rect (0, 0, 100, 50), "play")) {
			playSong();
		}
		if (GUI.Button (new Rect (0, 50, 100, 50), "pause")) {
			pauseSong ();
		}
		if (GUI.Button (new Rect (0, 100, 100, 50), "stop")) {
			stopSong ();
		}
	}

	void componentInit(){
		panel = GameObject.Find ("ScrollPanel");
		up= GameObject.Find ("UpStep");
		down= GameObject.Find ("DownStep");
		left= GameObject.Find ("LeftStep");
		right= GameObject.Find ("RightStep");
		scroll = GetComponent<Scrollbar>();
		scroll = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
		wave = GameObject.Find ("Wave");
		au = GetComponent<AudioSource> ();
		checkButton= GameObject.Find ("CheckButton");
		BannerImageButton= GameObject.Find ("BannerImageButton");
		LyricsButton= GameObject.Find ("LyricsButton");
		CDLogoButton= GameObject.Find ("CDLogoButton");
		MusicFileButton= GameObject.Find ("MusicFileButton");

		BannerImageButton.GetComponent<UnityEngine.UI.Button> ().interactable = false;
		LyricsButton.GetComponent<UnityEngine.UI.Button> ().interactable = false;
		CDLogoButton.GetComponent<UnityEngine.UI.Button> ().interactable = false;
	}

	void MakeCloneObj(GameObject resource,GameObject obj,string name){
		clone = Instantiate(resource) as GameObject;
		clone.transform.parent = panel.transform;
		clone.transform.position = obj.transform.position;
		clone.transform.rotation = obj.transform.rotation;
		clone.transform.localScale = obj.transform.localScale;
		clone.transform.name = name;
	}		



	void audioVariableInit(){        
		resolution = au.clip.frequency / resolution;  // 샘플링 주파수 / 수집한 시간 단위 당 데이터수 =  분해능 (수집한 데이터수가 클수록 주파수가 더 세밀한 형태가 된다.)
		samples = new float[au.clip.samples*au.clip.channels]; // audio.clip.samples*audio.clip.channels = EnttitySong;
		au.clip.GetData(samples,0); //audio clip의 전체 데이터를 samples배열에 할당 
		waveForm = new float[(samples.Length/resolution)]; 		
		EntitySong = (int)au.clip.length * numNote; // 60bpm의 guide line으로 생성된 노래 전체의 길이
		//waveForm에 초기값 저장, 배열에 해달되는 주파수 값 저

		waveHeightSize = (waveForm.Length-1)+(waveForm.Length-1)*0.1485f; //waveHeightSize는 총길이 
		Debug.Log (waveHeightSize);
		panel.GetComponent<RectTransform> ().sizeDelta = new Vector2(panel.GetComponent<RectTransform> ().sizeDelta.x,waveHeightSize);
		//panel.GetComponent<RectTransform> ().

	}
	
	void DivideWaveValue(){
		waveForm.Initialize ();
		for (int i = 0; i < waveForm.Length; i++)
		{
			waveForm[i] = 0;
			for(int ii = 0; ii<resolution; ii++)//channel
			{
				waveForm[i] += Mathf.Abs(samples[(i * resolution) + ii]); 
				// 2채널 음량의 전체 sample길이에서 60번째의 값만 추출하여 값을 저장함.
				// abs 절대값 추출
			}
			waveForm[i] /= resolution;
			//add bpm sample
		}
	}
	float waveHeightSize = 0;
	//float gap = .1f;
	float waveWidthSize = 120;

	void CreateVectorValue(){
		for (int i = 0; i < waveForm.Length - 1; i++)
		{
			Vector3 sv = new Vector3( waveForm[i] * waveWidthSize ,-i , 0);
			Vector3 ev = new Vector3( -waveForm[i] * waveWidthSize ,-i , 0);
			SomeFunction(sv,ev,i);
		}
	}

	void SomeFunction(Vector3  p0, Vector3 p1,int number)
	{
		clone = Instantiate(wave) as GameObject;
		clone.transform.parent = panel.transform;
		clone.transform.position = wave.transform.position;
		clone.transform.rotation = wave.transform.rotation;
		clone.transform.localScale = wave.transform.localScale;

		LineRenderer lr = clone.AddComponent<LineRenderer> ();
		clone.GetComponent<LineRenderer> ().useWorldSpace = false;
		int t = waveForm.Length - 1 - number;
		clone.transform.name = "wave" + t;
		lr.SetPosition(0,p0);
		lr.SetPosition(1,p1);
	}

	int numNote = 4;
	bool p = true;


	void InputKeyEvnet(){
		//Note 변경을 해주는 부분.  NoteSave의 2차원 배열의 1~4인덱스만 건드는 부분
		if (Input.GetKeyDown (KeyCode.W)) {
			if (UPkey == 1) {
				u = "UpStep(Clone)";
				MakeNodeStep(u,"UpStep",2,up);
			}
            UPkey = 2;
        }
		if (Input.GetKeyDown(KeyCode.S)) {
			if (DOWNkey == 1) {
				d = "DownStep(Clone)";
				MakeNodeStep(d,"DownStep",1,down);							
			}
            DOWNkey = 2;
        } if (Input.GetKeyDown(KeyCode.A)) {
			if (LEFTkey == 1) {
				l = "LeftStep(Clone)";
				MakeNodeStep(l,"LeftStep",0,left);				
			}
            LEFTkey = 2;
        } if (Input.GetKeyDown(KeyCode.D)) {
			if (RIGHTkey == 1) {
				r = "RightStep(Clone)";
				MakeNodeStep(r,"RightStep",3,right);						
			}
            RIGHTkey = 2;
        }
		if (Input.GetKeyUp (KeyCode.W)) {
			UPkey = 1;
		}
		if (Input.GetKeyUp (KeyCode.S)) {
			DOWNkey = 1;
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			LEFTkey = 1;
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			RIGHTkey = 1;
		}

		//위 아래 index 변경 시키는 부분
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if (UP == 1) {
				if(upDown>0){
					upDown--;
					scroll.value += (waveHeightSize/au.clip.length)/(numNote*waveHeightSize);
					//1 : x = 전체길이 : (전체길이/전체시간)/(노드개수); => scroll.value 1번 누를때 증가량
				} // bpm으로 속도를 올린다. 4노트 8노트 16 노트 이런 식으로 증가.
			}
			UP = 2;	
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			if (DOWN == 1) {
				if(upDown<NoteLength/numNote){
					upDown++;
					scroll.value -= (waveHeightSize/au.clip.length)/(numNote*waveHeightSize);
				}
			}
			DOWN = 2;
		}
		if (Input.GetKeyUp(KeyCode.UpArrow)) {
			UP = 1;
		}
		if (Input.GetKeyUp(KeyCode.DownArrow)) {
			DOWN = 1;
		}
        
        if (Input.GetKey(KeyCode.Escape))
        {            
            DialogResult result =  MessageBox.Show("종료!","정말로 종료하시겠습니까?",MessageBoxButtons.YesNo);
            
            switch (result)
            {
                case DialogResult.Yes:   // Yes button pressed
                    UnityEngine.Application.Quit();
                    break;
                case DialogResult.No:    // No button pressed
                    break;
                default:                 // Neither Yes nor No pressed (just in case)
                    MessageBox.Show("Oh noes! What did you press?!?!");
                    break;
            }

        }

        //버튼 관리하는 if 구문 
        //p 는 중복 버튼 클릭 방지를 위한 변수
        if (p) {
			p = false;
			BannerImageButton.GetComponent<UnityEngine.UI.Button> ().onClick.AddListener (() => {
				FileOpenDialog ("png", "BannerImageText", false);
				p = false;
			});
			LyricsButton.GetComponent<UnityEngine.UI.Button> ().onClick.AddListener (() => {
				FileOpenDialog ("png", "LyricsText", false);	//가사 확장명이 뭐지 ?
				p = false;
			});
			CDLogoButton.GetComponent<UnityEngine.UI.Button> ().onClick.AddListener (() => {
				FileOpenDialog ("txt", "CDLogoText", false);
				p = false;
			});
			MusicFileButton.GetComponent<UnityEngine.UI.Button> ().onClick.AddListener (() => {
				FileOpenDialog ("wav", "MusicFileText", true);

				WWW m_get = new WWW("file://"+filePath);
				
				//yield return m_get;
				while(!m_get.isDone)
					;
                
                au.clip = m_get.GetAudioClip(false) as AudioClip;	
				audioVariableInit (); // audio 값들을 초기화 하는 과정.
				DivideWaveValue ();
				CreateVectorValue (); //벡터값을 생성

				NoteSave = new int[EntitySong,4]; // Save note virable 
                // note dimension example PX.wav -> check 1 demension = 112 
                // fixed 2 demension = 4               
                p = false;
			});


			checkButton.GetComponent<UnityEngine.UI.Button> ().onClick.AddListener (() => {
				string title = GameObject.Find ("MusicTitleText").GetComponent<InputField> ().text;
				string save = TextFieldData (); // 저장할 내용을 함수에 정의
				System.IO.Directory.CreateDirectory (title);

				string filepath = title + "/" + title + ".sm"; //dialog.filename은 경로를 나타낸다.

				StreamWriter sw = new StreamWriter (filepath);
				sw.Write ("" + save);
				sw.Close ();
				p = false;
			});
		} else {
			p=false;
		}
		//메소드 끝
	}
	string folderName;
	void MakeNodeStep(string cloneName,string objName,int step,GameObject obj){
		cloneName = cloneName+upDown+step;
		if(NoteSave[upDown,step] == 0){
            
			MakeCloneObj(Resources.Load (objName) as GameObject,obj,cloneName);//make up-note
            
            NoteSave[upDown,step] = 1;
            Debug.Log("NoteSave : " + NoteSave[upDown, step]);
        }
		else if(NoteSave[upDown,step] == 1){
			Destroy(GameObject.Find(cloneName));
			NoteSave[upDown,step] = 0;            
        }
	}

	void FileOpenDialog(string extension,string objname,bool isSing){
		OpenFileDialog openPanel = new OpenFileDialog();
		openPanel.InitialDirectory=@"d:\";
		openPanel.Filter = "CustomFile (*."+extension+")|*."+extension+"| All files (*.*)|(*.*)";
		if(openPanel.ShowDialog() == DialogResult.OK)
		{
			Debug.Log("OK      " +openPanel.FileName+ " ");
			filePath = openPanel.FileName;
		}

		if (filePath.Length != 0)
		{
			/// *이 부분은 처음에 음악파일 선택할때 필요한 부분. 
			// * 음악파일을 선택하게되면 음악제목을 따서 폴더를 만들고 음악파일을 카피해서 폴더안에 복사해 둔다.
			if(isSing){
				//노래 제목으로 폴더를 만들고 그 안에 노래파일을 저장한다.
				folderName = getFileName(getFileNameExtension(filePath)); // 폴더이름
				// 파일이름확장명에서 파일이름만 가져오는 메소드
				System.IO.Directory.CreateDirectory (folderName); 
				//처음에 음악파일 설정할때 디렉토리 생성을 한다.
				GameObject.Find ("MusicTitleText").GetComponent<InputField> ().text = getFileName(getFileNameExtension(filePath));
			
			}
			string fileNameExtension = getFileNameExtension(filePath); // 파일이름+확장명
			
			try{			
				//파일이름과 확장명을 가져오는 메소드
				System.IO.File.Copy(filePath, folderName+"/"+fileNameExtension); 
				System.IO.File.Copy(filePath, "assets/resources/"+fileNameExtension);
			}catch(IOException e){
				Debug.Log ("IOExeption "+e);
			}
			GameObject.Find (objname).GetComponent<InputField> ().text = fileNameExtension;
			BannerImageButton.GetComponent<UnityEngine.UI.Button> ().interactable = true;
			LyricsButton.GetComponent<UnityEngine.UI.Button> ().interactable = true;
			CDLogoButton.GetComponent<UnityEngine.UI.Button> ().interactable = true;
			Debug.Log (fileNameExtension);
		}
		p=true;
	}
	string getFileNameExtension(string path){
		string[] str = new string[100];
		str = filePath.Split('\\'); // 노래제목 + 확장명 가져오기 위한 배열
		return str [str.Length - 1];
	}
	string getFileName(string nameExtension){
		string[] title = new string[10];
		title = nameExtension.Split('.');
		return title [0]; // 노래 제목만 반환
	}


	string Text1,sum;
	string TextFieldData(){
		Text1 = GameObject.Find ("MusicTitleText").GetComponent<InputField> ().text;
		Debug.Log (Text1);
		/*
		for (int i = 0; i < NoteSave.Length-1; i++) {
			for (int j =0; j < numNote; j++) {
				NoteSave [i, j] = 0;
			}
		}*/
		sum = "#TITLE:" + GameObject.Find ("MusicTitleText").GetComponent<InputField> ().text + ";\r\n" +
			"#SUBTITLE:" + GameObject.Find ("MusicSubTitleText").GetComponent<InputField> ().text + ";\r\n" +
			"#ARTIST:" + GameObject.Find ("MusicComposerText").GetComponent<InputField> ().text + ";\r\n" +
			"#TITLETRANSLIT:" + GameObject.Find ("TranslationText").GetComponent<InputField> ().text + ";\r\n" +
			"#SUBTITLETRANSLIT:" + GameObject.Find ("TranslationSubText").GetComponent<InputField> ().text + ";\r\n" +
			"#ARTISTTRANSLIT:" + GameObject.Find ("TranslationComposerText").GetComponent<InputField> ().text + ";\r\n" +
				"#GENRE:" + /*"YES;" + 여기다 뭐 넣어야 하지 ? 실수로 지움 수정아 ㅎㅎㅎ 채워넣어줭*/ ";\r\n" +
				"#CREDIT:" + GameObject.Find ("SMFileMakerText").GetComponent<InputField> ().text + ";\r\n" +
			"#BANNER:" + GameObject.Find ("BannerImageText").GetComponent<InputField> ().text + ";\r\n" + 
				//System.IO.File.Copy(sourceFilePath, destFilePath);
			"#BACKGROUND:" + "" + ";\r\n" +
				//System.IO.File.Copy(sourceFilePath, destFilePath);
			"#LYRICSPATH:" + GameObject.Find ("LyricsText").GetComponent<InputField> ().text + ";\r\n" +
				//System.IO.File.Copy(sourceFilePath, destFilePath);
			"#CDTITLE:" + GameObject.Find ("CDLogoText").GetComponent<InputField> ().text + ";\r\n" +
			"#MUSIC:" + GameObject.Find ("MusicFileText").GetComponent<InputField> ().text + ";\r\n" +
				//System.IO.File.Copy(sourceFilePath, destFilePath);
			
			"#OFFSET:" + GameObject.Find ("OffsetText").GetComponent<InputField> ().text + ";\r\n" +
			"#SAMPLESTART:" + GameObject.Find ("StatingPreviewText").GetComponent<InputField> ().text + ";\r\n" +
			"#SAMPLELENGTH:" + GameObject.Find ("LengthPreviewText").GetComponent<InputField> ().text + ";\r\n" +
			"#SELECTABLE:" + "YES;" + ";\r\n" + /*No" 로 바꿀경우 노래 고르는 창에서 못고르게 하고 "Roulette 혹은 엑스트라 스테이지" 에서만 플레이 가능*/
			"#BPMS:0.000=" + GameObject.Find ("BPMText").GetComponent<InputField> ().text + ";\r\n" + // 0.000=180.000 0비트에서 180bpm속도로 진행한다. 분당 180회 의 ex) 초당 4번  240bpm
			"#STOPS:" + GameObject.Find ("StopPositionText").GetComponent<InputField> ().text + ";\r\n" +
			"#BGCHANGE:" + GameObject.Find ("BannerChangeText").GetComponent<InputField> ().text + ";\r\n" + /*예: #BGCHANGE: 60.000=space of soul.avi,80.000=space of soul-bg.png; = 60번째 비트에서 영상으로 바꾸고 80번째 비트에서 이미지로 바꿈.*/
				//System.IO.File.Copy(sourceFilePath, destFilePath);
			"#KEYSOUNDS:"+";\r\n" +


			"\r\n//---------------dance-single - Blank----------------//\r\n"+
			//Note 기본정보
			"#NOTES:\r\n" +
			"\tdance-single:\r\n" + 
			"\t:\r\n" +
			"\tHard:\r\n" +
			"\t4:\r\n" +
			"\t1.000,0.585,1.000,0.051,0.890:\r\n";


		//for문으로 numNote의 개수마다 콤마를 찍어 마디를 구분해 준다.

		for (int i = 0; i < NoteSave.Length/numNote; i++) {			
			for(int j =0 ; j < numNote;j++){
				Debug.Log("i : "+i+"\nj :"+j+"\nNoteSave :"+NoteSave[i,j]);
				sum += NoteSave[i,j];
			}
			sum+="\r\n";
			if(i==NoteSave.Length/numNote-1){
				break;
			}

			if((i+1) % numNote==0){
				sum+=",\r\n";
			}
		}

		sum += ";";
		return sum;
	}
	bool playing = false;

	void playSong(){
		if(!playing){
			playing = true;
			au.Play();	
		}
	}

	void pauseSong(){
		if (playing) {
			au.Pause();
			playing = false;
		}
	}

	void stopSong(){
		au.Stop();
		playing = false;
		try{
		AudioClip test = null;
		au.clip = test;
            //audioVariableInit (); // audio 값들을 초기화 하는 과정.
            //DivideWaveValue ();
            //CreateVectorValue (); //벡터값을 생성
            Transform[] testss = panel.GetComponentsInChildren<Transform>();
            foreach(Transform te in testss){
                Debug.Log("name"+ te.gameObject.name);
                if(te.gameObject.name != "ScrollPanel" && te.gameObject.name != "Wave")
                    Destroy(te.gameObject);
            }
        resolution = 60; //not initalize 
        samples.Initialize();
        waveForm.Initialize();
        EntitySong = 0;
        NoteSave.Initialize();
        scroll.value = 1;
		p = false;
		}catch(Exception e){
			Debug.Log(e);
		}
	}

}