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
    public GameObject radius;
    protected GameManager gM;
    public GameObject onHitEffect;
    protected TowerUpgradeController towerUpgradeController;
    public GameObject tower;
    public GameObject towerPlacement;

    [SerializeField] private FadeBehaviour fadeBehaviour;

    [Header("BaseStats")]

    public float range = 15f;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] private float shotDamage = 5000f;
    public float cost = 150f;
    public float materialCost;
    protected List<GameObject> shots = new List<GameObject>();

    public float ShotDamage { get { return shotDamage; } set { shotDamage = value; } }

    protected Transform target;

    protected Shot bullet;



    public abstract void HitTarget(GameObject hit, GameObject hitEffect);
    public abstract void ShowUpgradeUI(Transform towerMenu);
    public abstract float UpgradeCostUpdate();

    // private void Awake()
    // {
    //     StartCoroutine(BuildEffect());
    // }

    // IEnumerator BuildEffect()
    // {
    //     GameObject buildEffect = transform.Find("BuildEffect").gameObject;
    //     BuildingEffect effect = buildEffect.GetComponentInChildren<BuildingEffect>();
    //     yield return effect.PlayEffect();
    //     buildEffect.SetActive(false);
    //     gameObject.SetActive(true);
    // }

    public void LevelUpTower()
    {
        towerUpgradeController = TowerUpgradeController.Instance;
        switch (towerUpgradeController.GetUpgradesPurchased())
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
        towerUpgradeController = TowerUpgradeController.Instance;
    }
    protected virtual void TowerLevel2()
    {
        gM = GameManager.Instance;
        towerUpgradeController = TowerUpgradeController.Instance;
    }
    protected virtual void TowerLevel3()
    {
        gM = GameManager.Instance;
        towerUpgradeController = TowerUpgradeController.Instance;
    }

    protected abstract void Level1(GameObject tower);
    protected abstract void Level2(GameObject tower);
    protected abstract void Level3(GameObject tower);

    public void ShowHover()
    {
        if (!fadeBehaviour.Faded())
        {
            print("Not faded");
            return;
        }

        fadeBehaviour.Fade();
    }
    public void HideHover()
    {
        if (fadeBehaviour.Faded())
        {
            print("Already faded");
            return;
        }

        fadeBehaviour.Fade();
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
            // Lock on target
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
  
        }
    }
}
