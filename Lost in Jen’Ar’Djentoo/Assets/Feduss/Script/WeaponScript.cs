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
    private BulletScript bulletscript;

    //munizioni nel caricatore, di riserva e tipo di arma
    private int leftMagAmmo;
	private int leftInvAmmo;
	private int index;

    //Proiettile
    public GameObject bullet;

    //start_bullet=posizione iniziale di istanza del proiettile
    private Transform start_bullet;

    



	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
		m_AudioSource = GetComponent<AudioSource>();

		inventario = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
		hudsystem = GameObject.FindGameObjectWithTag("Player").GetComponent<HUDSystem>();

        bulletscript = bullet.GetComponent<BulletScript>(); 

    }

	public void Update() {
		//se il giocatore ha la pistola
		if (player.GetBool("Pistol")) {

			//imposto l'index dell'arma (serve per l'inventario)
			index = 0;
            //attivo l'hud della pistola (WIP, perchè serve solo quando raccogli la prima arma, che è la pistola)
			hudsystem.hudShots();

            //Salvo quella che sarà la parentela del proiettile con l'arma
            start_bullet = GameObject.Find("P226 (Impugnata)").transform;

            //Distruggo il proiettile già presente nell'arma
            Destroy(GameObject.Find("P69mm"), 1f);
        }

        //Setto le munizioni nel caricatore e di riserva con quanto vi è nell'inventario
		leftMagAmmo = inventario.ammoLeft(index);
		leftInvAmmo = inventario.ammoInvLeft(index);

		//Spara se preme il tasto sinistro del mouse, se non sta già sparando e se non sta ricaricando
		if (Input.GetButtonDown("Fire1") && !player.GetCurrentAnimatorStateInfo(1).IsName("Reload") && 
			!(animator.GetCurrentAnimatorStateInfo(0).IsName("Fire"))) {

			//Se il caricatore è vuoto
			if (leftMagAmmo == 0) {
				PlayEmptyMag();
			} else {
				animator.SetBool("Fire", true);

                //Istanzio il proiettile
                Instantiate(bullet, start_bullet);

                PlayShootSound();
				player.SetBool("isFiring", true);
                
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