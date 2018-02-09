using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeWeapon : MonoBehaviour
{

    public Transform attachPoint;
    public GameObject owner;

    void Start()
    {
        if (owner != null)
        {
            transform.localScale = owner.transform.localScale;
        }
    }

    void Update()
    {
        if (owner != null)
        {
            transform.position = attachPoint.position;
            transform.rotation = attachPoint.rotation;
        }
        else Destroy(gameObject);
    }
}
