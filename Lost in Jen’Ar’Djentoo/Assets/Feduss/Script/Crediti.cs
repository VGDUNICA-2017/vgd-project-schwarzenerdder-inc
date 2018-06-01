using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crediti : MonoBehaviour {

	public void toCrediti() {
        SceneManager.LoadScene("Crediti", LoadSceneMode.Single);
    }

    public void toMenu() {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
