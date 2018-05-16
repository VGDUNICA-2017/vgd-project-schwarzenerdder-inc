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
            weaponRange = 2.5f;
        }
	}

    public void Reload()
    {
        //Può ricaricare se preme R, se non sta già ricaricando o sparando e se ha abbastanza colpi di riserva
        if (Input.GetButton("Reload") && !(player.GetCurrentAnimatorStateInfo(0).IsName("Reload")) && !(player.GetCurrentAnimatorStateInfo(0).IsName("Reload_Smg")) &&
            !player.GetCurrentAnimatorStateInfo(1).IsName("Fire") && !(player.GetCurrentAnimatorStateInfo(1).IsName("Fire_Smg")) &&
            leftInvAmmo > 0)
        {
            //Avvio l'animazione
            player.SetBool("Reload", true);

            //Cambio il suono della ricarica in base ai colpi nel caricatore
            if (leftMagAmmo == 0)
            {
                playsound.PlayEmptyMagReload(m_EmptyMagReload);
            }
            else
            {
                playsound.PlayReloadSound(m_ReloadSound);
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

    public void PistolFire()
    {

        //Spara se preme il tasto sinistro del mouse, se non sta già sparando, se non sta ricaricando e se non sta correndo
        if (Input.GetButtonDown("Fire1") && !player.GetCurrentAnimatorStateInfo(0).IsName("Reload") &&
            !player.GetCurrentAnimatorStateInfo(1).IsName("Fire") && (!player.IsInTransition(1) && !player.GetBool("Run")))
        {

            //Se il caricatore è vuoto
            if (leftMagAmmo == 0)
            {
                playsound.PlayEmptyMag(m_EmptyMag);
            }
            else
            {
                player.SetTrigger("Fire"); //avvio l'animazione di sparo

                RaycastShot(); //Richiamo la funzione che gestisce il raycast

                fire_effect.GetComponent<ParticleSystem>().Play(); //WIP

                playsound.PlayShootSound(m_ShootSound);
                
            }
            inventario.shot(index); //Scalo un colpo


        }
        else
        {
            fire_effect.GetComponent<ParticleSystem>().Stop(); //WIP
        }

        
    }

    public void SmgFire()
    {   

        //Spara se preme il tasto sinistro del mouse, se non sta già sparando, se non sta ricaricando e se non sta correndo
        if (Input.GetButton("Fire1") &&  !player.GetCurrentAnimatorStateInfo(0).IsName("Reload_Smg") && !player.IsInTransition(1) && !player.GetBool("Run"))
        {

            //Se ha ancora colpi nel caricatore, allora spara normalmente
            if (leftMagAmmo > 0)
            {
                player.SetBool("AutomaticFire", true); //Avvio l'animazione
                fire_effect.GetComponent<ParticleSystem>().Play(); //WIP
                                                                   //playsound.PlayShootSound();
                playsound.PlayShootSound(m_ShootSound);//da vedere se è corretto inserirlo qui!

            }
            else
            {
                //Altrimenti, per una sola volta (one):
                if (one)
                {
                    print("we");
                    //Se il caricatore è vuoto
                    playsound.PlayEmptyMag(m_EmptyMag);
                    inventario.shot(index); //Serve per far pulsare di rosso lo 0 dei colpi nel caricatore
                    player.SetBool("AutomaticFire", false); //Esco dall'animazione
                    one = false; //La imposto su false per non entrare più in questo if sino al prossimo reset di one
                }
            }


           
            
        }
        else
        {
            //Quando rilascio il pulsante di sparo, esco dall'animazione e resetto one
            if (Input.GetButtonUp("Fire1") || leftMagAmmo==0)
            {
                player.SetBool("AutomaticFire", false);
                one = true;    
            }
        }

        
        
        


    }

    public void Axehit()
    {

        //Posso colpo con l'ascia se premo il tasto sinistro del mouse, se non sto già attaccando o correndo
        if (Input.GetButtonDown("Fire1") && !(player.GetCurrentAnimatorStateInfo(1).IsName("MeleeAttack")) && !player.IsInTransition(1) && !player.GetBool("Run"))
        {
            player.SetTrigger("Attack"); //Avvio l'animazione
            RaycastShot(); //Richiamo la funzione che gestisce il raycast
            playsound.PlayAxeAttackSound(m_ShootSound);
        }
    }

    public void RaycastShot()
    {

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
                if (hit.collider.gameObject.CompareTag("Testa"))
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