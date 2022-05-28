using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMortar : EnemyController
{
    public float timer = 2;
    public int cd = 5;
    private TankState[] tanks;
    private Transform explosionRadius = null;
    public Rigidbody mortarRigidbody;

    // Update is called once per frame
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
            CalculateTarget();
            if (explosionRadius != null)
            {
                CalculateVelocity(3);
                LaunchShot();
            }
            timer = 0;
            explosionRadius = null;
        }
    }

    private void CalculateTarget()
    {
        if (FindObjectOfType<TankState>())
        {
            tanks = FindObjectsOfType<TankState>();
            foreach (TankState tank in tanks)
            {
                if (explosionRadius == null || Vector2.Distance(tank.transform.position, transform.position) < Vector3.Distance(explosionRadius.position, transform.position))
                {
                    explosionRadius = tank.transform;
                }
            }

        }
        else
        {
            timer = 2;
        }
    }

    private Vector3 CalculateVelocity(float duration)
    {
        Vector3 distance = explosionRadius.position - transform.position;
        Vector3 distanceXZ = new Vector3(distance.x, 0, distance.z);

        float yDistance = distance.y;
        float xzDistance = distanceXZ.magnitude;

        float xzVelocity = xzDistance / duration;
        float yVelocity = yDistance / duration + 0.5f * Mathf.Abs(Physics.gravity.y) * duration;

        Vector3 result = distanceXZ.normalized;
        result *= xzVelocity;
        result.y = yVelocity;

        return result;
    }

    private void LaunchShot()
    {
        Rigidbody rigidbody = Instantiate(mortarRigidbody, transform.position, Quaternion.identity);
        rigidbody.velocity = explosionRadius.position;
    }
}
