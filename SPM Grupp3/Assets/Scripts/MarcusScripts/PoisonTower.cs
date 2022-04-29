using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : Tower
{
    [SerializeField] private float poisonTicks = 5;
    [SerializeField] private float poisonDamagePerTick = 25;

/*    [SerializeField] private GameObject shot;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject radius;*/
    public float PoisonTicks { get { return poisonTicks; } set { poisonTicks = value; } }
    public float PoisonDamagePerTick { get { return poisonDamagePerTick; } set { poisonDamagePerTick = value; } }
    // Start is called before the first frame update
    void Start()
    {
        radius.transform.localScale = new Vector3(range * 2f, 0.01f, range * 2f);
        radius.SetActive(false);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        LockOnTarget();

        if (CanYouShoot())
        {
            Shoot();
            EnemyController enemyTarget = target.gameObject.GetComponent<EnemyController>();

            if (bullet.CheckIfProjectileHit())
            {
                TypeOfShot(enemyTarget);
            }

        }
    }

    protected override void TypeOfShot(EnemyController enemyTarget)
    {
        enemyTarget.HitByPoison(PoisonTicks,PoisonDamagePerTick);
    }
    protected void Shoot()
    {
        GameObject bulletGO = Instantiate(shot, firePoint.position, firePoint.rotation);
        bulletGO.transform.parent = transform;
        bulletGO.SetActive(true);
        bullet = bulletGO.GetComponent<Shot>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
