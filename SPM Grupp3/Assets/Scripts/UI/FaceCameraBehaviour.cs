using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraBehaviour : MonoBehaviour
{
    private Transform lookAt;
    public bool isHealth = true;

    private void Awake()
    {
        lookAt = GameObject.FindGameObjectWithTag("Look").transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(lookAt);

        if(isHealth)
            transform.Rotate(90, 180, 0);
        else
            transform.Rotate(0, 180, -90);
    }
}
