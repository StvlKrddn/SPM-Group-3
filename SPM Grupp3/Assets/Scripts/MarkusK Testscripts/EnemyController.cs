using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float hp = 100f;
    [SerializeField] private GameObject hitEffect;
    private GameObject tank1;
    private GameObject tank2;
    private GameManager gM;
    private Transform target;
    private int currIndex = 0;
    private int damage = 10;
    private int moneyDrop = 10;
    private float shotTimer = 0f;
    private float shotCD = 3f;
    public Transform bullet;
    public Transform material;

    public GameObject hitByPoisonEffect;
    private float defaultSpeed;
    public List<float> poisonTickTimers = new List<float>();


    // Start is called before the first frame update

    private void Awake()
    {
        defaultSpeed = speed;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShots"))
        {
            BulletBehavior bullet = other.gameObject.GetComponent<BulletBehavior>();
            GameObject hitEffektInstance = Instantiate(hitEffect, transform.position, transform.rotation);
            TakeDamage(bullet.BulletDamage);
            Destroy(hitEffektInstance, 1f);
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        gM.AddMoney(moneyDrop); // add money and spawn material
        Instantiate(material, transform.position, transform.rotation);
        DieEvent dieEvent = new DieEvent("död", gameObject, null, null);
        EventHandler.Instance.InvokeEvent(dieEvent);
        Destroy(gameObject);
    }

    public void HitBySlow(float slowProc, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in colliders)
        {
            if (c.GetComponent<EnemyController>())
            {
                EnemyController eC = c.GetComponent<EnemyController>();
                eC.speed *= slowProc;
                eC.Invoke("SlowDuration", 3f);
            }
        }

/*        Invoke("SlowDuration", 3f);*/
    }

    void SlowDuration()
    {

        speed = defaultSpeed;
    }

    public void HitByPoison(float ticks, float dps)
    {
        GameObject poisonEffect = Instantiate(hitByPoisonEffect, gameObject.transform);
        Destroy(poisonEffect, ticks);
        if (poisonTickTimers.Count <= 0)
        {
            poisonTickTimers.Add(ticks);
            StartCoroutine(PoisonTick(dps));
        }
    }

    IEnumerator PoisonTick(float dps)
    {
        while (poisonTickTimers.Count > 0)
        {
            for (int i = 0; i < poisonTickTimers.Count; i++)
            {
                poisonTickTimers[i]--;
            }
            TakeDamage(dps);
            poisonTickTimers.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(0.75f);
        }
    }

    public void HitBySplash(float radius, float splashDamage)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in colliders)
        {
            if (c.GetComponent<EnemyController>())
            {
                c.GetComponent<EnemyController>().TakeDamage(splashDamage);
            }
        }
    }
}
