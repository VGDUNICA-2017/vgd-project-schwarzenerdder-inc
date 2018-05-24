using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class Save : MonoBehaviour {

    private InventorySystem isys;
    private GameObject player;
    private HUDSystem hud;
    private Misc misc;

    private Scene m_Scene;
    
    private GameObject torcia;
    private GameObject ascia;
    private GameObject pistola;
    private GameObject smg;
    private GameObject final_key;
    private GameObject cutter;

    // Use this for initialization
    void Start () {
        isys = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
        misc = player.GetComponent<Misc>();

        m_Scene = SceneManager.GetActiveScene();

        if (m_Scene.name.Equals("Scena 1 - Il massiccio"))
        {
            torcia = GameObject.Find("la Torcia");
            ascia = GameObject.Find("l'ascia");
            pistola = GameObject.Find("P226");
            smg = GameObject.Find("MP5");
            final_key = GameObject.FindGameObjectWithTag("FinalKey");
            cutter = GameObject.FindGameObjectWithTag("Cutter");
        }


    }
	
	// Update is called once per frame
	void Update () {
	}

    public void Save_()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

        SceneData data = new SceneData();
        data.pdata = isys.SavePlayer();
        data.edata = new List<Enemy>();
        data.idata = new List<string>();
        data.checkpoint_name = gameObject.name;
        data.scena_name = SceneManager.GetActiveScene().name;
        
        
        
        //Per ogni nemico con tag Enemy
        foreach (GameObject nemico in GameObject.FindGameObjectsWithTag("Enemy")){
            if (nemico.activeInHierarchy)
            {
                data.edata.Add(nemico.GetComponent<EnemyController>().saveEnemy());

            }
        }

        foreach(GameObject kitmedico in GameObject.FindGameObjectsWithTag("FirstAid"))
        {
            data.idata.Add(kitmedico.gameObject.name);
        }

        foreach (GameObject ammo_9mm in GameObject.FindGameObjectsWithTag("Ammo_9mm"))
        {
            data.idata.Add(ammo_9mm.gameObject.name);
        }

        foreach (GameObject ammo_smg in GameObject.FindGameObjectsWithTag("Ammo_smg"))
        {
            data.idata.Add(ammo_smg.gameObject.name);
        }

        if(torcia!=null) data.idata.Add(torcia.gameObject.name);

        if (ascia != null)  data.idata.Add(ascia.gameObject.name);

        if (pistola != null) data.idata.Add(pistola.gameObject.name);

        if (smg != null) data.idata.Add(smg.gameObject.name);

        if (final_key != null) data.idata.Add(final_key.gameObject.name);

        if (cutter != null) data.idata.Add(cutter.gameObject.name);

        formatter.Serialize(file, data);

        file.Close();

        misc.supportFunction(gameObject);
    }

    

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Salvataggio");
            hud.sideBoxEnabler(true);
            hud.sideBoxText("Checkpoint raggiunto!");
            Save_();
            
            
        }
    }
}

[System.Serializable]
class SceneData
{
    public PlayerData pdata;
    public List<Enemy> edata;
    public List<string> idata;
    public string checkpoint_name;
    public string scena_name;
}

[System.Serializable]
public class Enemy
{

    public float x;
    public float y;
    public float z;
    public string name;
    public int health;

    public float startX;
    public float startY;
    public float startZ;

}


[System.Serializable]
public class Events
{
    public bool active;
}