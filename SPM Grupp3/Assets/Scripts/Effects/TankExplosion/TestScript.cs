using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject effect;
    public Color tankColor;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TankExplosion tankExplosion = Instantiate(effect).GetComponent<TankExplosion>();
            tankExplosion.TankColor = tankColor;
        }
    }
}
