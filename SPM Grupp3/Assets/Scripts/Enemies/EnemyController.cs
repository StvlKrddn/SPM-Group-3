using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public float speed = 10f;
    [SerializeField] private float health = 100f;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float meleeDamage;
    private GameManager gM;
    private Transform target;
    private int currIndex = 0;
    public int damageBase = 10;
    public int moneyDrop = 10;
    public bool materialDrop = false;
    public Transform material;

    private GameObject hitByPoisonEffect;
    private float defaultSpeed;
    private List<float> poisonTickTimers = new List<float>();
    public bool spread = false;
    private float amountOfTicks;
    private float amountOfDps;
    private bool dead = false;
    private float currentHealth;

    public float MeleeDamage { get { return meleeDamage; } set { meleeDamage = value; } }
    public float Health { get { return health; } }

    // Start is called before the first frame update

    protected virtual void Awake()
    {
        defaultSpeed = speed;
        currentHealth = health;
        gM = GameManager.Instance;
        target = Waypoints.wayPoints[currIndex];
        //Change to right tank when done with tanks
    }

    protected virtual void Start() {}

    // Update is called once per frame
    protected virtual void Update()
    {
        MoveStep();   
    }

    public void MoveStep()
    {
        //WIP Enemy moves right direction
        Vector3 direction = target.position - transform.position;
        //gameObject.transform.LookAt(target);
        direction.Normalize();
        transform.Translate(speed * Time.deltaTime * direction);
        Debug.DrawRay(transform.position, direction * 100, Color.red);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            NextTarget();
        }
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
        gM.TakeDamage(damageBase, gameObject);
        DieEvent dieEvent = new DieEvent("d�d fr�n bas", gameObject, null, null);
        EventHandler.Instance.InvokeEvent(dieEvent);
        Destroy(gameObject);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShots"))
        {
            BulletBehavior bullet = other.gameObject.GetComponent<BulletBehavior>();
            GameObject hitEffektInstance = Instantiate(hitEffect, transform.position, transform.rotation);
            TakeDamage(bullet.BulletDamage);
            Destroy(hitEffektInstance, 1f);
        }
    }*/

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && dead == false)
        {
            dead = true;
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        gM.AddMoney(moneyDrop); // add money and spawn material
        if (materialDrop == true)
        {
            Instantiate(material, transform.position, transform.rotation);
        }
        DieEvent dieEvent = new DieEvent("d�d", gameObject, null, null);
        EventHandler.Instance.InvokeEvent(dieEvent);
        Destroy(gameObject);
    }

    public void HitBySlow(float slowProc, float radius, bool single)
    {
        if (single)
        {
            speed = slowProc;
            Invoke(nameof(SlowDuration), 3f);
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider c in colliders)
            {
                if (c.GetComponent<EnemyController>())
                {
                    EnemyController eC = c.GetComponent<EnemyController>();
                    eC.speed = slowProc;
                    eC.Invoke(nameof(SlowDuration), 3f);
                }
            }
        }       
    }

    private void SlowDuration()
    {

        speed = defaultSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (spread)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (poisonTickTimers != null)
                {
                    EnemyController eC = collision.gameObject.GetComponent<EnemyController>();
                    eC.HitByPoison(amountOfTicks, amountOfDps, hitByPoisonEffect);
                }
            }
        }
    }

    public void HitByPoison(float ticks, float dps, GameObject effect)
    {
        amountOfTicks = ticks;
        amountOfDps = dps;
        GameObject poisonEffect = Instantiate(effect, gameObject.transform);
        Destroy(poisonEffect, ticks);
        if (poisonTickTimers.Count <= 0)
        {
            poisonTickTimers.Add(ticks);
            StartCoroutine(PoisonTick(dps));
        }
    }

    private IEnumerator PoisonTick(float dps)
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

    public virtual void HitBySplash(float radius, float splashDamage)
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
