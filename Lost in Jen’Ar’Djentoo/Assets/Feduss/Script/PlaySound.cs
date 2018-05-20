using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    //Suoni passi
    [SerializeField] public AudioClip[] m_FootstepSounds_Indoor;
    [SerializeField] public AudioClip[] m_FootstepSounds_Snow;    // an array of footstep sounds that will be randomly selected from.

    [SerializeField] private AudioClip m_PlayerHit;
    [SerializeField] private AudioClip m_Spotted;
    [SerializeField] private AudioClip m_PlayerDeath;

    private AudioSource m_AudioSource;

    // Use this for initialization
    void Start () {

        m_AudioSource = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Metodi per gestire i suoni del giocatore

    public void PlayFootStepAudioSnow(int speed)
    {
        if (m_AudioSource.isPlaying != true)
        {
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            m_AudioSource.pitch = speed;
            int n = Random.Range(1, m_FootstepSounds_Snow.Length);
            m_AudioSource.clip = m_FootstepSounds_Snow[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds_Snow[n] = m_FootstepSounds_Snow[0];
            m_FootstepSounds_Snow[0] = m_AudioSource.clip;
        }
    }

    public void PlayFootStepAudioIndoor(int speed)
    {
        if (m_AudioSource.isPlaying != true)
        {
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            m_AudioSource.pitch = speed;
            int n = Random.Range(1, m_FootstepSounds_Indoor.Length);
            m_AudioSource.clip = m_FootstepSounds_Indoor[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds_Indoor[n] = m_FootstepSounds_Indoor[0];
            m_FootstepSounds_Indoor[0] = m_AudioSource.clip;
        }
    }

    public void PlayReloadSound(AudioClip ac)
    {
        m_AudioSource.clip = ac;
        m_AudioSource.Play();
    }

    public void PlayShootSound(AudioClip ac)
    {
        m_AudioSource.clip = ac;
        m_AudioSource.Play();
    }

    public void PlayEmptyMag(AudioClip ac)
    {
        m_AudioSource.clip = ac;
        m_AudioSource.Play();
    }

    public void PlayEmptyMagReload(AudioClip ac)
    {
        m_AudioSource.clip = ac;
        m_AudioSource.Play();
    }

    public void PlayEquipPistolSound(AudioClip ac)
    {
        m_AudioSource.clip = ac;
        m_AudioSource.Play();
    }

    public void PlayAxeAttackSound(AudioClip ac)
    {
        if (m_AudioSource.isPlaying != true)
        {
            m_AudioSource.clip = ac;
            m_AudioSource.Play();
        }
    }

    public void PlayPlayerHitSound()
    {
        if (m_AudioSource.isPlaying != true)
        {
            m_AudioSource.clip = m_PlayerHit;
            m_AudioSource.Play();
        }
    }

    public void PlaySpottedSong()
    {
        if (m_AudioSource.isPlaying != true)
        {
            m_AudioSource.clip = m_Spotted;
            m_AudioSource.Play();
        }
    }

   

    public void PlayPlayerDeath()
    {
        if (m_AudioSource.isPlaying != true)
        {
            m_AudioSource.clip = m_PlayerDeath;
            m_AudioSource.Play();
        }
    }
}
