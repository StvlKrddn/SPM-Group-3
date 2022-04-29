using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    [SerializeField] private float slowProc = 0.7f;

/*    [SerializeField] private GameObject shot;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject radius;*/
    public float SlowProc { get { return slowProc; } set { slowProc = value; } }
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
        shot.GetComponent<Renderer>().enabled = false;

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
        enemyTarget.HitBySlow(SlowProc, range);
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
