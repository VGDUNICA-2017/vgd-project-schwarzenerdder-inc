using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour {

    private InventorySystem isys;
    private GameObject player;
    private HUDSystem hud;
    private Misc misc;
    private Scene m_Scene;

    // Use this for initialization
    void Start () {
        m_Scene = SceneManager.GetActiveScene();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Load_()
    {
        if (Input.GetKeyDown("l") && File.Exists(Application.persistentDataPath + "/save.dat") && m_Scene.name.Equals("Scena 1 - Il massiccio"))
        {
            print("Loading pt2");
            isys = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
            hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
            player = GameObject.FindGameObjectWithTag("Player");
            misc = player.GetComponent<Misc>();

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);

            SceneData data = (SceneData)formatter.Deserialize(file);
            file.Close();

            isys.LoadPlayer(data.pdata);
            LoadEnemy(data.edata);


        }
    }

    public void LoadEnemy(List<Enemy> edata)
    {
        foreach (Enemy nemico in edata)
        {
            GameObject tempEnemy = GameObject.Find(nemico.name);
            tempEnemy.transform.localPosition = new Vector3(nemico.x, nemico.y, nemico.z);
            if (tempEnemy.GetComponent<EnemyController>() != null)
            {
                tempEnemy.GetComponent<EnemyController>().health = nemico.health;
                tempEnemy.GetComponent<EnemyController>().setFromLoad(new Vector3(nemico.startX, nemico.startY, nemico.startZ));
            }
            if (tempEnemy.GetComponent<Boss1Controller>() != null) tempEnemy.GetComponent<Boss1Controller>().health = nemico.health;
            if (tempEnemy.GetComponent<BossHealth>() != null) tempEnemy.GetComponent<BossHealth>().currentHealth = nemico.health;



        }


    }

    public void OnClick(){
        print(gameObject.name);
        //Se si trova nel menù principale, e preme il bottone di Load (il gameobject che contiene la script)
        if (gameObject.name.Equals("Load"))
        {
            Destroy(GameObject.Find("Music"));
            print("Loading pt1");
            SceneManager.LoadScene("Scena 1 - Il massiccio", LoadSceneMode.Single);
            Load_();
        }
    }
}
