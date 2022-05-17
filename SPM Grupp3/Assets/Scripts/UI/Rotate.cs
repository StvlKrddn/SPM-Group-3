using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private float start = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        start++;

        Quaternion rot = Quaternion.Euler(0f, start, 0f);
        gameObject.transform.rotation = rot;
        if (start == 360)
        {
            start = 0;
        }
    }
}
