using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    int delay = 2;
    float repeatRate = 0.01f;
    float shrinkRate;
    float shrinkAmount = 0.03f;

    void Start()
    {
        shrinkRate = transform.localScale.x * shrinkAmount;
        InvokeRepeating(nameof(Shrink), delay, repeatRate);
    }

    void Shrink()
    {
        transform.localScale -= new Vector3(shrinkRate, shrinkRate, shrinkRate);
        if (transform.localScale.x < 0)
        {
            Destroy(gameObject);
        }
    }
}
