using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEnemySound : MonoBehaviour {

    [SerializeField] private AudioClip m_Roar;
    [SerializeField] private AudioClip m_EnemyAttack;
    [SerializeField] private AudioClip m_EnemyHit;
    [SerializeField] private AudioClip m_EnemyDeath;
    private AudioSource m_AudioSource;

    // Use this for initialization
    void Start () {
        m_AudioSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayRoarSound()
    {
        if (m_AudioSource.isPlaying != true)
        {
            m_AudioSource.clip = m_Roar;
            m_AudioSource.Play();
        }
    }

    public void PlayEnemyAttackSound()
    {
        if (m_AudioSource.isPlaying != true)
        {
            m_AudioSource.clip = m_EnemyAttack;
            m_AudioSource.Play();
        }
    }

    public void PlayEnemyHitSound()
    {
        if (m_AudioSource.isPlaying != true)
        {
            m_AudioSource.clip = m_EnemyHit;
            m_AudioSource.Play();
        }
    }

    public void PlayEnemyDeath()
    {
        if (m_AudioSource.isPlaying != true)
        {
            m_AudioSource.clip = m_EnemyDeath;
            m_AudioSource.Play();
        }
    }
}
