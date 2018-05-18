using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveAndLoad : MonoBehaviour {

    private InventorySystem isys;
    private GameObject player;

	// Use this for initialization
	void Start () {
        isys = player.GetComponent<InventorySystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameDir.dat");

        SceneData data = new SceneData();
        data.pdata = isys.SavePlayer();
        
        //Per ogni nemico con tag Enemy
        foreach (GameObject nemico in GameObject.FindGameObjectsWithTag("Enemy")){

            data.enemyList.Add(nemico); //Li salvo in una lista
        }

        //Salvo il miniboss
        GameObject miniBoss = GameObject.FindGameObjectWithTag("MiniBoss");
        data.miniBoss = miniBoss;

        //Salvo i due boss (fine lv1 e fine lv2)
        foreach (GameObject boss in GameObject.FindGameObjectsWithTag("Boss"))
        {

            //Se sono attivi nella scena (cioè se sono spawnati e non sono morti)
            if (boss.activeInHierarchy)
            {
                data.enemyList.Add(boss); //Li salvo in una lista
            }
        }

        foreach(GameObject kitmedico in GameObject.FindGameObjectsWithTag("FirstAid"))
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

        GameObject Smg = GameObject.Find("MP5");
        data.weapons.Add(pistola);

        /*GameObject shotgun = GameObject.Find("???");
        data.weapons.Add(shotgun);*/

        GameObject final_key = GameObject.FindGameObjectWithTag("Final_Key");
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
        data.doors.Add(porta_preboss_lv1);

        formatter.Serialize(file, data);
        file.Close();
     
    }

    public void Load()
    {
        //isys.LoadPlayer();
    }
}

[System.Serializable]
class SceneData
{
    public PlayerData pdata;

    public ArrayList enemyList;
    public GameObject miniBoss;
    public ArrayList boss_lv1;

    public ArrayList kit;
    public ArrayList ammo_9mm;
    public ArrayList ammo_smg;
    public ArrayList weapons;

    public ArrayList key_objects;
    public ArrayList events;
    public ArrayList doors;
}