using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject[] effects;
    [SerializeField] private Transform[] spawns;
    
    private GameObject[] spawnedEffects;

    void Start() 
    {
        spawnedEffects = new GameObject[effects.Length];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < effects.Length; i++)
            {
                if (spawnedEffects[i] != null)
                {
                    Destroy(spawnedEffects[i]);
                }
                spawnedEffects[i] = Instantiate(effects[i], spawns[i].position, Quaternion.identity);
            }
        }        
    }
}
