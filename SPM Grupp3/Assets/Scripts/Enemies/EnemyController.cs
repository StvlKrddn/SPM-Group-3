using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour
{
    public float speed = 10f;
    [SerializeField] private float health = 100f;
    public GameObject deathEffect;
    [SerializeField] private float meleeDamage;
    private GameManager gameManager;
    private Transform target;
    private HealthBar healthBar;
    protected int currWaypointIndex = 0;
    public int damageBase = 10;
    public int moneyDrop = 10;
    public bool materialDrop = false;

    private float defaultSpeed;
    private List<float> poisonTickTimers = new List<float>();
    public bool spread = false;
    private bool dead = false;
    private float currentHealth;
    public int path;
    protected List<Transform[]> wayPoints;
    private MaterialHolder materialHolder;

    private Color moneyColor = new Color(255, 100, 0, 255);
    public GameObject changerText;
    [SerializeField] private Transform spawnTextPosition;

    public List<float> PoisonTickTimers { get { return poisonTickTimers; } set { poisonTickTimers = value; } }
    public float DefaultSpeed {  get { return defaultSpeed; } set { defaultSpeed = value; } }
    public float MeleeDamage { get { return meleeDamage; } set { meleeDamage = value; } }
    public float Health { get { return health; } }

	// Start is called before the first frame update
	protected virtual void OnEnable()
	{
        
        currentHealth = health;
        currWaypointIndex = 0;
        poisonTickTimers.Clear();
        dead = false;
        healthBar.slider.maxValue = health;
        healthBar.slider.value = health;
        path = Waypoints.instance.GivePath();
        target = wayPoints[path][currWaypointIndex];
    }

	protected virtual void OnDestroy()
	{
        Destroy(changerText);
	}

	protected virtual void Awake() 
    {
        defaultSpeed = speed;
        currentHealth = health;
        gameManager = GameManager.Instance;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.slider.maxValue = health;
        healthBar.slider.value = health;
        wayPoints = Waypoints.instance.GetWaypoints();
        materialHolder = FindObjectOfType<MaterialHolder>();
        changerText.GetComponentInChildren<Text>().text = moneyDrop.ToString();
        changerText.GetComponentInChildren<Text>().color = moneyColor;
        changerText = Instantiate(changerText, spawnTextPosition.position, spawnTextPosition.rotation, GameManager.Instance.transform.Find("DropTexts"));
        changerText.SetActive(false);
        //Change to right tank when done with tanks
    }

    protected virtual void Start() {}

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        MoveStep();   
    }

	public void MoveStep()
    {
        //WIP Enemy moves right direction
        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        transform.position += speed * direction * Time.deltaTime;
    }

    private void EnemyDeathBase()
    {
        gameManager.TakeDamage(damageBase, gameObject);
        DieEvent dieEvent = new DieEvent("död från bas", gameObject, null, null);
        EventHandler.InvokeEvent(dieEvent);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waypoint"))
        {
            if (wayPoints[path].Length - 1 <= currWaypointIndex) // Changes waypoint to til the enemy reaches the last waypoint
            {
                EnemyDeathBase();
                return;
            }
            currWaypointIndex++;
            target = wayPoints[path][currWaypointIndex];
            transform.LookAt(target);
        }

        if (other.gameObject.CompareTag("PlayerShots"))
        {
            BulletBehavior playerBullet = other.GetComponent<BulletBehavior>();
            TakeDamage(playerBullet.BulletDamage);
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

    public void EnemyDeath()
    {
        gameManager.AddMoney(moneyDrop); // add money and spawn material
        
        // Spawns a changerText for indikation for gaining money

        if(changerText != null && spawnTextPosition != null)
        {
            changerText.transform.position = spawnTextPosition.position;
            changerText.transform.rotation = spawnTextPosition.rotation;
            changerText.SetActive(true);
        }

        if (materialDrop == true)
        {
			materialHolder.GiveMaterial(transform.position, transform.rotation);
        }
        DieEvent dieEvent = new DieEvent("d�d", gameObject, null, null);
        EventHandler.InvokeEvent(dieEvent);
        Destroy(Instantiate(deathEffect, transform.position, Quaternion.identity), 1f);
        gameObject.SetActive(false);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Flamethrower"))
        {
            HitByFire(Flamethrower.FireDamage * Time.fixedDeltaTime);
        }
    }

    public virtual void HitByFire(float damage)
	{
		TakeDamage(damage);
	}
}
