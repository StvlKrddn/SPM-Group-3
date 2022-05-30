using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : EnemyController
{
    public float timer = 2;
    public int cd = 5;
    public GameObject shot;
    private List<GameObject> bullets = new List<GameObject>();

	// Update is called once per frame
	protected override void OnEnable()
	{
		base.OnEnable();
        timer = Random.Range(timer, cd - 1);
    }

	protected override void Awake()
	{
        base.Awake();
        timer = Random.Range(timer, cd - 1);
	}

	protected override void FixedUpdate()
    {

        MoveStep();
        timer += Time.deltaTime;
        if (timer >= cd)
        {
            ShootPlayer();
            timer = 0;
        }
    }

	protected override void OnDestroy()
	{
        base.OnDestroy();
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
	}

	private void ShootPlayer()
    {
        GameObject bullet;
        int bulletIndex = FindEmptyBullet();
        if (bulletIndex < 0)
        {
            bullet = Instantiate(shot, transform.position, Quaternion.identity, GameManager.Instance.transform.Find("EnemyContainer"));
            bullets.Add(bullet);
        }
        else
        {
            bullet = bullets[bulletIndex];
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
        }
    }

    private int FindEmptyBullet()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets.Count > i && bullets[i].activeSelf == false)
            {
                return i;
            }
        }
        return -1;
    }
}
