using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalRotation : MonoBehaviour {

    private GameObject testa;
    private GameObject braccio_sx;
    private GameObject braccio_dx;
    private GameObject torcia_imp;

    public float Velocità_Y = 1.0f; //Velocità di spostamento verticale
    public float fattore_rallentamento; //
    private float Spostamento_Y = 0.0f;
    
    private float posX; //Variabile dove salvo la rotazione verticale delle braccia e della testa (vedi lateupdate)

    private Animator animator;

    //Rotazione della torcia (cambia quando ti accovacci)
    private Vector3 torcia_start_angles = new Vector3(5.206f, -21.583f, -92.46301f);
    private Vector3 torcia_end_angles = new Vector3(12.544f, -19.5f, -72.632f);

    public float offset_pistola;

    // Use this for initialization
    void Start () {

        testa = GameObject.FindGameObjectWithTag("Testa");
        braccio_sx = GameObject.FindGameObjectWithTag("Braccio_sx");
        braccio_dx = GameObject.FindGameObjectWithTag("Braccio_dx");
        animator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        /////////////////
        //ROTAZIONE VERTICALE DELLE BRACCIA 
        RotazioneVerticale(braccio_dx.transform, Velocità_Y);
        RotazioneVerticale(braccio_sx.transform, Velocità_Y);
    }

    public void RotazioneVerticale(Transform parte_corpo, float Velocità_rotazione)
    {
        Spostamento_Y = -(Velocità_rotazione * Input.GetAxis("Mouse Y"));

        posX += Spostamento_Y;

        //Specifico gli angoli massimo e minimo di rotazione, normalizzando quelli che superano tali limiti
        if (posX < -45.0f) posX = -35.0f;
        if (posX > 60.0f) posX = 50.0f;

        //Rotazione verticale delle braccia a seconda dell'arma impugnata
        if ((animator.GetBool("Pistol") || animator.GetBool("Smg"))/* && parte_corpo.gameObject.CompareTag("Braccio_dx")*/)
        {
            parte_corpo.localEulerAngles = new Vector3(posX, parte_corpo.localEulerAngles.y, parte_corpo.localEulerAngles.z);
        }
        if (animator.GetBool("Axe") && parte_corpo.gameObject.CompareTag("Braccio_sx"))
        {
            parte_corpo.localEulerAngles = new Vector3(posX, parte_corpo.localEulerAngles.y, parte_corpo.localEulerAngles.z);
        }

        


    }
}
