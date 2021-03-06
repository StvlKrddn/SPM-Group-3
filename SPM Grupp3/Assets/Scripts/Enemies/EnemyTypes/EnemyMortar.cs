using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMortar : EnemyController
{
    private readonly List<GameObject> shots = new List<GameObject>();
    private Transform explosionRadius = null;
    private TankState[] tanks;
	[SerializeField] private float timer = 2;
    [SerializeField] private int cooldown = 5;
    [SerializeField] private Rigidbody mortarRigidbody;

    // Update is called once per frame
    protected override void Awake()
    {
        base.Awake();
        timer = Random.Range(timer, cooldown - 1);
    }

	protected override void OnEnable()
	{
		base.OnEnable();
        timer = Random.Range(timer, cooldown - 1);
    }

	protected override void OnDestroy()
	{
		base.OnDestroy();
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
        if (timer >= cooldown)
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
        Rigidbody rigidbody;
        int mortarIndex = FindEmptyMortar();
        if (mortarIndex < 0)
        {
            rigidbody = Instantiate(mortarRigidbody, transform.position, mortarRigidbody.rotation, GameManager.Instance.transform.Find("EnemyContainer"));
            rigidbody.velocity = explosionRadius.position;
            shots.Add(rigidbody.gameObject);
        }
        else
        {
            rigidbody = shots[mortarIndex].GetComponent<Rigidbody>();
            rigidbody.velocity = explosionRadius.position;
            rigidbody.transform.SetPositionAndRotation(transform.position, transform.rotation);
            rigidbody.gameObject.SetActive(true);
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
