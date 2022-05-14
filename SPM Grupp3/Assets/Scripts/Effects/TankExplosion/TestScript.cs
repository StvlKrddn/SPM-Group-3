using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject effect;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(effect);
        }
    }
}
