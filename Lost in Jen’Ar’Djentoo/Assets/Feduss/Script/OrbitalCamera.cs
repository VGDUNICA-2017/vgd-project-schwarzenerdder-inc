using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour {

    //Fov della camera (utile per quando si mira)
    private int start_fov = 60;
    private int end_fov = 35;

    private Vector3 follow_pos; //posizione default della camera quando il player si sposta (orbital camera only)

    private float posX;
    public float Velocità_Y = 1.0f; //Velocità di spostamento verticale
    public float Velocità_X = 1.0f; //Velocità di spostamento orizzontale

    private float Spostamento_Y = 0.0f; 
    private float Spostamento_X;

    private Animator animator;

    private GameObject MainCamera;

    public bool autoaim = false; //debug

    public bool orbitalCamera = false; //true= camera alla resident evil 6

    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();

        MainCamera = GameObject.Find("Camera");

        follow_pos = MainCamera.transform.localPosition;

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

        //camera orbitale
        if (orbitalCamera)
        {

            if ((Input.GetAxis("Vertical") == 0.0) && (Input.GetAxis("Horizontal") == 0.0) && !Input.GetButton("Aim"))
            {

                //MainCamera.transform.SetParent(null); //rendo il transform della camera indipendente
                
                //resetto il fov (esso cambia quando miri)
                MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, start_fov, Time.deltaTime * 5);

                //unione della rotazione verticale con la rotazione intorno al player
                MainCamera.transform.RotateAround(transform.localPosition, Vector3.up, Velocità_X * Input.GetAxis("Mouse X"));
                MainCamera.transform.localEulerAngles = new Vector3(posX, MainCamera.transform.localEulerAngles.y, 0.0f);

            }
            else
            {
                if (autoaim || Input.GetButton("Aim") && !animator.GetBool("Torch") && !animator.GetBool("isCrouching") && !animator.GetBool("isRunning") && !animator.GetBool("isReloading"))
                {

                    transform.eulerAngles = new Vector3(0.0f, Spostamento_X, 0.0f);
                    MainCamera.transform.localEulerAngles = new Vector3(posX, 0.0f, 0.0f);
                    MainCamera.transform.localPosition = follow_pos;
                    MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, end_fov, Time.deltaTime * 5);

                }
                else
                {
                    transform.eulerAngles = new Vector3(0.0f, Spostamento_X, 0.0f);
                    MainCamera.transform.localEulerAngles = new Vector3(posX, 0.0f, 0.0f);
                    MainCamera.transform.localPosition = follow_pos;
                    

                }
            }
        }
        else //camera standard
        {
            MainCamera.transform.SetParent(GameObject.Find("Eyes").transform);
            transform.eulerAngles = new Vector3(0.0f, Spostamento_X, 0.0f);
            MainCamera.transform.localEulerAngles = new Vector3(posX, 0.0f, 0.0f);

            if (animator.GetBool("isAiming"))
            {
                MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, end_fov, Time.deltaTime * 5);
            }
            else
            {
                MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, start_fov, Time.deltaTime * 5);
            }
        }



        
        
    }
}
