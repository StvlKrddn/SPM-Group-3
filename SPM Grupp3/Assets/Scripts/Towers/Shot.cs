using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [Header("Speed of the bullet")]
    public float shotSpeed = 1f;

    public Transform target;
    private Tower tower;
    private Vector3 direction;
    private float distanceThisFrame;

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
            EventHandler.Instance.InvokeEvent(new TowerHitEvent(
                    description: "An enemy hit",
                    towerGO: tower.gameObject,
                    hitEffect: tower.onHitEffect,
                    enemyHit: target.gameObject
                    )) ;
            Destroy(gameObject);
        }
    }


    // Under Muntan sa ni lärare att jag borde flytta all kod över vad som händer när de olika tornen träffar en fiende
    // Jag själv anser att det sättet jag hade tidigare var bättre då det var ett enklare system
    // Men jag har nu ändrat så att varje skott själv bestämmer vilken effekt den ska ha.
    public void DecideTypeOfShot(string towerType)
    {
        EnemyController enemy = target.GetComponent<EnemyController>();
        switch (towerType)
        {
            case "Cannon":
                CannonTower cT = tower.GetComponent<CannonTower>();
                enemy.TakeDamage(cT.ShotDamage);

                break;
            case "Missile":
                MissileTower mT = tower.GetComponent<MissileTower>();
                if (mT.thirdShot && mT.ShotsFired % 3 == 0)
                {
                    GetComponent<SplashTowerEffect>().HitBySplash(mT.SplashRadius, mT.SplashDamage * 2);
                    enemy.TakeDamage(mT.ShotDamage * 2);
                }
                else
                {
                    GetComponent<SplashTowerEffect>().HitBySplash(mT.SplashRadius, mT.SplashDamage);
                    enemy.TakeDamage(mT.ShotDamage);
                }

                break;
            case "Slow":

                SlowTower sT = tower.GetComponent<SlowTower>();
                GetComponent<SlowTowerEffect>().HitBySlow(sT.SlowProc, sT.range, sT.AreaOfEffect, false);

                break;
            case "Poison":
                PoisonTower pT = tower.GetComponent<PoisonTower>();
                GetComponent<PoisonTowerEffect>().HitByPoison(pT.PoisonTicks, pT.onHitEffect, pT.PoisonDamagePerTick, pT.MaxHealthPerTick, pT.range);

                break;
        }


    }
}
