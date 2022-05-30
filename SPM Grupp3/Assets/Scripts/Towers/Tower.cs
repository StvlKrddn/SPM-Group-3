using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Unity Setup Fields")]
    [SerializeField] private FadeBehaviour fadeBehaviour;
    [SerializeField] protected string enemyTag = "Enemy";
    [SerializeField] protected float turnSpeed = 10f;
    [SerializeField] protected GameObject shot;
    [SerializeField] protected Transform firePoint;
    protected TowerManager towerManager;
    protected GameManager gameManager;
    public GameObject Radius;
    public GameObject OnHitEffect;
    public GameObject TowerPlacement;

    [Header("BaseStats")]

    [SerializeField] private float shotDamage = 5000f;
    [SerializeField] protected float fireRate = 1f;
    public float range = 15f;
    public float cost = 150f;
    public float materialCost;

    protected List<GameObject> shots = new List<GameObject>();
    protected Transform target;
    protected Shot bullet;
    public float ShotDamage { get { return shotDamage; } set { shotDamage = value; } }

    public abstract void HitTarget(GameObject hit, GameObject hitEffect);
    public abstract void ShowUpgradeUI(Transform towerMenu);
    public abstract float UpgradeCostUpdate();

    public void LevelUpTower()
    {
        towerManager = TowerManager.Instance;
        switch (towerManager.GetUpgradesPurchased())
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
        gameManager = GameManager.Instance;
        towerManager = TowerManager.Instance;
    }
    protected virtual void TowerLevel2()
    {
        gameManager = GameManager.Instance;
        towerManager = TowerManager.Instance;
    }
    protected virtual void TowerLevel3()
    {
        gameManager = GameManager.Instance;
        towerManager = TowerManager.Instance;
    }

    protected abstract void Level1(GameObject tower);
    protected abstract void Level2(GameObject tower);
    protected abstract void Level3(GameObject tower);

    public void ShowHoverEffect()
    {
        fadeBehaviour.Hover();
    }
    public void HideHoverEffect()
    {
        fadeBehaviour.HideHover();
    }

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

    protected int FindShot()
    {
        for (int i = 0; i < shots.Count; i++)
        {
            if (shots[i].activeSelf == false)
            {
                return i;
            }
        }
        return -1;
    }

    protected IEnumerator DisableEffect(GameObject effect)
    {
        yield return new WaitForSeconds(1);
        effect.SetActive(false);
    }

    protected void LockOnTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);  
        }
    }
}
