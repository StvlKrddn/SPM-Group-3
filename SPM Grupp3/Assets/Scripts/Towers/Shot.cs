using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Tower tower;
    private Vector3 direction;
    private float distanceThisFrame;
    private EnemyController enemy;

    [Header("Speed of the bullet")]
    public float shotSpeed = 1f;
    public Transform target;

    public void Seek(Transform _target)
    {       
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if (target != null)
        {
            distanceThisFrame = shotSpeed * Time.deltaTime;
            direction = target.position - transform.position;

            transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        }

    }

    public Tower getTowerShotCameFrom()
    {
        return tower;
    }

    private void OnTriggerEnter(Collider other)
    {
        tower = gameObject.GetComponentInParent<Tower>();
        if (other.gameObject.CompareTag("Enemy"))
        {
            tower.HitTarget(other.gameObject, tower.OnHitEffect);
            gameObject.SetActive(false);
        }
    }


    // Under Muntan sa ni l�rare att jag borde flytta all kod �ver vad som h�nder n�r de olika tornen tr�ffar en fiende
    // Jag sj�lv anser att det s�ttet jag hade tidigare var b�ttre d� det var ett enklare system
    // Men jag har nu �ndrat s� att varje skott sj�lv best�mmer vilken effekt den ska ha.

    public void DecideTypeOfShot(string towerType)
    {
        enemy = target.GetComponent<EnemyController>();
        switch (towerType)
        {
            case "Cannon":
                CannonTower cannonTower = tower.GetComponent<CannonTower>();
                enemy.TakeDamage(cannonTower.ShotDamage);

                break;
            case "Missile":
                MissileTower missileTower = tower.GetComponent<MissileTower>();
                if (missileTower.ThirdShot && missileTower.ShotsFired % 3 == 0)
                {
                    GetComponent<SplashTowerEffect>().HitBySplash(missileTower.SplashRadius, missileTower.SplashDamage * 2);
                    enemy.TakeDamage(missileTower.ShotDamage * 2);
                }
                else
                {
                    GetComponent<SplashTowerEffect>().HitBySplash(missileTower.SplashRadius, missileTower.SplashDamage);
                    enemy.TakeDamage(missileTower.ShotDamage);
                }

                break;
            case "Slow":
                SlowTower slowTower = tower.GetComponent<SlowTower>();
                GetComponent<SlowTowerEffect>().HitBySlow(enemy, slowTower.SlowProc, slowTower.range, slowTower.AreaOfEffect, false);

                break;
            case "Poison":
                PoisonTower poisonTower = tower.GetComponent<PoisonTower>();
                GetComponent<PoisonTowerEffect>().HitByPoison(poisonTower.PoisonTicks, poisonTower.OnHitEffect, poisonTower.PoisonDamagePerTick, poisonTower.MaxHealthPerTick, poisonTower.range);

                break;
        }
    }
}
