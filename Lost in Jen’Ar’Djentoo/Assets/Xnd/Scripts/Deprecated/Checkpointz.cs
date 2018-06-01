using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class Checkpointz : MonoBehaviour {

	private InventorySystem isys;
	private GameObject player;
	private HUDSystem hud;
	private Misc misc;

	private Scene m_Scene;
	public bool loaded = false;

	private GameObject torcia;
	private GameObject ascia;
	private GameObject pistola;
	private GameObject smg;
	private GameObject final_key;
	private GameObject cutter;

	public GameObject AxeSpawn;
	public GameObject PistolSpawn;
	public GameObject ChainSpawn;
	public GameObject CutterSpawn;
	public GameObject SmgSpawn;

	public static bool AxeSpawnActive = false;
	public static bool PistolSpawnActive = false;
	public static bool ChainSpawnActive = false;
	public static bool CutterSpawnActive = false;
	public static bool SmgSpawnActive = false;

	// Use this for initialization
	void Start() {
		isys = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
		hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
		player = GameObject.FindGameObjectWithTag("Player");
		misc = player.GetComponent<Misc>();

		m_Scene = SceneManager.GetActiveScene();
        


	}

	// Update is called once per frame
	void Update() {
	}

	public void Save_() {
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

		SceneData data = new SceneData();
		data.pdata = isys.SavePlayer();
		data.edata = new List<Enemy>();
		data.idata = new List<string>();
		data.check_data = new List<string>();
		data.spawn = new Spawn();
		data.checkpoint_name = gameObject.name;
		data.scena_name = SceneManager.GetActiveScene().name;
		data.spawn.AxeSpawnActive = AxeSpawnActive;
		data.spawn.ChainSpawnActive = ChainSpawnActive;
		data.spawn.CutterSpawnActive = CutterSpawnActive;
		data.spawn.PistolSpawnActive = PistolSpawnActive;
		data.spawn.SmgSpawnActive = SmgSpawnActive;

		foreach (GameObject checkpoint_scene in GameObject.FindGameObjectsWithTag("Checkpoint")) {
			data.check_data.Add(checkpoint_scene.name);
		}

		//Per ogni nemico con tag Enemy
		foreach (GameObject nemico in GameObject.FindGameObjectsWithTag("Enemy")) {

			if (nemico.activeInHierarchy) {
				data.edata.Add(nemico.GetComponent<EnemyController>().saveEnemy());

			}
		}

		foreach (GameObject kitmedico in GameObject.FindGameObjectsWithTag("FirstAid")) {
			data.idata.Add(kitmedico.gameObject.name);
		}

		foreach (GameObject ammo_9mm in GameObject.FindGameObjectsWithTag("Ammo_9mm")) {
			data.idata.Add(ammo_9mm.gameObject.name);
		}

		foreach (GameObject ammo_smg in GameObject.FindGameObjectsWithTag("Ammo_smg")) {
			data.idata.Add(ammo_smg.gameObject.name);
		}

		if (torcia != null) data.idata.Add(torcia.gameObject.name);

		if (ascia != null) data.idata.Add(ascia.gameObject.name);

		if (pistola != null) data.idata.Add(pistola.gameObject.name);

		if (smg != null) data.idata.Add(smg.gameObject.name);

		if (final_key != null) data.idata.Add(final_key.gameObject.name);

		if (cutter != null) data.idata.Add(cutter.gameObject.name);

		formatter.Serialize(file, data);

		file.Close();

		misc.supportFunction(gameObject);
	}

	public void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag("Player")) {
			print(AxeSpawn);
			if (!AxeSpawn.activeInHierarchy) {
				AxeSpawn.SetActive(true);
			}
			else {
				AxeSpawnActive = true;
			}

			if (!PistolSpawn.activeInHierarchy) {
				PistolSpawn.SetActive(true);
			}
			else {
				PistolSpawnActive = true;
			}

			if (!ChainSpawn.activeInHierarchy) {
				ChainSpawn.SetActive(true);
			}
			else {
				ChainSpawnActive = true;
			}

			if (!CutterSpawn.activeInHierarchy) {
				CutterSpawn.SetActive(true);
			}
			else {
				CutterSpawnActive = true;
			}

			if (!SmgSpawn.activeInHierarchy) {
				SmgSpawn.SetActive(true);
			}
			else {
				SmgSpawnActive = true;
			}
		}
	}

	public void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player")) {

			print("Salvataggio");
			hud.sideBoxEnabler(true);
			hud.sideBoxText("Checkpoint raggiunto!");
			Save_();

			if (!AxeSpawnActive) {
				AxeSpawn.SetActive(false);
			}

			if (!PistolSpawnActive) {
				PistolSpawn.SetActive(false);
			}

			if (!ChainSpawnActive) {
				ChainSpawn.SetActive(false);
			}

			if (!CutterSpawnActive) {
				CutterSpawn.SetActive(false);
			}

			if (!SmgSpawnActive) {
				SmgSpawn.SetActive(false);
			}


		}
	}
}

