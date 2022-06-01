using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : EnemyController
{
    private float timer = 2;
    private readonly float yOffset = 1;
    private readonly List<GameObject> bullets = new List<GameObject>();
    [SerializeField] private int cooldown = 5;
    [SerializeField] private GameObject shot;

	protected override void Awake()
	{
        base.Awake();
        timer = cooldown - 1;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
        timer = cooldown - 1;
    }

	protected override void FixedUpdate()
    {
        MoveStep();
        timer += Time.deltaTime;
        if (timer >= cooldown)
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
            bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            bullet.SetActive(true);
        }
        bullet.transform.position += new Vector3(0, yOffset, 0);
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
