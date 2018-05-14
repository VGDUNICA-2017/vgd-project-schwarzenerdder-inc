using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour {

    public int startingBossHealth = 500;
    public int currentHealth;

    Animator anim;
    CapsuleCollider capsuleCollider;
    bool isDead = false;

    private HUDSystem hud;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        currentHealth = startingBossHealth;

        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage (int amount) {
        if (isDead) return;
        currentHealth -= amount;
        hud.bossBarSetter(startingBossHealth, currentHealth);
        if (currentHealth <= 0) Death();
    }

    void Death() {
        isDead = true;
        capsuleCollider.isTrigger = true;
        anim.SetTrigger("Die");
    }
}
