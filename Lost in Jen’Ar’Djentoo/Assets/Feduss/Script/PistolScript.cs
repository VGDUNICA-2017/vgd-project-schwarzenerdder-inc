using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolScript : MonoBehaviour {

    private Animator animator;

    [SerializeField] private AudioClip m_ReloadSound;
    [SerializeField] private AudioClip m_ShootSound;
    [SerializeField] private AudioClip m_EmptyMag;
    [SerializeField] private AudioClip m_EmptyMagReload;
    [SerializeField] private AudioClip m_EquipPistol;
    private PlayerMagazineHUD playerMagazineHUD;

    private int leftMagAmmo;
    private int leftInvAmmo;

    private AudioSource m_AudioSource;
    private ParticleSystem em;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();

        playerMagazineHUD= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMagazineHUD>();

        leftMagAmmo = playerMagazineHUD.ammo;
        leftInvAmmo = playerMagazineHUD.invAmmo;

        em = GameObject.Find("Sparks").GetComponent<ParticleSystem>();
        em.Stop();
        
    }

    public void Update()
    {
        //se il giocatore ha la pistola
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().GetBool("Pistol")) {
            leftMagAmmo = playerMagazineHUD.ammo;
            leftInvAmmo = playerMagazineHUD.invAmmo;

            animator.SetBool("Fire", false);
            animator.SetBool("Reload", false);
            animator.SetBool("OutOfAmmo", false);
            animator.SetBool("OutOfInvAmmo", false);
            em.Stop();

            //Spara se preme il tasto sinistro del mouse, se non sta già sparando e se ha almeno 1 colpo
            if (Input.GetButtonDown("Fire1") && !(animator.GetCurrentAnimatorStateInfo(0).IsName("Fire")) && leftMagAmmo > 0)
            {
                PlayShootSound();
                em.Play();
                animator.SetBool("Fire", true);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetBool("isFiring", true);
                playerMagazineHUD.AdjAmmo(-1);

            }

            if (Input.GetButton("Reload") && !(animator.GetCurrentAnimatorStateInfo(0).IsName("Reload")) && leftInvAmmo > 0)
            {
                animator.SetBool("Reload", true);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetBool("isReloading", true);
                playerMagazineHUD.Reload();
                PlayReloadSound();
            }

            if (Input.GetButtonDown("Fire1") && leftMagAmmo == 0)
            {
                PlayEmptyMag();
            }

            if (Input.GetButton("Reload") && leftMagAmmo == 0)
            {
                PlayEmptyMagReload();
            }

            if (leftMagAmmo == 0)
            {
                animator.SetBool("OutOfAmmo", true);

            }

            if (leftInvAmmo == 0)
            {
                animator.SetBool("OutOfInvAmmo", true);
            }
        }

    }

    // Update is called once per frame
    void FixedUpdate () {

        

    }

    private void PlayReloadSound()
    {
        m_AudioSource.clip = m_ReloadSound;
        m_AudioSource.Play();
    }

    private void PlayShootSound()
    {
        m_AudioSource.clip = m_ShootSound;
        m_AudioSource.Play();
    }

    private void PlayEmptyMag()
    {
        m_AudioSource.clip = m_EmptyMag;
        m_AudioSource.Play();
    }

    private void PlayEmptyMagReload()
    {
        m_AudioSource.clip = m_EmptyMagReload;
        m_AudioSource.Play();
    }

    public void PlayEquipPistolSound()
    {
        m_AudioSource.clip = m_EquipPistol;
        m_AudioSource.Play();
    }
}
