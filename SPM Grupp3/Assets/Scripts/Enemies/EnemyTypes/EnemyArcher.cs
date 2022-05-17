using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : EnemyController
{
    public float timer = 2;
    public int cd = 5;
    public GameObject shot;

	// Update is called once per frame
	protected override void Awake()
	{
        base.Awake();
        timer = Random.Range(timer, cd - 1);
	}

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
