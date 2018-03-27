using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    //Suoni passi
    [SerializeField] public AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] public AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] public AudioClip m_LandSound;           // the sound played when character touches back on ground.

    //audio
    [SerializeField] private AudioClip m_ReloadSound;
    [SerializeField] private AudioClip m_ShootSound;
    [SerializeField] private AudioClip m_EmptyMag;
    [SerializeField] private AudioClip m_EmptyMagReload;
    [SerializeField] private AudioClip m_EquipPistol;

    private AudioSource m_AudioSource;

    // Use this for initialization
    void Start () {

        m_AudioSource = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Metodi per gestire i suoni del giocatore

    public void PlayFootStepAudio(int speed)
    {
        if (m_AudioSource.isPlaying != true)
        {
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            m_AudioSource.pitch = speed;
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }
    }

    public void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        //m_NextStep = m_StepCycle + .5f;
    }

    public void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }

    public void PlayReloadSound()
    {
        m_AudioSource.clip = m_ReloadSound;
        m_AudioSource.Play();
    }

    public void PlayShootSound()
    {
        m_AudioSource.clip = m_ShootSound;
        m_AudioSource.Play();
    }

    public void PlayEmptyMag()
    {
        m_AudioSource.clip = m_EmptyMag;
        m_AudioSource.Play();
    }

    public void PlayEmptyMagReload()
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
