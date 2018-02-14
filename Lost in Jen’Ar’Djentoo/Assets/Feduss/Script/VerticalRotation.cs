using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalRotation : MonoBehaviour {

    public float Velocità_Y;
    private float Spostamento_Y = 0.0f;

    private Animator animator;
    public GameObject fin;

    private float posX;

    // Use this for initialization
    void Start () {

        animator = fin.GetComponent<Animator>();

        posX = 0.0f;
    }
    

    public void LateUpdate()
    {

        if (gameObject.CompareTag("Testa")) RotazioneVerticale();

        if (animator.GetBool("Torch")) RotazioneVerticale();
    }

    public void RotazioneVerticale()
    {
        Spostamento_Y = -(Velocità_Y * Input.GetAxis("Mouse Y"));

        posX += Spostamento_Y;

        if (posX < -45.0f) posX = -45.0f;
        if (posX > 60.0f) posX = 60.0f;

        transform.Rotate(new Vector3(posX, 0.0f, 0.0f));

    }
}
