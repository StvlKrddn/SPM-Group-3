using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    void Start() 
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        while(transform.localScale.x > 0)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            yield return new WaitForSeconds(0.0001f);
        }
        Destroy(transform.parent.gameObject);
    }
}
