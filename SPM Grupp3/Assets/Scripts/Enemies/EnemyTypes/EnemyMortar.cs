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
    private List<GameObject> shots = new List<GameObject>();

    // Update is called once per frame
    protected override void Awake()
    {
        base.Awake();
        timer = Random.Range(timer, cd - 1);
    }

	protected override void OnEnable()
	{
		base.OnEnable();
        timer = Random.Range(timer, cd - 1);
    }

	private void OnDestroy()
	{
        foreach (GameObject shot in shots)
        {
            Destroy(shot);
        }
        shots.Clear();
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


    private void LaunchShot()
    {
        int mortarIndex = FindEmptyMortar();
        if (mortarIndex < 0)
        {
            Rigidbody rigidbody = Instantiate(mortarRigidbody, transform.position, mortarRigidbody.rotation, GameManager.Instance.transform.Find("EnemyContainer"));
            rigidbody.velocity = explosionRadius.position;
            shots.Add(rigidbody.gameObject);
        }
    }

    private int FindEmptyMortar()
    {
        for (int i = 0; i < shots.Count; i++)
        {
            if (shots.Count > i && shots[i].activeSelf == false)
            {
                return i;
            }
        }
        return -1;
    }
}
