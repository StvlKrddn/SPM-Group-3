using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float hp = 100f;
    private GameObject tank1;
    private GameObject tank2;
    private GameManager gM;
    private Transform target;
    private int currIndex = 0;
    private int damage = 1;
    private int moneyDrop = 1;
    private float shotTimer = 0f;
    private float shotCD = 3f;
    public Transform bullet;
    public Transform material;


    // Start is called before the first frame update

    private void Awake()
    {
        gM = FindObjectOfType<GameManager>();
        //Change to right tank when done with tanks
        tank1 = FindObjectOfType<TankController>().gameObject;
        tank2 = tank1;
    }

    void Start()
    {
        target = Waypoints.wayPoints[currIndex];
    }

    // Update is called once per frame
    void Update()
    {
        shotTimer += Time.deltaTime;

        if (Vector3.Distance(transform.position, tank1.transform.position) <= 5f || Vector3.Distance(transform.position, tank2.transform.position) <= 5f)
        {
            if (shotTimer >= shotCD) // if tank is in range, shot the player
            {
                ShotPlayer();
                shotTimer = 0;
            }
        }
        else
        {
            //WIP Enemy moves right direction
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            transform.Translate(speed * Time.deltaTime * direction);
            Debug.DrawRay(transform.position, direction, Color.red);
            //float angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
        }

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            NextTarget();
        }
    }

	private void ShotPlayer()
	{
        Instantiate(bullet, transform.position, transform.rotation);
	}

	private void NextTarget()
    {
        if (Waypoints.wayPoints.Length - 1 <= currIndex) // Changes waypoint to til the enemy reaches the last waypoint
        {
            EnemyDeathBase();
            return;
        }
        currIndex++;
        target = Waypoints.wayPoints[currIndex];
    }

    private void EnemyDeathBase()
    {
        gM.TakeDamage(damage);
        Destroy(gameObject);
    }
	private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerShots") //change to tankbullet and turretbullets
        {
           Debug.Log("fdeaf");
            hp -= collision.gameObject.GetComponent<BulletBehavior>().BulletDamage;
            if (hp <= 0)
            {
                EnemyDeath();
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShots"))
        {
            print("Enemy hit!");
            BulletBehavior bullet = other.gameObject.GetComponent<BulletBehavior>();
            hp -= bullet.BulletDamage;
            if (hp < 0)
            {
                EnemyDeath();
            }
        }
    }

    private void EnemyDeath()
    {
        gM.AddMoney(moneyDrop); // add money and spawn material
        Instantiate(material, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
