using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

	private Animator animator;
	private Animator player;


    //Script varie
    private InventorySystem inventario;
	private HUDSystem hudsystem;

    //munizioni nel caricatore, di riserva e tipo di arma
    private int leftMagAmmo;
	private int leftInvAmmo;
	private int index;

    //Effetto dello sparo
    public GameObject fire_effect;

    //start_bullet=posizione iniziale di istanza del proiettile
    private Transform start_bullet;

    //RAYCAST
    private int gunDamage = 20; //danno dell'arma
    public float fireRate = .25f; //tempo tra uno sparo e l'altro..nel nostro caso, determinerà la durante dell'animazione di sparo (più è alto, più sparera velocemente)

    public float weaponRange = 50f; //gittata dell'arma
    public float hitForce = 100f; //forza impressa dal colpo durante la collisione

    public Transform gunEnd; //transform del gameobject vuoto, dal quale partirà il raycast

    private Camera tpsCam; //serve per sapere dove il player sta mirando

    public GameObject bullet_impact;
    public GameObject bullet_impact_generic;

    private PlaySound playsound;

    public bool attack_flag = false;

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

		inventario = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
		hudsystem = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();

        tpsCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        playsound = GameObject.FindGameObjectWithTag("Player").GetComponent<PlaySound>();

    }

	public void Update() {

		//se il giocatore ha la pistola
		if (player.GetBool("Pistol")) {

			//imposto l'index dell'arma (serve per l'inventario)
			index = 0;
            //attivo l'hud della pistola (WIP, perchè serve solo quando raccogli la prima arma, che è la pistola)
			hudsystem.hudShotsEnabler(true);

            //Salvo quella che sarà la parentela del proiettile con l'arma
            start_bullet = GameObject.Find("Start_Bullet").transform;

            gunDamage = 15;

            //Distruggo il proiettile già presente nell'arma
            Destroy(GameObject.Find("P69mm"), 1f);

        }
        else hudsystem.hudShotsEnabler(false);

        if (player.GetBool("Axe"))
        {
            hudsystem.hudShotsEnabler(false);
            gunDamage = 30;
        }
        

        //Setto le munizioni nel caricatore e di riserva con quanto vi è nell'inventario
        leftMagAmmo = inventario.ammoLeft(index);
		leftInvAmmo = inventario.ammoInvLeft(index);

        //Se il giocatore sta impugnando un'arma da fuoco
        if (player.GetBool("Pistol"))
        {
            FireGun();
            weaponRange = 100f;
        }

        //Se il giocatore sta impugnando l'ascia
        if (player.GetBool("Axe"))
        {
            Axehit();
            weaponRange = 10f;
        }
	}

    public void FireGun()
    {
        //Spara se preme il tasto sinistro del mouse, se non sta già sparando, se non sta ricaricando e se non sta correndo
        if (Input.GetButtonDown("Fire1") && !player.GetCurrentAnimatorStateInfo(1).IsName("Reload") &&
            (!animator.GetCurrentAnimatorStateInfo(0).IsName("Fire") && !animator.IsInTransition(0) && !player.GetBool("isRunning")))
        {

            //Se il caricatore è vuoto
            if (leftMagAmmo == 0)
            {
                playsound.PlayEmptyMag();
            }
            else
            {
                if (!gameObject.CompareTag("Axe"))  animator.SetBool("Fire", true);

                RaycastShot();

                Instantiate(fire_effect, start_bullet.transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 90f, 0f)));


                playsound.PlayShootSound();
            }
            inventario.shot(index);

        }
        else
        {
            if(!gameObject.CompareTag("Axe")) animator.SetBool("Fire", false);
        }

        if (Input.GetButton("Reload") && !(animator.GetCurrentAnimatorStateInfo(0).IsName("Reload")) &&
            leftInvAmmo > 0)
        {

            animator.SetBool("Reload", true);
            player.SetBool("isReloading", true);

            //Cambio la ricarica in base ai colpi nel caricatore
            if (leftMagAmmo == 0)
            {
                playsound.PlayEmptyMagReload();
            }
            else
            {
                playsound.PlayReloadSound();
            }
            inventario.reloadWeapon(index);
        }
        else
        {
            animator.SetBool("Reload", false);
            player.SetBool("isReloading", false);
        }

        if (leftMagAmmo == 0)
        {
            animator.SetBool("OutOfAmmo", true);
        }
        else
        {
            animator.SetBool("OutOfAmmo", false);
        }

        if (leftInvAmmo == 0)
        {
            animator.SetBool("OutOfInvAmmo", true);
        }
        else
        {
            animator.SetBool("OutOfInvAmmo", false);
        }
    }

    public void Axehit()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            player.SetTrigger("Attack");
            //RaycastShot();
        }

        if (attack_flag)
        {
            RaycastShot();
            playsound.PlayAxeAttackSound();
            attack_flag = false;
        }
    }

    private void RaycastShot()
    {
        Vector3 rayOrigin;
        Vector3 direction;

        rayOrigin = tpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0)); //centro della camera
        direction = tpsCam.transform.forward;
        
        RaycastHit hit; //variabile per la memorizazzione delle info sull'oggetto colpito

        //cast del ray: rayOrigin: 
        //-origine del ray (il centro del camera)
        //-direzione del ray (il forward della camera
        //-info sull'oggetto colpito
        //-gittata raggio

        if (Physics.Raycast(rayOrigin, direction, out hit, weaponRange))
        {
            if (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Enemy_part"))
            {
                //Istanzio il sangue sul nemico
                Instantiate(bullet_impact, hit.point, Quaternion.Euler(hit.normal));
                Debug.Log("nemico");
                //Gli infliggo danno

                hit.collider.gameObject.GetComponent<EnemyController>().takeDamage(gunDamage);
            }
            else //danno bonus se lo colpisce all testa
                if (hit.collider.gameObject.CompareTag("Testa"))
                {
                    //Istanzio il sangue sul nemico
                    Instantiate(bullet_impact, hit.point, Quaternion.Euler(hit.normal));
                    hit.collider.gameObject.GetComponentInParent<EnemyController>().takeDamage(gunDamage * 2);
                }
                else
                {
                    if (!gameObject.CompareTag("Axe"))
                    {
                        Instantiate(bullet_impact_generic, hit.point, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
                    }
                }



        }

   
        

    }

}