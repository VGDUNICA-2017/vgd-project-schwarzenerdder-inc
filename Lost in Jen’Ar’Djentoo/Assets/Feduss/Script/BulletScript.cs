using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public GameObject fire_fx;
    private GameObject start_bullet;
    public GameObject bullet_impact;
    public GameObject bullet_impact_generic;
    private ContactPoint contact;
    public float offset;

    // Use this for initialization
    void Start () {

        start_bullet = GameObject.Find("Start_Bullet");

        //Instantiate(fire_fx, transform);

        //Aggiungo la forza dello sparo al proiettile, frontalmente rispetto al gamebject vuoto usato come riferimento
        GetComponent<Rigidbody>().AddForce(start_bullet.transform.forward*100f, ForceMode.Impulse);

        //Distrugge il colpo se non colpisce niente
        if (gameObject!=null) Destroy(gameObject, 15f);
    }

    public void OnCollisionEnter(Collision other)
    {
        //Effetto particellare dell'impatto del proiettile
        

        contact = other.contacts[0]; //punto di collisione
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal); //aggiusto la rotazione
        Vector3 pos = contact.point;

        if (other.gameObject.tag != "Enemy") Instantiate(bullet_impact_generic, pos, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
        else Instantiate(bullet_impact, pos, rot);

        Destroy(gameObject);
        


    }

}
