using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider slider;
    public Text loadingProgress;
    public GameObject LoadingController;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClick() {
        Destroy(GameObject.Find("Music"));
        StartCoroutine(LoadAsync("Scena 1 - Il massiccio"));
    }

    IEnumerator LoadAsync(string scene_name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_name); //Caricamento asincrono della scena (mi restituisc un oggetto con info utili)
        loadingScreen.SetActive(true);
        foreach (GameObject bottone in GameObject.FindGameObjectsWithTag("BottoniMenu"))
        {
            bottone.SetActive(false);
        }

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f); //Serve per portare il valore di caricamento tra 0 e 1, anzichè tra 0 e 0.9
            slider.value = progress;
            loadingProgress.text = (int)progress * 100f + "%";
            DontDestroyOnLoad(LoadingController);
            yield return null;
        }
    }
}
