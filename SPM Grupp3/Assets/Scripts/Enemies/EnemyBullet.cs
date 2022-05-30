using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;
    private TankState[] tanks;
    Vector3 direction;
    public float bulletTime = 5f;
    public float timer = 0;
    public float damage = 5;
    private Transform garageTrigger;
	// Start is called before the first frame update

	private void OnEnable()
	{
        timer = 0;
		Invoke(nameof(FindTarget), 0.01f);
	}

	void Start()
    {
        garageTrigger = FindObjectOfType<GarageTrigger>().transform.parent;
        FindTarget();
        //Checks who is closer between tank1 and tank2
    }

    private void FindTarget()
    {
        if (FindObjectOfType<TankState>())
        {
            tanks = FindObjectsOfType<TankState>();
            TankState tankToTarget = tanks[Random.Range(0, tanks.Length)];
            target = tankToTarget.transform;

        }
        else
        {
            target = garageTrigger;
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
        transform.Translate(speed * Time.deltaTime * direction, Space.World); //Bullet travels
        if (timer >= bulletTime)
        {
            gameObject.SetActive(false);
        }
    }
    public int GetDamage()
    {
        return (int) damage;
    }
}
