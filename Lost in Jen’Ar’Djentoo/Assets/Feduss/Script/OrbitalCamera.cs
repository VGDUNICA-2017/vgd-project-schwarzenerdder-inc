using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour {

    //Fov della camera (utile per quando si mira)
    private int start_fov = 60;
    private int end_fov = 35;

    //temp
    private Vector3 start_pos = new Vector3(1.5f, 3.43f, -3f);
    private Vector3 start_angles = new Vector3();
    private float posX;
    public float Velocità_Y = 1.0f; //Velocità di spostamento verticale
    private float Spostamento_Y = 0.0f;

    public float Velocità_X;
    private float Spostamento_X;
    private Vector3 offset;

    private Animator animator;

    private GameObject MainCamera;

    public bool autoaim = false; //debug

    private VerticalRotation vr;

    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();

        MainCamera = GameObject.Find("Camera");

        vr=GetComponent<VerticalRotation>();

        offset = transform.position - MainCamera.transform.position;

	}
	
	// Update is called once per frame
	void Update () {

        CameraRotation();

    }

    private void CameraRotation()
    {
        //Ricavo l'angolo di rotazione orizzontale in base allo spostamento del mouse
        Spostamento_X += Velocità_X * Input.GetAxis("Mouse X");

        Spostamento_Y = -(Velocità_Y * Input.GetAxis("Mouse Y"));

        posX += Spostamento_Y;


        if (animator.GetBool("Pistol"))
        {
            if (posX < -15.0f) posX = -15.0f;
            if (posX > 20.0f) posX = 20.0f;
        }
        else
        {
            if (posX < -45.0f) posX = -45.0f;
            if (posX > 60.0f) posX = 60.0f;
        }

        //MainCamera.transform.localPosition = start_pos;
        //MainCamera.transform.localEulerAngles = start_angles;

        if ((Input.GetAxis("Vertical") == 0.0) && (Input.GetAxis("Horizontal") == 0.0))
        {
            print("fermo" + offset);
            MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, start_fov, Time.deltaTime * 5);
            
            MainCamera.transform.RotateAround(transform.localPosition, Vector3.up, Velocità_X * Input.GetAxis("Mouse X"));
        }
        else
        {
            if (autoaim || Input.GetButton("Aim") && !animator.GetBool("Torch") && !animator.GetBool("isCrouching") && !animator.GetBool("isRunning") && !animator.GetBool("isReloading"))
            {
                print("mira");
                vr.RotazioneVerticale(MainCamera.transform, 1);

                MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, end_fov, Time.deltaTime * 5);
            }
            else
            {
                print("movimento");
                MainCamera.transform.position = transform.position - offset;

                transform.eulerAngles = MainCamera.transform.forward;
                transform.localEulerAngles = new Vector3(0.0f, Spostamento_X, 0.0f);
                MainCamera.transform.eulerAngles = transform.forward;
                //MainCamera.transform.localEulerAngles = new Vector3(posX, transform.rotation.y, 0f);



            }
        }
        


        /*if ((Input.GetAxis("Vertical") == 0.0) && (Input.GetAxis("Horizontal") == 0.0))
        {

            print("we");

            //Ricavo l'angolo di rotazione orizzontale in base allo spostamento del mouse
            Spostamento_X = Velocità_X * Input.GetAxis("Mouse X");

            MainCamera.transform.RotateAround(transform.localPosition, Vector3.up, Spostamento_X);
        }
        else
        {
            RotazioneVerticale(MainCamera.transform, Velocità_Y / fattore_rallentamento);


        }*/
    }
}
