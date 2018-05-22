using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClick() {
        Destroy(GameObject.Find("Music"));
        SceneManager.LoadScene("Scena 1 - Il massiccio", LoadSceneMode.Single);
    }
}
