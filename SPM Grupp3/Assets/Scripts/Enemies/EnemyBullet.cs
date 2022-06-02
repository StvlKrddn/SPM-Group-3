using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Transform target;
    private TankState[] tanks;
    private Vector3 direction;
    [SerializeField] private float timer = 0;

    public float Speed = 5f;
    public float BulletTime = 5f;
    public int Damage = 5;
	// Start is called before the first frame update

	private void OnEnable()
	{
        timer = 0;
		Invoke(nameof(FindTarget), 0.01f);
	}

	void Start()
    {
        FindTarget();
        //Gets one random tank, if there is not one tank he focuses on the garage
    }

    private void FindTarget()
    {
        if (FindObjectOfType<TankState>())
        {
            tanks = FindObjectsOfType<TankState>();
            TankState tankToTarget = tanks[Random.Range(0, tanks.Length)];
            target = tankToTarget.transform;

        }

        direction = target.position - transform.position; //Checks direction
        direction.Normalize();
        direction.y = 0;
        transform.LookAt(direction);
    }

	private void OnTriggerEnter(Collider other)
	{
        if(other.CompareTag("Tank"))
        {
            gameObject.SetActive(false);
        }
	}

	// Update is called once per frame
	void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(Speed * Time.deltaTime * direction, Space.World); //Bullet travels
        if (timer >= BulletTime)
        {
            gameObject.SetActive(false);
        }
    }
}
