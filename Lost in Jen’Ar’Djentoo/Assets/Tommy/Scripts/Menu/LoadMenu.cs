using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour {

    public static bool isInGame = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick(){

        if(!isInGame) SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        else
        {
            isInGame = false;
            SceneManager.UnloadSceneAsync("Options");
        }
    }
}
