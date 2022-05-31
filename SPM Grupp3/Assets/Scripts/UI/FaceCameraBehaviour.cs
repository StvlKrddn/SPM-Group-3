using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraBehaviour : MonoBehaviour
{
    private Transform lookAt;

    private void Awake()
    {
        lookAt = GameObject.FindGameObjectWithTag("Look").transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(lookAt);

        transform.Rotate(90, 180, 0);
    }
}
