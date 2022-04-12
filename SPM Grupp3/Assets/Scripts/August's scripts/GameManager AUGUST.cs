using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_AUGUST : MonoBehaviour
{
    public static GameManager_AUGUST instance;

    private static float materials;

    public static float Materials { get { return materials; } set { materials = value; } }

    public static GameManager_AUGUST Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        //print(materials);
    }
}
