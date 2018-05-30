using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour {

    /// <summary>
    /// author: feduss
    /// </summary>
    private GameObject shutter1;
    private GameObject boss_door1;
    public static GameObject boss1;
    private HUDSystem hud;
    public static bool boss_defeated;
    public static bool boss_is_active;

	// Use this for initialization
	void Start () {
        shutter1 = GameObject.FindGameObjectWithTag("Serranda");
        boss_door1 = GameObject.Find("door_2");

        if (gameObject.name.Equals("boss_door"))
        {
            boss1 = GameObject.FindGameObjectWithTag("MiniBoss");
            boss1.SetActive(false);
        }
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
        boss_defeated = false;
        boss_is_active = false;
        

        


    }
	
	// Update is called once per frame
	void Update () {
        if (boss1!=null && boss_is_active && boss1.GetComponent<Boss1Controller>().health == 0)
        {
            boss_defeated = true;
            hud.bossBarEnabler(false);
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        //Chiusura serranda
        if (other.CompareTag("Player") && gameObject.name.Equals("shutter1"))
        {
            shutter1.GetComponent<Animator>().SetTrigger("Close");
        }

        //Se il boss tocca la zona dove atterra, succede quanto segue:
        if (other.gameObject.CompareTag("MiniBoss") && gameObject.name.Equals("Miniboss_spawn"))
        {
            boss_is_active = true;
            Destroy(gameObject);
        }

        //Chiudo la porta quando mi ci avvicino ad essa (è la porta di uscita dalla mini boss fight)
        if (other.CompareTag("Player") && gameObject.name.Equals("boss_door") && !boss_is_active)
        {
            boss_door1.GetComponent<Animator>().SetTrigger("Close");
            boss1.SetActive(true);
            hud.bossBarEnabler(true);
            hud.bossNameSetter("Gente Mala");
        }
        else
        {
            //Se ho sconfitto il miniboss, apro la porta di uscita dalla zona
            if (boss_defeated)
            {
                boss_door1.GetComponent<Animator>().SetTrigger("Boss dies");
            }
        }

    }
}
