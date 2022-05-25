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
    // Start is called before the first frame update

    void Start()
    {
        //Checks who is closer between tank1 and tank2
        if (FindObjectOfType<TankState>())
        {
            tanks = FindObjectsOfType<TankState>();
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
        direction = target.position - transform.position; //Checks direction
        direction.Normalize();
        direction.y = 0;
        transform.LookAt(direction);
    }

	private void OnTriggerEnter(Collider other)
	{
    //    if (!other.CompareTag("Enemy"))
    //    {
    //        Destroy(gameObject, Mathf.Epsilon);
    //    }


        if(other.CompareTag("Player"))
        {
            Destroy(gameObject, Mathf.Epsilon);
        }
	}

	// Update is called once per frame
	void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(speed * Time.deltaTime * direction, Space.World); //Bullet travels
        if (timer >= bulletTime)
        {
            Destroy(gameObject);
        }
    }
    public int GetDamage()
    {
        return (int) damage;
    }
}
