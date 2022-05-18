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
    private Health healthBar;
    protected int currWaypointIndex = 0;
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
    protected int path;

    public float MeleeDamage { get { return meleeDamage; } set { meleeDamage = value; } }
    public float Health { get { return health; } }

    // Start is called before the first frame update

    protected virtual void Awake()
    {
        defaultSpeed = speed;
        currentHealth = health;
        gM = GameManager.Instance;
        healthBar = GetComponent<Health>();
        //Change to right tank when done with tanks
    }

    public void TakePath(int path)
    {
        this.path = path;
        target = Waypoints.wayPoints[path][currWaypointIndex];
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
        direction.Normalize();
        transform.position += speed * Time.deltaTime * direction;
    }

    private void EnemyDeathBase()
    {
        gM.TakeDamage(damageBase, gameObject);
        DieEvent dieEvent = new DieEvent("död från bas", gameObject, null, null);
        EventHandler.Instance.InvokeEvent(dieEvent);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waypoint"))
        {
            if (Waypoints.wayPoints[path].Length - 1 <= currWaypointIndex) // Changes waypoint to til the enemy reaches the last waypoint
            {
                EnemyDeathBase();
                return;
            }
            currWaypointIndex++;
            target = Waypoints.wayPoints[path][currWaypointIndex];
            transform.LookAt(target);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.ModifyHealth(damage);
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

    public void HitBySlow(float slowProc, float radius, bool areaOfEffect)
    {
        if (!areaOfEffect)
        {
            speed *= slowProc;
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
                    eC.speed *= slowProc;
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
                    print("DamageTaken");
                    EnemyController eC = collision.gameObject.GetComponent<EnemyController>();
                    eC.HitByPoison(amountOfTicks, amountOfDps, hitByPoisonEffect);
                }
            }
        }
    }

	private void OnParticleCollision(GameObject other)
	{
        if (other.CompareTag("Flamethrower"))
        {
            HitByFire(Flamethrower.FireDamage * Time.fixedDeltaTime);
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

	public virtual void HitByFire(float damage)
	{
		TakeDamage(damage);
	}
}
