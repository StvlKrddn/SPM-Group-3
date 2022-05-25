using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Unity Setup Fields")]

    [SerializeField] protected string enemyTag = "Enemy";
    [SerializeField] protected float turnSpeed = 10f;
    [SerializeField] protected GameObject shot;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject radius;
    protected GameManager gM;
    public GameObject onHitEffect;
    protected TowerUpgradeController tUC;
    public GameObject tower;
    public Tower towerScript;
    public GameObject upgradeUI;
    public GameObject buildUI;
	public GameObject towerPlacement;
    public Tower towerScript;

    [Header("BaseStats")]

    public float range = 15f;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] private float shotDamage = 5000f;
    public float cost = 150f;
    public float materialCost;

    public float ShotDamage { get { return shotDamage; } set { shotDamage = value; } }

    protected Transform target;

    protected Shot bullet;

    public abstract void HitTarget(TowerHitEvent eventInfo);
    public abstract void ShowUpgradeUI(Transform towerMenu);
    public abstract float UpgradeCostUpdate();

    public void LevelUpTower()
    {
        tUC = TowerUpgradeController.Instance;
        switch (tUC.GetUpgradesPurchased())
        {
            case 0:
                TowerLevel1();
                break;
            case 1:
                TowerLevel2();
                break;
            case 2:
                TowerLevel3();
                break;
        }
    }

    public void LoadTowerLevel(PlacedTower tower)
    {
        for (int i = 0; i < tower.upgradesPurchased; i++)
        {
            switch(i)
            {
                case 0:
                    Level1(tower.tower);
                    break;
                case 1:
                    Level2(tower.tower);
                    break;
                case 2:
                    Level3(tower.tower);
                    break;
            }
        }
    }

    protected virtual void TowerLevel1()
    {
        gM = GameManager.Instance;
        tUC = TowerUpgradeController.Instance;
    }
    protected virtual void TowerLevel2()
    {
        gM = GameManager.Instance;
        tUC = TowerUpgradeController.Instance;
    }
    protected virtual void TowerLevel3()
    {
        gM = GameManager.Instance;
        tUC = TowerUpgradeController.Instance;
    }

    protected abstract void Level1(GameObject tower);
    protected abstract void Level2(GameObject tower);
    protected abstract void Level3(GameObject tower);

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float closestEnemy = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestEnemy)
            {
                closestEnemy = distanceToEnemy; 
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && closestEnemy <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
    
    protected void LockOnTarget()
    {
        if (target != null)
        {
            // Lock on target
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
  
        }
    }
}
