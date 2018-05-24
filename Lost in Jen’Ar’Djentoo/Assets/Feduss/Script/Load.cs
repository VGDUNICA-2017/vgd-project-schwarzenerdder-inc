using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Load : MonoBehaviour {

    //LoadingBar --> https://www.youtube.com/watch?v=YMj2qPq9CP8

    private InventorySystem isys;
    private GameObject player;
    private HUDSystem hud;
    private Misc misc;
    private Scene m_Scene;

    public GameObject loadingScreen;
    public Slider slider;
    public Text loadingProgress;
    public GameObject premiper;
    public static bool new_game;

    private GameObject torcia;
    private GameObject ascia;
    private GameObject pistola;
    private GameObject smg;
    private GameObject final_key;
    private GameObject cutter;
    private GameObject boss_door1;

    // Use this for initialization
    void Start () {
        m_Scene = SceneManager.GetActiveScene();

        if (m_Scene.name.Equals("Scena 1 - Il massiccio"))
        {
            torcia = GameObject.Find("la Torcia");
            ascia = GameObject.Find("l'ascia");
            pistola = GameObject.Find("P226");
            smg = GameObject.Find("MP5");
            final_key = GameObject.FindGameObjectWithTag("FinalKey");
            cutter = GameObject.FindGameObjectWithTag("Cutter");
            boss_door1 = GameObject.Find("door_2");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("l"))
        {
            Load_("Scena 1 - Il massiccio");
        }
	}

    public void OnLevelWasLoaded(int level)
    {
        print(gameObject.name);
        if (SceneManager.GetActiveScene().name.Equals("Scena 1 - Il massiccio") && new_game==false)
        {
            torcia = GameObject.Find("la Torcia");
            ascia = GameObject.Find("l'ascia");
            pistola = GameObject.Find("P226");
            smg = GameObject.Find("MP5");
            final_key = GameObject.FindGameObjectWithTag("FinalKey");
            cutter = GameObject.FindGameObjectWithTag("Cutter");
            boss_door1 = GameObject.Find("door_2");

            Load_("Scena 1 - Il massiccio");
        }

        //if(gameObject.name.Equals("LoadingController")) Destroy(gameObject);
    }

    public void Load_(string scene_name)
    {
        print(SceneManager.GetActiveScene().name);
        if (File.Exists(Application.persistentDataPath + "/save.dat") && scene_name.Equals("Scena 1 - Il massiccio"))
        {
            print(GameObject.FindGameObjectWithTag("Player").gameObject.name);
            isys = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
            hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
            player = GameObject.FindGameObjectWithTag("Player");
            misc = player.GetComponent<Misc>();

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);

            SceneData data = (SceneData)formatter.Deserialize(file);
            file.Close();

            isys.LoadPlayer(data.pdata);

            //Per ogni nemico nella scena (inizialmente sono tutti attivi, anche quelli da spawnare via script)
            foreach (GameObject nemico in GameObject.FindGameObjectsWithTag("Enemy")) { 
                //Per ogni enemydata presente nella lista degli enemy salvatii
                foreach(Enemy enemy in data.edata)
                {
                    //Se il nome del nemico è presente nella lista, allora carico le sue info
                    if (nemico.gameObject.name.Equals(enemy.name))
                    {
                        nemico.GetComponent<EnemyController>().loadEnemy(enemy);
                        nemico.GetComponent<EnemyController>().loaded = true;
                    }
                }
            }

            //Elimino i nemici che non sono stati caricati
            foreach(GameObject nemico in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (nemico.GetComponent<EnemyController>().loaded == false)
                {
                    Destroy(nemico);
                }
            }

            //Trovo gli elementi da mantenere nella scena, impostando a true loaded (quelli a false andranno eliminati nel prossimo ciclo for)
            foreach (string nome in data.idata)
            {
                foreach (GameObject kitmedico in GameObject.FindGameObjectsWithTag("FirstAid"))
                {
                    if (kitmedico.gameObject.name.Equals(nome))
                    {
                        kitmedico.GetComponent<Pickup>().loaded = true;
                    }
                }

                foreach (GameObject ammo_9mm in GameObject.FindGameObjectsWithTag("Ammo_9mm"))
                {
                    if (ammo_9mm.gameObject.name.Equals(nome))
                    {
                        ammo_9mm.GetComponent<Pickup>().loaded = true;
                    }
                }

                foreach (GameObject ammo_smg in GameObject.FindGameObjectsWithTag("Ammo_smg"))
                {
                    if (ammo_smg.gameObject.name.Equals(nome))
                    {
                        ammo_smg.GetComponent<Pickup>().loaded = true;
                    }
                }
                
                if (torcia!=null && torcia.gameObject.name.Equals(nome))
                {
                    torcia.GetComponent<Pickup>().loaded = true;
                }

                if (ascia != null && ascia.gameObject.name.Equals(nome))
                {
                    ascia.GetComponent<Pickup>().loaded = true;
                }

                if (pistola != null && pistola.gameObject.name.Equals(nome))
                {
                    pistola.GetComponent<Pickup>().loaded = true;
                }

                if (smg != null && smg.gameObject.name.Equals(nome))
                {
                    smg.GetComponent<Pickup>().loaded = true;
                }

                if (final_key != null && final_key.gameObject.name.Equals(nome))
                {
                    final_key.GetComponent<KeyScript>().loaded = true;
                }

                if (cutter != null && cutter.gameObject.name.Equals(nome))
                {
                    cutter.GetComponent<KeyScript>().loaded = true;
                }
            }

            //Elimino gli oggetti che non servono
            foreach (GameObject kitmedico in GameObject.FindGameObjectsWithTag("FirstAid"))
            {
                if (kitmedico.GetComponent<Pickup>().loaded==false)
                {
                    Destroy(kitmedico);
                }
            }

            foreach (GameObject ammo_9mm in GameObject.FindGameObjectsWithTag("Ammo_9mm"))
            {
                if (ammo_9mm.GetComponent<Pickup>().loaded == false)
                {
                    Destroy(ammo_9mm);
                }
            }

            foreach (GameObject ammo_smg in GameObject.FindGameObjectsWithTag("Ammo_smg"))
            {
                if (ammo_smg.GetComponent<Pickup>().loaded == false)
                {
                    Destroy(ammo_smg);
                }
            }

            if (torcia!=null && torcia.GetComponent<Pickup>().loaded == false)
            {
                Destroy(torcia);
            }

            if (ascia != null && ascia.GetComponent<Pickup>().loaded == false)
            {
                Destroy(ascia);
            }

            if (pistola != null && pistola.GetComponent<Pickup>().loaded == false)
            {
                Destroy(pistola);
            }

            if (smg != null && smg.GetComponent<Pickup>().loaded == false)
            {
                Destroy(smg);
            }

            if (final_key != null && final_key.GetComponent<KeyScript>().loaded == false)
            {
                Destroy(final_key);
            }

            if (cutter != null && cutter.GetComponent<KeyScript>().loaded == false)
            {
                Destroy(cutter);
            }

            //Setto gli eventi
            if (data.checkpoint_name.Equals("Pre-MiniBoss"))
            {
                GameObject.FindGameObjectWithTag("Recinzione").GetComponent<Animator>().SetBool("Open", true);
            }

            if (data.checkpoint_name.Equals("Post-MiniBoss"))
            {
                Destroy(GameObject.FindGameObjectWithTag("MiniBoss"));
                boss_door1.GetComponent<Animator>().SetTrigger("Boss dies");
            }

            if (data.checkpoint_name.Equals("Pre-Boss"))
            {
                foreach(GameObject porta in GameObject.FindGameObjectsWithTag("Porta"))
                {
                    if (porta.GetComponent<DoorBrokenDown>() != null)
                    {
                        porta.GetComponent<DoorBrokenDown>().brokenOnLoading();
                    }
                }
            }



        }
    }

    
    //Caricamento asincrono della scena indicata per mostrare la barra di caricamento (Il tutto dal menù principale)
    IEnumerator LoadAsync(string scene_name)
    {
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_name); //Caricamento asincrono della scena (mi restituisc un oggetto con info utili)
        loadingScreen.SetActive(true); //Abilito la barra di loading
        foreach (GameObject bottone in GameObject.FindGameObjectsWithTag("BottoniMenu"))
        {
            bottone.SetActive(false);
        }
        operation.allowSceneActivation = false; //Non permetto l'attivazione della scena quando il caricamento viene effettuato (Utile per far premere un tasto prima di attivarla..da implementare?)

        //Fino a quando la scena non è "pronta"
        while (!operation.isDone)
        {
            //Calcolo e aggiorno il valore della barra di caricamento
            float progress = Mathf.Clamp01(operation.progress / .9f); //Serve per portare il valore di caricamento tra 0 e 1, anzichè tra 0 e 0.9 (i valori da 0.9 a 1 sono poco usati, e solo durante l'attivazione delle scene)
            slider.value = progress;
            loadingProgress.text = progress*100f + "%";

            //Quando il caricamento è finito
            if (operation.progress == .9f && loadingProgress.text.Equals("100%"))
            {
                premiper.SetActive(true);

                if (Input.GetButtonDown("Open Door"))
                {
                    operation.allowSceneActivation = true;                    
                }
                yield return null;
            }
        }               

    }

    public void OnClick(){
        //Se si trova nel menù principale, e preme il bottone di Load (il gameobject che contiene la script)
        if (gameObject.name.Equals("LoadingController"))
        {
            Destroy(GameObject.Find("Music"));

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);

            SceneData data = (SceneData)formatter.Deserialize(file);
            string scene_name = data.scena_name;
            file.Close();
            if (EventSystem.current.currentSelectedGameObject.name.Equals("Load")) new_game = false;
            else new_game = true;
            StartCoroutine(LoadAsync(scene_name));
        }
    }
}
