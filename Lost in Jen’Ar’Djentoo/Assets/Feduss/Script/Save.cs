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

	// Use this for initialization
	void Start () {
        isys = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
        misc = player.GetComponent<Misc>();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void Save_()
    {

        print("Salvataggio start");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

        SceneData data = new SceneData();
        data.pdata = isys.SavePlayer();
        data.edata = new List<Enemy>();
        
        
        //Per ogni nemico con tag Enemy
        foreach (GameObject nemico in GameObject.FindGameObjectsWithTag("Enemy")){
            if (nemico.activeInHierarchy)
            {
                data.edata.Add(SaveEnemy(nemico));

            }
        }

        //Salvo il miniboss
        GameObject miniBoss = GameObject.FindGameObjectWithTag("MiniBoss");
        if(miniBoss!=null && miniBoss.activeInHierarchy) data.edata.Add(SaveEnemy(miniBoss));


        //Salvo i due boss (fine lv1 e fine lv2)
        foreach (GameObject boss in GameObject.FindGameObjectsWithTag("Boss"))
        {

            //Se sono attivi nella scena (cioè se sono spawnati e non sono morti)
            if (boss.activeInHierarchy)
            {
                data.edata.Add(SaveEnemy(boss)); //Li salvo in una lista
            }
        }

        /*foreach(GameObject kitmedico in GameObject.FindGameObjectsWithTag("FirstAid"))
        {
            data.kit.Add(kitmedico);
        }

        foreach (GameObject ammo_9mm in GameObject.FindGameObjectsWithTag("Ammo_9mm"))
        {
            data.kit.Add(ammo_9mm);
        }

        foreach (GameObject ammo_smg in GameObject.FindGameObjectsWithTag("Ammo_smg"))
        {
            data.kit.Add(ammo_smg);
        }

        GameObject torcia = GameObject.Find("la Torcia");
        data.weapons.Add(torcia);

        GameObject ascia = GameObject.Find("l'ascia");
        data.weapons.Add(ascia);

        GameObject pistola = GameObject.Find("P226");
        data.weapons.Add(pistola);

        GameObject smg = GameObject.Find("MP5");
        data.weapons.Add(smg);

        GameObject final_key = GameObject.FindGameObjectWithTag("FinalKey");
        data.key_objects.Add(final_key);

        GameObject cutter = GameObject.FindGameObjectWithTag("Cutter");
        data.key_objects.Add(cutter);

        GameObject chain = GameObject.Find("chain");
        data.events.Add(chain);

        GameObject recinzione = GameObject.FindGameObjectWithTag("Recinzione");
        data.events.Add(recinzione);

        GameObject serranda = GameObject.FindGameObjectWithTag("Serranda");
        data.events.Add(serranda);

        foreach (GameObject porta in GameObject.FindGameObjectsWithTag("Porta"))
        {
            data.doors.Add(porta);
        }

        GameObject porta_preboss_lv1 = GameObject.FindGameObjectWithTag("FinalDoor");
        data.doors.Add(porta_preboss_lv1);*/

        formatter.Serialize(file, data);

        print("Salvataggio end");
        print(Application.persistentDataPath + "/gameData.dat");

        file.Close();
     
    }

    public Enemy SaveEnemy(GameObject nemico)
    {
        Enemy tempEnemy = new Enemy();
        tempEnemy.x = nemico.transform.position.x;
        tempEnemy.y = nemico.transform.position.y;
        tempEnemy.z = nemico.transform.position.z;
        Vector3 startPos = new Vector3();
        if (nemico.GetComponent<EnemyController>() != null)  startPos = nemico.GetComponent<EnemyController>().saveStartPos();
        tempEnemy.startX = startPos.x;
        tempEnemy.startY = startPos.y;
        tempEnemy.startZ = startPos.z;
        tempEnemy.name = nemico.gameObject.name;
        if(nemico.GetComponent<EnemyController>()!=null )tempEnemy.health = nemico.GetComponent<EnemyController>().health;
        if(nemico.GetComponent<Boss1Controller>()!=null )tempEnemy.health = nemico.GetComponent<Boss1Controller>().health;
        if(nemico.GetComponent<BossHealth>()!=null )     tempEnemy.health = nemico.GetComponent<BossHealth>().currentHealth;
        return tempEnemy;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Salvataggio");
            hud.sideBoxEnabler(true);
            hud.sideBoxText("Checkpoint raggiunto!");
            Save_();
            misc.supportFunction(gameObject);
            
        }
    }
}

[System.Serializable]
class SceneData
{
    public PlayerData pdata;
    public List<Enemy> edata;
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