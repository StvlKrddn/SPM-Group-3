using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float start = 0f;
    [SerializeField] private float from;
    [SerializeField] private float to;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
