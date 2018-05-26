using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {


    //Ho seguito il seguente tutorial ---> https://www.youtube.com/watch?v=THnivyG0Mvo per impostare il rateo di fuoco e l'effetto particellare dello sparo
    //Ho seguito il tutorial di unity per impostare il raycast (per lo sparo)

    private Animator player;


    //Script varie
    private InventorySystem inventario;
	private HUDSystem hudsystem;

    //munizioni nel caricatore, di riserva e tipo di arma
    private int leftMagAmmo;
	private int leftInvAmmo;
	private int index;

    //Effetto dello sparo
    public ParticleSystem fire_effect;

    //RAYCAST
    private int gunDamage = 20; //danno dell'arma
    public float fireRate = 4f; //tempo tra uno sparo e l'altro
    public float nextFire = 0f;

    public float weaponRange = 50f; //gittata dell'arma
    public float hitForce = 100f; //forza impressa dal colpo durante la collisione

    public Transform gunEnd; //transform del gameobject vuoto, dal quale partirà il raycast

    private Camera tpsCam; //serve per sapere dove il player sta mirando

    public GameObject bullet_impact;
    public GameObject bullet_impact_generic;

    private PlaySound playsound;

    public LayerMask lm;

    public bool one = true;

    //audio
    [SerializeField] private AudioClip m_ReloadSound;
    [SerializeField] private AudioClip m_ShootSound;
    [SerializeField] private AudioClip m_EmptyMag;
    [SerializeField] private AudioClip m_EmptyMagReload;
    [SerializeField] private AudioClip m_EquipPistol;

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

                gunDamage = 25;

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
        if (player.GetBool("Pistol") || player.GetBool("Smg"))
        {
            Shot();
            Reload();
            weaponRange = 50f;
        }

        //Se il giocatore sta impugnando l'ascia
        if (player.GetBool("Axe"))
        {
            Axehit();
            weaponRange = 4f;
        }
	}

    public void Reload()
    {
        //Può ricaricare se preme R, se non sta già ricaricando o sparando e se ha abbastanza colpi di riserva
        if (Input.GetButton("Reload") && !(player.GetCurrentAnimatorStateInfo(0).IsName("Reload")) && !(player.GetCurrentAnimatorStateInfo(0).IsName("Reload_Smg")) &&
            !player.GetCurrentAnimatorStateInfo(1).IsName("Fire") && !(player.GetCurrentAnimatorStateInfo(1).IsName("Fire_Smg")) &&
            leftInvAmmo > 0 && leftMagAmmo!=inventario.maxAmmo(index))
        {
            //Avvio l'animazione
            player.SetBool("Reload", true);

            //Cambio il suono della ricarica in base ai colpi nel caricatore
            if (leftMagAmmo == 0)
            {
                playsound.PlayEmptyMagReload(m_EmptyMagReload, GetComponent<AudioSource>());
            }
            else
            {
                playsound.PlayReloadSound(m_ReloadSound, GetComponent<AudioSource>());
            }

        }
        else
        {
            //Esco dall'animazione
            player.SetBool("Reload", false);
        }

        if (leftMagAmmo == 0)
        {
            //Setto lo switch dell'animazione di ricarica
            player.SetFloat("OutofAmmo", 1f);
        }
        else
        {
            //Resetto lo switch dopo la ricarica da outOfAmmo
            if (!(player.GetCurrentAnimatorStateInfo(0).IsName("Reload")) && !(player.GetCurrentAnimatorStateInfo(0).IsName("Reload Smg"))) player.SetFloat("OutofAmmo", 0f);
        }
    }

    public void Shot()
    {

        //Spara se preme il tasto sinistro del mouse e se è passato il tempo minimo tra uno sparo e l'altro
        if (Input.GetButton("Fire1") && Time.time >= nextFire && !player.GetCurrentAnimatorStateInfo(1).IsName("Fire") &&
            !(player.GetCurrentAnimatorStateInfo(0).IsName("Reload")) && !(player.GetCurrentAnimatorStateInfo(0).IsName("Reload_Smg"))) //Serve per riprodurre correttamente (per ogni sparo) l'animazione della pistola (per quella del mitra non serve)
        {
            nextFire = Time.time + 1f/fireRate; //Spara ogni 1/fireRate secondi

            //Se il caricatore è vuoto
            if (leftMagAmmo == 0)
            {
                playsound.PlayEmptyMag(m_EmptyMag, GetComponent<AudioSource>());
            }
            else
            {
                player.SetBool("Fire", true); //avvio l'animazione di sparo

                RaycastShot(); //Richiamo la funzione che gestisce il raycast

                playsound.PlayShootSound(m_ShootSound, GetComponent<AudioSource>());
                

            }
            inventario.shot(index); //Scalo un colpo
            


        }
        else if(Input.GetButtonUp("Fire1"))
        {
            player.SetBool("Fire", false);
        }


        
    }

    public void Axehit()
    {

        //Posso colpo con l'ascia se premo il tasto sinistro del mouse, se non sto già attaccando o correndo
        if (Input.GetButtonDown("Fire1") && !(player.GetCurrentAnimatorStateInfo(1).IsName("MeleeAttack")) && !player.IsInTransition(1) && !player.GetBool("Run"))
        {
            player.SetTrigger("Attack"); //Avvio l'animazione
            RaycastShot(); //Richiamo la funzione che gestisce il raycast
            playsound.PlayAxeAttackSound(m_ShootSound, GetComponent<AudioSource>());
        }
    }

    public void RaycastShot()
    {
        

        if (!player.GetBool("Axe")) fire_effect.Play();

        //Funzione in parte scritta seguendo il tutorial di unity sui raycast

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
            //Se colpisco il nemico (Enemy_part=mani del nemico)) o il boss
            if (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Enemy_part") || hit.collider.gameObject.CompareTag("Boss") || hit.collider.gameObject.CompareTag("MiniBoss"))
            {
                print(hit.collider.name);
                //Istanzio il sangue sul nemico
                Instantiate(bullet_impact, hit.point, Quaternion.Euler(hit.normal));
                //Gli infliggo danno (l'else gestisce il danno se colpisco il nemico nelle mani, cioè enemy_part)
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<EnemyController>().takeDamage(gunDamage);
                }
                else if (hit.collider.gameObject.CompareTag("MiniBoss")) {
                        hit.collider.gameObject.GetComponent<Boss1Controller>().takeDamage(gunDamage);
                }
                     else if (hit.collider.gameObject.CompareTag("Boss"))
                     {
                            hit.collider.gameObject.GetComponent<BossHealth>().TakeDamage(gunDamage);
                     }
                            else
                            {
                                if (hit.collider.gameObject.GetComponentInParent<EnemyController>()!=null) {
                                    hit.collider.gameObject.GetComponentInParent<EnemyController>().takeDamage(gunDamage);
                                }
                                else
                                {
                                    hit.collider.gameObject.GetComponentInParent<Boss1Controller>().takeDamage(gunDamage);
                                }
                            }
                     
            }
            else
            { //danno bonus se lo colpisce all testa (WIP, manca un collider che gestisca la testa)
                if (hit.collider.gameObject.CompareTag("Testa") && !gameObject.CompareTag("Axe"))
                {
                    print("HEADSHOTTTT!");
                    //Istanzio il sangue sul nemico
                    Instantiate(bullet_impact, hit.point, Quaternion.Euler(hit.normal));
                    hit.collider.gameObject.GetComponentInParent<EnemyController>().takeDamage(gunDamage * 2);
                }
                else
                {
                    //Se colpisco qualcosa diverso da un nemico, istanzio un diverso effetto particellare (ma non se colpisco con l'ascia)
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