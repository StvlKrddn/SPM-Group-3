using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour
{
    private GameManager gameManager;
    private HealthBar healthBar;
    private float defaultSpeed;
    private float currentHealth;
    private List<float> poisonTickTimers = new List<float>();
    private bool dead = false;
    private MaterialHolder materialHolder;
    private Color moneyColor = new Color(255, 100, 0, 255);
    private float defaultAnimationSpeed;
    private PoisonTowerEffect poisonTowerEffect;
    [SerializeField] private float health = 100f;
    [SerializeField] private float meleeDamage;
    [SerializeField] private Transform spawnTextPosition;
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioClip enemyDeathSound;

    protected Transform target;
    protected int currentWaypointIndex = 0;
    protected List<Transform[]> wayPoints;

    public float Speed = 10f;
    public GameObject DeathEffect;
    public GameObject gainText;
    public int DamageBase = 10;
    public int MoneyDrop = 10;
    public int Path;
    public bool MaterialDrop = false;
    public bool Spread = false;
  
    public List<float> PoisonTickTimers { get { return poisonTickTimers; } set { poisonTickTimers = value; } }
    public float DefaultSpeed {  get { return defaultSpeed; } set { defaultSpeed = value; } }
    public float MeleeDamage { get { return meleeDamage; } set { meleeDamage = value; } }
    public float Health { get { return health; } }

	// Start is called before the first frame update

	protected virtual void Awake() 
    {
        defaultSpeed = Speed;
        currentHealth = health;
        gameManager = GameManager.Instance;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.slider.maxValue = health;
        healthBar.slider.value = health;
        if (animator != null)
        {
            defaultAnimationSpeed = animator.speed;
        }

        wayPoints = Waypoints.instance.GetWaypoints();
        materialHolder = FindObjectOfType<MaterialHolder>();

        gainText.GetComponentInChildren<Text>().text = MoneyDrop.ToString();
        gainText.GetComponentInChildren<Text>().color = moneyColor;

        //Set base values. Gets changertext, movement, health and healthbar, speed and waypoints/path
    }
    protected virtual void Start() {}

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        MoveStep();   
    }

	protected virtual void OnEnable()
	{
        currentHealth = health;
        currentWaypointIndex = 0;
        poisonTickTimers.Clear();
        dead = false;
        healthBar.slider.maxValue = health;
        healthBar.slider.value = health;
        Path = Waypoints.instance.GivePath();
        target = wayPoints[Path][currentWaypointIndex];
    }

	protected virtual void OnDestroy()
	{
        
	}


	public void MoveStep()
    {
        //WIP Enemy moves right direction
        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        transform.position += Speed * Time.deltaTime * direction;
        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            if (wayPoints[Path].Length - 1 <= currentWaypointIndex) // Changes waypoint to til the enemy reaches the last waypoint
            {
                EnemyDeathBase();
                return;
            }
            currentWaypointIndex++;
            target = wayPoints[Path][currentWaypointIndex];
            transform.LookAt(target);
        }
    }

    private void EnemyDeathBase()
    {
        gameManager.TakeDamage(DamageBase, gameObject);
        DieEvent dieEvent = new DieEvent("dead of base", gameObject, null, null);
        EventHandler.InvokeEvent(dieEvent);
        gameObject.SetActive(false);
    }

    public void EnemyDeath()
    {
        gameManager.AddMoney(MoneyDrop); // add money and spawn material
        
        // Spawns a changerText for indikation for gaining money

        if(gainText != null && spawnTextPosition != null)
        {
            Instantiate(gainText, spawnTextPosition.position, spawnTextPosition.rotation, GameManager.Instance.transform.Find("DropTexts"));
        }

        if (MaterialDrop == true)
        {
			materialHolder.GiveMaterial(transform.position, transform.rotation);
        }
        //Deathevent to wavemanager
        DieEvent dieEvent = new DieEvent("dead", gameObject, null, null);
        EventHandler.InvokeEvent(dieEvent);
        Destroy(Instantiate(DeathEffect, transform.position, Quaternion.identity), 1f);
        gameObject.SetActive(false);

        EventHandler.InvokeEvent(new PlaySoundEvent("EnemyDied", enemyDeathSound));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerShots"))
        {
            BulletBehavior playerBullet = other.GetComponent<BulletBehavior>();
            TakeDamage(playerBullet.BulletDamage);
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Flamethrower"))
        {
            HitByFire(Flamethrower.FireDamage * Time.fixedDeltaTime);
        }
    }
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.HandleHealthChanged(currentHealth);
        if (currentHealth <= 0 && dead == false)
        {
            dead = true;
            EnemyDeath();
        }
    }

    public virtual void HitByFire(float damage)
	{
        //Used for specific enemy
		TakeDamage(damage);
	}

    public void HitBySlow()
    {
        if (animator != null)
        {
            animator.speed = Speed / DefaultSpeed; 
        }
    }

    public void ResetAnimator()
    {
        if (animator != null)
        {
            animator.speed = defaultAnimationSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Spread)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (GetComponent<EnemyController>().PoisonTickTimers.Count != 0)
                {
                    PoisonTower poisonTower = poisonTowerEffect.poisonTower.GetComponent<PoisonTower>();
                    poisonTowerEffect.HitByPoison(poisonTower.PoisonTicks, poisonTower.OnHitEffect, poisonTower.PoisonDamagePerTick, poisonTower.MaxHealthPerTick, 0);
                }
            }
        }
        
    }
}
