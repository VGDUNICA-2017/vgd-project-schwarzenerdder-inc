using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

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

    public LayerMask lm;

    // Use this for initialization
    void Start () {

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

		inventario = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
		hudsystem = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();

        tpsCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        playsound = GameObject.FindGameObjectWithTag("Player").GetComponent<PlaySound>();

    }

	public void Update() {


        if (player.GetBool("Axe"))
        {
            hudsystem.hudShotsEnabler(false);
            gunDamage = 20;
        }
        else
        {
            //se il giocatore ha la pistola
            if (player.GetBool("Pistol"))
            {

                //imposto l'index dell'arma (serve per l'inventario)
                index = 0;
                //attivo l'hud della pistola
                hudsystem.hudShotsEnabler(true);

                gunDamage = 20;

                //Distruggo il proiettile già presente nell'arma
                Destroy(GameObject.Find("P69mm"), 1f);

            }

            //se il giocatore ha la pistola
            if (player.GetBool("Smg"))
            {

                //imposto l'index dell'arma (serve per l'inventario)
                index = 2;
                //attivo l'hud della pistola
                hudsystem.hudShotsEnabler(true);

                gunDamage = 8;

            }


        }
        

        //Setto le munizioni nel caricatore e di riserva con quanto vi è nell'inventario
        leftMagAmmo = inventario.ammoLeft(index);
		leftInvAmmo = inventario.ammoInvLeft(index);

        //Se il giocatore sta impugnando un'arma da fuoco
        if (player.GetBool("Pistol"))
        {
            PistolFire();
            Reload();
            weaponRange = 100f;
        }

        if (player.GetBool("Smg"))
        {
            SmgFire();
            Reload();
            weaponRange = 100f;
        }

        //Se il giocatore sta impugnando l'ascia
        if (player.GetBool("Axe"))
        {
            Axehit();
            weaponRange = 5f;
        }
	}

    public void Reload()
    {
        if (Input.GetButton("Reload") && (!(player.GetCurrentAnimatorStateInfo(0).IsName("Reload")) || !(player.GetCurrentAnimatorStateInfo(0).IsName("Reload Smg"))) &&
            (!player.GetCurrentAnimatorStateInfo(1).IsName("Fire") || !(player.GetCurrentAnimatorStateInfo(1).IsName("Fire Smg"))) &&
            leftInvAmmo > 0)
        {

            player.SetBool("Reload", true);

            //Cambio la ricarica in base ai colpi nel caricatore
            if (leftMagAmmo == 0)
            {
                playsound.PlayEmptyMagReload();
            }
            else
            {
                playsound.PlayReloadSound();
            }

        }
        else
        {
            player.SetBool("Reload", false);
        }

        if (leftMagAmmo == 0)
        {
            player.SetFloat("OutofAmmo", 1f);
        }
        else
        {
            if (!(player.GetCurrentAnimatorStateInfo(0).IsName("Reload")) || !(player.GetCurrentAnimatorStateInfo(0).IsName("Reload Smg"))) player.SetFloat("OutofAmmo", 0f);
        }
    }

    public void PistolFire()
    {

        //Spara se preme il tasto sinistro del mouse, se non sta già sparando, se non sta ricaricando e se non sta correndo
        if (Input.GetButtonDown("Fire1") && !player.GetCurrentAnimatorStateInfo(0).IsName("Reload") &&
            !player.GetCurrentAnimatorStateInfo(1).IsName("Fire") && (!player.IsInTransition(1) && !player.GetBool("Run")))
        {

            //Se il caricatore è vuoto
            if (leftMagAmmo == 0)
            {
                playsound.PlayEmptyMag();
            }
            else
            {
                if (!gameObject.CompareTag("Axe"))  player.SetBool("Fire", true);

                RaycastShot();

                fire_effect.GetComponent<ParticleSystem>().Play();
                playsound.PlayShootSound();
            }
            inventario.shot(index);

        }
        else
        {
            if (!gameObject.CompareTag("Axe"))
            {
                player.SetBool("Fire", false);
                fire_effect.GetComponent<ParticleSystem>().Stop();

            }
        }

        
    }

    public void SmgFire()
    {

        //Spara se preme il tasto sinistro del mouse, se non sta già sparando, se non sta ricaricando e se non sta correndo
        if (Input.GetButton("Fire1") && !player.GetCurrentAnimatorStateInfo(0).IsName("Reload Smg") &&
            !player.GetCurrentAnimatorStateInfo(1).IsName("AutoFire_Smg") && !player.IsInTransition(1) && !player.GetBool("Run"))
        {

            //Se il caricatore è vuoto
            if (leftMagAmmo == 0)
            {
                playsound.PlayEmptyMag();
            }
            else
            {
                if (!gameObject.CompareTag("Axe")) player.SetBool("AutomaticFire", true);

                RaycastShot();

                fire_effect.GetComponent<ParticleSystem>().Play();
                playsound.PlayShootSound();
            }
            inventario.shot(index);

        }
        else
        {
            if (!gameObject.CompareTag("Axe"))
            {
                player.SetBool("AutomaticFire", false);
                fire_effect.GetComponent<ParticleSystem>().Stop();

            }
        }


    }

    public void Axehit()
    {


        if (Input.GetButtonDown("Fire1") && !(player.GetCurrentAnimatorStateInfo(1).IsName("MeleeAttack")) && !player.IsInTransition(1) && !player.GetBool("Run"))
        {
            player.SetTrigger("Attack");
            RaycastShot();
            playsound.PlayAxeAttackSound();
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

        if (Physics.Raycast(rayOrigin, direction, out hit, weaponRange, lm))
        {
            print(hit.collider.name);

            if (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Enemy_part") || hit.collider.gameObject.CompareTag("Boss"))
            {

                //Istanzio il sangue sul nemico
                Instantiate(bullet_impact, hit.point, Quaternion.Euler(hit.normal));
                //Gli infliggo danno
                if (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Boss")) hit.collider.gameObject.GetComponent<EnemyController>().takeDamage(gunDamage);
                else hit.collider.gameObject.GetComponentInParent<EnemyController>().takeDamage(gunDamage);
            }
            else
            { //danno bonus se lo colpisce all testa
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

    public void SetActive()
    {
        GetComponentInParent<SwitchWeapon>().SetActive();
    }



}