using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;
    public GameObject tank1;
    public GameObject tank2;
    Vector3 direction;
    public float bulletTime = 5f;
    public float timer = 0;
    public float damage = 5;
    // Start is called before the first frame update

    private void Awake()
    {
        //Change to right tank when done with tanks
    }

    void Start()
    {
        //Checks who is closer between tank1 and tank2
        if (FindObjectOfType<TankState>())
        {
            tank1 = FindObjectOfType<TankState>().gameObject; //Needs change 
            tank2 = FindObjectOfType<TankState>().gameObject;
            if (Vector3.Distance(transform.position, tank1.transform.position) < Vector3.Distance(transform.position, tank2.transform.position))
            {
                target = tank1.transform;
            }
            else
            {
                target = tank2.transform;
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
