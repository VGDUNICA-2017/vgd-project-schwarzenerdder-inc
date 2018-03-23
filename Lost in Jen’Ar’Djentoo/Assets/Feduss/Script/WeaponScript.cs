using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

	private Animator animator;
	private Animator player;

    //audio
	[SerializeField] private AudioClip m_ReloadSound;
	[SerializeField] private AudioClip m_ShootSound;
	[SerializeField] private AudioClip m_EmptyMag;
	[SerializeField] private AudioClip m_EmptyMagReload;
	[SerializeField] private AudioClip m_EquipPistol;
    private AudioSource m_AudioSource;

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

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
		m_AudioSource = GetComponent<AudioSource>();

		inventario = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
		hudsystem = GameObject.FindGameObjectWithTag("Player").GetComponent<HUDSystem>();

        tpsCam = GameObject.Find("Camera").GetComponent<Camera>();

    }

	public void Update() {
		//se il giocatore ha la pistola
		if (player.GetBool("Pistol")) {

			//imposto l'index dell'arma (serve per l'inventario)
			index = 0;
            //attivo l'hud della pistola (WIP, perchè serve solo quando raccogli la prima arma, che è la pistola)
			hudsystem.hudShots(true);

            //Salvo quella che sarà la parentela del proiettile con l'arma
            start_bullet = GameObject.Find("Start_Bullet").transform;

            //Distruggo il proiettile già presente nell'arma
            Destroy(GameObject.Find("P69mm"), 1f);
        }
        else hudsystem.hudShots(false);

        //Setto le munizioni nel caricatore e di riserva con quanto vi è nell'inventario
        leftMagAmmo = inventario.ammoLeft(index);
		leftInvAmmo = inventario.ammoInvLeft(index);

		//Spara se preme il tasto sinistro del mouse, se non sta già sparando, se non sta ricaricando e se non sta correndo
		if (Input.GetButtonDown("Fire1") && !player.GetCurrentAnimatorStateInfo(1).IsName("Reload") && 
			(!animator.GetCurrentAnimatorStateInfo(0).IsName("Fire") && !player.GetCurrentAnimatorStateInfo(1).IsName("Idle") && !animator.IsInTransition(0) && !player.GetBool("isRunning") && !player.GetBool("isCrouching"))) {

			//Se il caricatore è vuoto
			if (leftMagAmmo == 0) {
				PlayEmptyMag();
			}
            else {
				animator.SetBool("Fire", true);

                RaycastShot();

                Instantiate(fire_effect, start_bullet.transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3 (0f,90f,0f)));


                PlayShootSound();
            }
            inventario.shot(index);

        } else {
			animator.SetBool("Fire", false);
        }

		if (Input.GetButton("Reload") && !(animator.GetCurrentAnimatorStateInfo(0).IsName("Reload")) && 
			leftInvAmmo > 0) {

			animator.SetBool("Reload", true);
			player.SetBool("isReloading", true);

			//Cambio la ricarica in base ai colpi nel caricatore
			if (leftMagAmmo == 0) {
				PlayEmptyMagReload ();
			} else {
				PlayReloadSound ();
			}
			inventario.reloadWeapon(index);
		} else {
			animator.SetBool("Reload", false);
			player.SetBool("isReloading", false);
		}

		if (leftMagAmmo == 0) {
			animator.SetBool ("OutOfAmmo", true);
		} else {
			animator.SetBool ("OutOfAmmo", false);
		}

		if (leftInvAmmo == 0) {
			animator.SetBool ("OutOfInvAmmo", true);
		} else {
			animator.SetBool ("OutOfInvAmmo", false);
		}
	}

    private void RaycastShot()
    {

        Vector3 rayOrigin = tpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0)); //centro della camera
        RaycastHit hit; //variabile per la memorizazzione delle info sull'oggetto colpito

        //cast del ray: rayOrigin: 
        //-origine del ray (il centro del camera)
        //-direzione del ray (il forward della camera
        //-info sull'oggetto colpito
        //-gittata raggio
        if (Physics.Raycast(rayOrigin, tpsCam.transform.forward, out hit, weaponRange))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
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
                    Instantiate(bullet_impact_generic, hit.point, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
                    Debug.Log("terreno");
                }



        }
        

    }


	private void PlayReloadSound() {
		m_AudioSource.clip = m_ReloadSound;
		m_AudioSource.Play ();
	}

	private void PlayShootSound() {
		m_AudioSource.clip = m_ShootSound;
		m_AudioSource.Play ();
	}

	private void PlayEmptyMag() {
		m_AudioSource.clip = m_EmptyMag;
		m_AudioSource.Play ();
	}

	private void PlayEmptyMagReload() {
		m_AudioSource.clip = m_EmptyMagReload;
		m_AudioSource.Play ();
	}

	public void PlayEquipPistolSound() {
		m_AudioSource.clip = m_EquipPistol;
		m_AudioSource.Play ();
	}
}