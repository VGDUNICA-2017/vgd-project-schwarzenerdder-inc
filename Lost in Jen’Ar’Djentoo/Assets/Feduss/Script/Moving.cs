using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    /// <summary>
    /// author: feduss
    /// </summary>
    private float horizontal;
    private float vertical;
    private Vector3 moveDirection;
    public float speed;

    public float Velocità_Y = 1.0f; //Velocità di spostamento verticale
    private float Spostamento_Y = 0.0f;

    public float Velocità_X = 1.0f; //Velocità di spostamento verticale
    private float Spostamento_X = 0.0f;

    private float posX;
    private float posY;

    //Fov della camera (utile per quando si mira)
    private int start_fov = 60;
    private int end_fov = 35;

    private CharacterController controller;
    private Animator animator;
    private Camera cam;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();
    }

    private void LateUpdate()
    {
        mouseRotation(); //nel lateupdate per le braccia sono animate, e devo modificare la rotazione prima del rendering dell'animazione stessa
    }

    public void move()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        moveDirection = new Vector3(horizontal, 0f, vertical); //calcolo lo spostamento
        moveDirection = cam.transform.TransformDirection(moveDirection); //converto la direzione del transform (da locale a globale)

        // applico la gravità

        moveDirection.y += Physics.gravity.y;

        //muovo il character controller

        //se corre, la velocità è doppia
        if (animator.GetBool("Run"))
        {
            controller.Move(moveDirection * Time.deltaTime * speed * 2);
        }
        else
        {
            controller.Move(moveDirection * Time.deltaTime * speed);
        }
        
    }


    public void mouseRotation()
    {
        Spostamento_Y = -(Velocità_Y * Input.GetAxis("Mouse Y"));
        Spostamento_X = (Velocità_X * Input.GetAxis("Mouse X"));

        posX += Spostamento_Y; //rotazione verticale
        posY += Spostamento_X; //rotazione orizzontale

        //Vincolo la rotazione verticale
        if (posX < -45.0f) posX = -45.0f;
        if (posX > 60.0f) posX = 60.0f;


        //Aggiorno la rotazione del player
        GameObject.FindGameObjectWithTag("Hands").transform.localEulerAngles = new Vector3(posX-90f, posY, 0f);
        GameObject.FindGameObjectWithTag("MainCamera").transform.localEulerAngles = new Vector3(posX, posY, 0f);

        //Se mira, cambio il fov (non puoi mirare se hai la torcia o l'ascia, o stai correndo o ricaricando)
        if ((Input.GetButton("Aim") && (animator.GetBool("Pistol") || !animator.GetBool("Smg")) && !animator.GetBool("Run") && !animator.GetBool("Reload")))
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, end_fov, Time.deltaTime * 5);

        }
        else
        {
            //resetto il fov (esso cambia quando mira)
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, start_fov, Time.deltaTime * 5);


        }
    }
}
