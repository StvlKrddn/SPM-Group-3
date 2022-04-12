using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed = 10f;
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
        tank1 = FindObjectOfType<TankController>().gameObject;
        tank2 = FindObjectOfType<TankController>().gameObject;
    }

    void Start()
    {
        //Checks who is closer between tank1 and tank2
        if (Vector3.Distance(transform.position, tank1.transform.position) < Vector3.Distance(transform.position, tank2.transform.position))
        {
            target = tank1.transform;
        }
        else
        {
            target = tank2.transform;
        }
        direction = target.position - transform.position; //Checks direction
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(speed * Time.deltaTime * direction); //Bullet travels
        if (timer >= bulletTime)
        {
            Destroy(gameObject);
        }
    }
}
