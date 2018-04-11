using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCamera : MonoBehaviour {

    public float Velocità_Y = 1.0f; //Velocità di spostamento verticale
    private float Spostamento_Y = 0.0f;

    public float Velocità_X = 1.0f; //Velocità di spostamento verticale
    private float Spostamento_X = 0.0f;

    private GameObject player;

    private float posX; //Variabile dove salvo la rotazione verticale delle braccia e della testa (vedi lateupdate)
    private float posY;

    Quaternion rotation; 

    // Use this for initialization
    void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        rotation = transform.rotation; //sovrascrivo l'attuale rotazione della camera (serve per rendere la sua rotazione indipendente da quella del genitore)

    }
	
	// Update is called once per frame
	void Update () {




        Spostamento_Y = -(Velocità_Y * Input.GetAxis("Mouse Y"));
        Spostamento_X = (Velocità_X * Input.GetAxis("Mouse X"));

        posX += Spostamento_Y; //rotazione verticale
        posY += Spostamento_X; //rotazione orizzontale

        //limito la rotazione verticale della camera in base all'animazone corrente
        if (player.GetComponent<Animator>().GetBool("Pistol"))
        {
            if (posX < -15.0f) posX = -15.0f;
            if (posX > 20.0f) posX = 20.0f;
        }
        else
        {
            if (posX < -45.0f) posX = -45.0f;
            if (posX > 60.0f) posX = 60.0f;
        }

        rotation = Quaternion.Euler(new Vector3(posX, posY, 0f)); //salvo la nuova rotazione della camera in base al movimento del mouse 

    }

    void LateUpdate()
    {
        //setto le nuove rotazioni

         player.transform.eulerAngles = new Vector3(0f, player.transform.eulerAngles.y + Spostamento_X, 0f);

         
         transform.rotation = rotation;
    }
}
