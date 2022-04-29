using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Transform target;
    public float shotSpeed = 1f;
    [SerializeField] private float shotDamage = 5000f;
    public GameObject hitEffect;
    [SerializeField] private float poisonTicks = 5;
    [SerializeField] private float poisonDamagePerTick = 25;

    [SerializeField] private float slowProc = 0.7f;
    [SerializeField] private float splashRadius = 1f;
    [SerializeField] private float splashDamage = 20f;
    [SerializeField] private GameObject ShotPrefab;
    private Tower tower;

    public float ShotDamage { get { return shotDamage; } set { shotDamage = value; } }
    public float SlowProc { get { return slowProc; } set { slowProc = value; } }
    public float SplashRadius { get { return splashRadius; } set { splashRadius = value; } }
    public float SplashDamage { get { return splashDamage; } set { splashDamage = value; } }
    public float PoisonTicks { get { return poisonTicks; } set { poisonTicks = value; } }
    public float PoisonDamagePerTick { get { return poisonDamagePerTick; } set { poisonDamagePerTick = value; } }

    private void Awake()
    {
        tower = gameObject.GetComponentInParent<Tower>();
    }
    public void Seek(Transform _target)
    {       
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = shotSpeed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        EnemyController enemyTarget = target.GetComponent<EnemyController>();
        GameObject effectInstance = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(effectInstance, 1f);

        TypeOfShot(enemyTarget);
        
/*        Destroy(target.gameObject);*/
        Destroy(gameObject);
    }

    void TypeOfShot(EnemyController enemyTarget)
    {
        switch (gameObject.tag)
        {
            case "PoisonTower":
                shotDamage = 0f;
                enemyTarget.HitByPoison(PoisonTicks, PoisonDamagePerTick);
                break;
            case "SlowTower":
                shotDamage = 0f;
                enemyTarget.HitBySlow(SlowProc, tower.range);
                break;
            case "MissileTower":
                enemyTarget.HitBySplash(SplashRadius, SplashDamage);
                enemyTarget.TakeDamage(ShotDamage);
                break;
            default:
                enemyTarget.TakeDamage(ShotDamage);
                break;
        }
            
    }
}
