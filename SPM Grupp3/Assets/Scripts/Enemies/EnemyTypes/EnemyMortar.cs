using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMortar : EnemyController
{
    public float timer = 2;
    public int cd = 5;
    public GameObject mortarShot;
    public bool shooting;
    public float stayDuration;

	// Update is called once per frame
	protected override void OnEnable()
	{
		base.OnEnable();
        shooting = true;
        timer = Random.Range(timer, cd - 1);
    }

	protected override void Awake()
    {
        base.Awake();
        timer = Random.Range(timer, cd - 1);
    }

    protected override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cd)
        {
            StartCoroutine(ShootMortar());
            timer = 0;
        }

        if (shooting == false)
        {
            MoveStep();
        }

    }

    private IEnumerator ShootMortar()
    {
        shooting = true;
        yield return new WaitForSeconds(stayDuration);
        ShootPlayer();
        yield return new WaitForSeconds(stayDuration / 2);
        shooting = false;
    }

    private void ShootPlayer()
    {
        Instantiate(mortarShot, transform.position, Quaternion.identity);
    }
}
