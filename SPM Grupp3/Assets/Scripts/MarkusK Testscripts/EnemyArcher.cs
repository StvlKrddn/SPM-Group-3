using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : EnemyController
{
    public float timer = 2;
    public int cd = 3;
    public GameObject shot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        MoveStep();
        timer += Time.deltaTime;
        if (timer >= cd)
        {
            ShootPlayer();
            timer = 0;
        }
    }

    private void ShootPlayer()
    {
        Instantiate(shot, transform.position, Quaternion.identity);
    }
}
