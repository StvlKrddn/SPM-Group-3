using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyRoundUp : MonoBehaviour
{
    [SerializeField] private Text text;
    private float amountOfMoney;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string t = text.text;

        amountOfMoney = float.Parse(t);

        if (amountOfMoney / 1000 >= 1)
        {
            amountOfMoney /= 1000;
            t = amountOfMoney + " k";

            text.text = t;
        }
    }
}
