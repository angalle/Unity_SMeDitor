using UnityEngine;
using System.Collections;

public class workNextBtnSrc : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clicked()
    {
        Application.LoadLevel("selectGameMode");
        Debug.Log("test");
    }
}
