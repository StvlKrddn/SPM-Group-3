using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    private TankState[] tanks;
    Vector3 direction;
    public float bulletTime = 5f;
    public float timer = 0;
    public float damage = 5;
    // Start is called before the first frame update


    public void SetTarget(Transform target)
    {
        direction = target.position - transform.position; //Checks direction
        direction.Normalize();
        direction.y = 0;
        transform.LookAt(direction);
    }

	private void OnTriggerEnter(Collider other)
	{
        if (!other.CompareTag("Enemy"))
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
