using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Load : MonoBehaviour {

    /// <summary>
    /// authors: feduss, silvio
    /// </summary>
    //LoadingBar --> https://www.youtube.com/watch?v=YMj2qPq9CP8

    private InventorySystem isys;
    private GameObject player;
    private HUDSystem hud;
    private Misc misc;
    private Scene m_Scene;
    private string scene_name;
    public string descrizione;

    public GameObject loadingScreen;
    public Slider slider;
    public Text loadingProgress;
    public GameObject premiper;
    public static bool new_game;
    public GameObject loadButton;

    public GameObject torcia;
    public GameObject ascia;
    public GameObject pistola;
    public GameObject smg;
    public GameObject final_key;
    public GameObject cutter;
    public GameObject boss_door1;

    public GameObject AxeSpawn;
    public GameObject PistolSpawn;
    public GameObject ChainSpawn;
    public GameObject CutterSpawn;
    public GameObject SmgSpawn;




    // Use this for initialization
    void Start () {
        m_Scene = SceneManager.GetActiveScene();
        

        if (m_Scene.name.Equals("Scena 1 - Il massiccio"))
        {
            //torcia = GameObject.Find("la Torcia");
            //ascia = GameObject.Find("l'ascia");
            //pistola = GameObject.Find("P226");
            //smg = GameObject.Find("MP5");
            //final_key = GameObject.FindGameObjectWithTag("FinalKey");
            //cutter = GameObject.FindGameObjectWithTag("Cutter");
            //boss_door1 = GameObject.Find("door_2");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (loadButton != null)
        {
            //Se il file di salvataggio non esiste, oscuro leggermente il bottono e lo rendo non cliccabile
            if (!File.Exists(Application.persistentDataPath + "/save.dat"))
            {
                loadButton.GetComponent<Button>().interactable = false;
            }
            else { 

                loadButton.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void OnLevelWasLoaded(int level)
    {
        Time.timeScale = 1f;
        if (SceneManager.GetActiveScene().name.Equals("Scena 1 - Il massiccio") && new_game==false)
        {
            //torcia = GameObject.Find("la Torcia");
            //ascia = GameObject.Find("l'ascia");
            //pistola = GameObject.Find("P226");
            //smg = GameObject.Find("MP5");
            //final_key = GameObject.FindGameObjectWithTag("FinalKey");
            //cutter = GameObject.FindGameObjectWithTag("Cutter");
            //boss_door1 = GameObject.Find("door_2");


    Load_("Scena 1 - Il massiccio");
        }

        //if(gameObject.name.Equals("LoadingController")) Destroy(gameObject);
    }

    public void Load_(string scene_name)
    {
        
        if (File.Exists(Application.persistentDataPath + "/save.dat") && scene_name.Equals("Scena 1 - Il massiccio"))
        {
            isys = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
            hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
            player = GameObject.FindGameObjectWithTag("Player");
            misc = player.GetComponent<Misc>();
            //AxeSpawn =GameObject.FindGameObjectWithTag("AxeEnemySpawn");
            //PistolSpawn = GameObject.FindGameObjectWithTag("PistolEnemySpawn"); ;
            //ChainSpawn = GameObject.FindGameObjectWithTag("ChainEnemySpawn"); ;
            //CutterSpawn = GameObject.FindGameObjectWithTag("CutterEnemySpawn"); ;
            //SmgSpawn = GameObject.FindGameObjectWithTag("SmgEnemySpawn"); ;

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);

            SceneData data = (SceneData)formatter.Deserialize(file);
            file.Close();

            isys.LoadPlayer(data.pdata);

            Save.AxeSpawnActive = data.spawn.AxeSpawnActive;
            if (data.spawn.AxeSpawnActive) {
                AxeSpawn.SetActive(true);
            }
            else {
                AxeSpawn.SetActive(false);
            }

            Save.PistolSpawnActive = data.spawn.PistolSpawnActive;
            if (data.spawn.PistolSpawnActive) {
                PistolSpawn.SetActive(true);
                
            }
            else {
                PistolSpawn.SetActive(false);
            }

            Save.ChainSpawnActive = data.spawn.ChainSpawnActive;
            if (data.spawn.ChainSpawnActive) {
                ChainSpawn.SetActive(true);
            }
            else {
                ChainSpawn.SetActive(false);
            }

            Save.CutterSpawnActive = data.spawn.CutterSpawnActive;
            if (data.spawn.CutterSpawnActive) {
                CutterSpawn.SetActive(true);
            }
            else {
                CutterSpawn.SetActive(false);
            }

            Save.SmgSpawnActive = data.spawn.SmgSpawnActive;
            if (data.spawn.SmgSpawnActive) {
                SmgSpawn.SetActive(true);
            }
            else {
                SmgSpawn.SetActive(false);
            }

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

                Destroy(GameObject.Find("WeaponsCrate"));
                Destroy(GameObject.Find("crate"));
            }

            Destroy(GameObject.Find(data.checkpoint_name));

            foreach(GameObject checkpoint_scena in GameObject.FindGameObjectsWithTag("Checkpoint")) {
                foreach (string checkpoint_save in data.check_data) {
                    if (checkpoint_scena.name.Equals(checkpoint_save)) {
                        checkpoint_scena.GetComponent<Save>().loaded = true;
                    }
                }
            }

            foreach (GameObject checkpoint_scena in GameObject.FindGameObjectsWithTag("Checkpoint")) {
                if (checkpoint_scena.GetComponent<Save>().loaded == false) {
                    Destroy(checkpoint_scena);
                }
                else {
                    checkpoint_scena.GetComponent<Save>().AxeSpawn = AxeSpawn;
                    print(checkpoint_scena.GetComponent<Save>().AxeSpawn);
                    checkpoint_scena.GetComponent<Save>().PistolSpawn = PistolSpawn;
                    checkpoint_scena.GetComponent<Save>().ChainSpawn = ChainSpawn;
                    checkpoint_scena.GetComponent<Save>().CutterSpawn = CutterSpawn;
                    checkpoint_scena.GetComponent<Save>().SmgSpawn = SmgSpawn;
                }

            }


            }
    }

    
    //Caricamento asincrono della scena indicata per mostrare la barra di caricamento (Il tutto dal menù principale)
    public IEnumerator LoadAsync(string scene_name, Text loadingProgress, GameObject premiper, Slider slider)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_name); //Caricamento asincrono della scena (mi restituisce un oggetto con info utili)
        
        operation.allowSceneActivation = false;

        if (scene_name.Equals("Scena 1 - Il massiccio")) {
            descrizione = "Fin, un ex soldato congedato con onore dalla guerra in Iraq, vive a Pimentel con la moglie e si gode la pensione ma un giorno, " +
                            "mentre è in viaggio(verso una località a noi ignota), una luce abbagliante lo investe...";
        }
        else {
            descrizione = "descrizione livello 2";
        }

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            loadingProgress.text = progress * 100f + "%";

            premiper.SetActive(true);
            premiper.GetComponent <Text>().text = descrizione;

            if (operation.progress == .9f && loadingProgress.text.Equals("100%")) {
                loadingProgress.text = "Premi E per giocare";

                if (Input.GetButtonDown("Open Door")) {
                    premiper.SetActive(true);
                    premiper.GetComponent<Text>().text = "Attivazione scena...";
                    operation.allowSceneActivation = true;
                }
            }
                yield return null;
        }
        
    }

    public void OnClick(){
        //Se si trova nel menù principale, e preme il bottone di Load (il gameobject che contiene la script)
        if (gameObject.name.Equals("LoadingController"))
        {
            //Disattivo i bottoni
            foreach (GameObject bottone in GameObject.FindGameObjectsWithTag("BottoniMenu")) {
                bottone.SetActive(false);
            }
            loadingScreen.SetActive(true); //Abilito la barra di loading

            Destroy(GameObject.Find("Music"));

            if (EventSystem.current.currentSelectedGameObject.name.Equals("Load"))
            {
                new_game = false;
                
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);


                SceneData data = (SceneData)formatter.Deserialize(file);
                scene_name = data.scena_name;
                file.Close();
                StartCoroutine(LoadAsync(scene_name, loadingProgress, premiper, slider));
                
            }
            else
            {
                new_game = true;
                print("new game");
                scene_name = "Scena 1 - Il massiccio";

                StartCoroutine(LoadAsync(scene_name, loadingProgress, premiper, slider));
            }
            
            
            
        }
    }
}
