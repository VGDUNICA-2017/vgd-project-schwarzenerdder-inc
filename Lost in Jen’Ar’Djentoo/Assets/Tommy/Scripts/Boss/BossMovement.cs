using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour {

    Transform player;
    NavMeshAgent nav;
    Animator anim;
    public float attackDistance = 5.0f;
    public float chaseDistance = 30.0f;
    private float currentDistance;

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        print(nav.enabled);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Mutant Attack") && GameObject.Find("TriggerFight").GetComponent<BossFight>().fightStarted == true) {
            nav.enabled = true;
        }
        /*if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump Attack")) {
            nav.speed = 25;
            nav.angularSpeed = 120;
            nav.acceleration = 8;
        }*/

        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Mutant Attack")) { 
            nav.enabled = false;
        }
        if (nav.enabled == true) {

            nav.SetDestination(player.position);
            currentDistance = Vector3.Distance(this.transform.position, player.position);
            //print("distance: " + currentDistance);

            /*if (currentDistance > chaseDistance) Jump();
            else */if (currentDistance > attackDistance) Chase();
            else Attack();
        }
        
	}

    /*void Jump() {
        anim.SetBool("IsWalking", false);
        anim.SetBool("JumpAttack", true);
        nav.speed = 2000;
        nav.angularSpeed = 500;
        nav.acceleration = 18;
    }*/

    void Chase() {
        anim.SetBool("JumpAttack", false);
        anim.SetBool("IsWalking", true);
    }

    void Attack() {
        anim.SetBool("IsWalking", false);
        anim.SetBool("JumpAttack", false);
        nav.enabled = false;
        anim.SetTrigger("AttackTrigger");
    }
}
