using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMusic : MonoBehaviour {

    private static BackMusic instance = null; //variabile che contiene l'istanza della musica di background

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static BackMusic Instance {
        get { return instance; }
    }
	//funzione che permette di mantenere la musica di gioco tra le varie sezioni del menu (obsoleta)
    void Awake() {
        if (instance != null && instance != this) { //se è già presente un istanza e se è diversa da quella attuale
            Destroy(this.gameObject); //distruggi l'istanza attuale
            return;
        } else instance = this; //altrimenti mantienila nella variabile
        DontDestroyOnLoad(this.gameObject); //non distruggere al cambio di scena
    }

	//fonte del codice: https://answers.unity.com/questions/11314/audio-or-music-to-continue-playing-between-scene-c.html
}
