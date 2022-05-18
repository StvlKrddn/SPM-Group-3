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
            StartCoroutine( ShootPlayer());
            timer = 0;
        }
    }

    private IEnumerator ShootPlayer()
    {
        Transform target = FindTarget();
        transform.LookAt(target);
        yield return new WaitForSeconds(1);
        GameObject g = Instantiate(shot, transform.position, Quaternion.identity);
        g.GetComponent<EnemyBullet>().SetTarget(target);
        transform.LookAt(Waypoints.wayPoints[path][currWaypointIndex]);
    }

    private Transform FindTarget()
    {
        Transform target = null;
        if (FindObjectOfType<TankState>()) //Find player
        {
            TankState[] tanks = FindObjectsOfType<TankState>();
            foreach (TankState tank in tanks)
            {
                if (target == null || Vector2.Distance(tank.transform.position, transform.position) < Vector3.Distance(target.position, transform.position))
                {
                    target = tank.transform;
                }
            }

        }
        else
        {
            target = FindObjectOfType<GarageTrigger>().gameObject.transform;
        }
        return target;
    }
}
