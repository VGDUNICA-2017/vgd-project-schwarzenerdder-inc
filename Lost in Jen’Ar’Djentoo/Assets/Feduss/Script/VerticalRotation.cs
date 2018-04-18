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
        torcia_imp = GameObject.Find("la Torcia (Impugnata)");

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


       /* if (animator.GetBool("Pistol"))
        {
            if (posX < -15.0f) posX = -15.0f;
            if (posX > 20.0f) posX = 20.0f;
        }
        else*/
        //{
        if (posX < -45.0f) posX = -45.0f;
        if (posX > 60.0f) posX = 60.0f;
        //}

        if (animator.GetBool("WeaponLess") && parte_corpo.gameObject.CompareTag("Testa"))
        {
            parte_corpo.localEulerAngles = new Vector3(posX, transform.rotation.y, 0f);
        }

        //Offset per le braccia (quando hai la pistola
        if (animator.GetBool("Pistol"))
        {
            parte_corpo.localEulerAngles = new Vector3(posX, transform.rotation.y, 0f);
        }

        //Aggiusto l'offset iniziale della rotazione verticale quando è accovacciato
        if (!parte_corpo.gameObject.CompareTag("Braccio_dx") && animator.GetBool("isCrouching") && animator.GetBool("Torch"))
        {
            parte_corpo.localEulerAngles = new Vector3(posX, transform.rotation.y + offset_pistola, 0f);
            torcia_imp.transform.localEulerAngles = torcia_end_angles;
        }

        //offset del braccio sx quando hai la la torcia
        if (!parte_corpo.gameObject.CompareTag("Braccio_dx") && animator.GetBool("Torch") && !animator.GetBool("isCrouching"))
        {   
            parte_corpo.localEulerAngles = new Vector3(posX, transform.rotation.y, 0f);
            torcia_imp.transform.localEulerAngles = torcia_start_angles;
        }


    }
}
