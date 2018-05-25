using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    //tutorial --> https://www.youtube.com/watch?v=JivuXdrIHK0

    public static bool GameIsPaused=false;
    public GameObject PauseMenuUI;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

	}

    public void Pause()
    {
        PauseMenuUI.SetActive(true); //Attimo il menù di pausa
        Time.timeScale = 0f; //"Fermo" il tempo
        GameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>().enabled = false;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GetComponentInParent<HUDSystem>().centralBoxEnabler(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>().enabled = true;
    }

    public void LoadMenu_()
    {
        GameIsPaused = true;
        SceneManager.LoadScene("Menu");
    }

    public void Options()
    {
        LoadMenu.isInGame = true;
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void WarningMessageEnter()
    {
        GetComponentInParent<HUDSystem>().centralBoxEnabler(true);
        GetComponentInParent<HUDSystem>().centralBoxText("Selezionando questa opzione i dati non salvati andranno persi!");
        
    }
    public void WarningMessageExit()
    {
        GetComponentInParent<HUDSystem>().centralBoxEnabler(false);
        
    }
    
}
