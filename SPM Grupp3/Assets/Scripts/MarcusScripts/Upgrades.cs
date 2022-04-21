using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [SerializeField] private float upgradeDamageAmount;
    [SerializeField] private float upgradeRangeAmount;
    [SerializeField] private float upgradeFireRateAmount;
    [SerializeField] private float upgradePoisonTickDamageAmount;
    [SerializeField] private float upgradePoisonTicksAmount;
    [SerializeField] private float upgradeSlowProcAmount;
    [SerializeField] private float upgradeSplashDamageAmount;
    [SerializeField] private float upgradeSplashRadiusAmount;

    private GameObject placedTower;
    private Tower tower;
    private Shot shot;
    private BuildManager buildManager;

    // Start is called before the first frame update
    void Awake()
    {
        buildManager = BuildManager.instance;
        placedTower = buildManager.placedTower;
        tower = placedTower.GetComponent<Tower>();
        shot = tower.transform.GetChild(0).GetComponent<Shot>();
        gameObject.SetActive(false);
    }

    public void UpgradeDamage()
    {
        shot.ShotDamage += upgradeDamageAmount;
    }
    public void UpgradeRange()
    {
        tower.range += upgradeRangeAmount;
    }

    public void UpgradeFireRate()
    {
        tower.fireRate += upgradeFireRateAmount;
    }
    public void UpgradeDPS()
    {
        shot.PoisonDamagePerTick += upgradePoisonTickDamageAmount;
    }
    public void UpgradeSlowProc()
    {
        shot.SlowProc += upgradeSlowProcAmount;
    }
    public void UpgradeSplashDamage()
    {
        shot.SplashDamage += upgradeSplashDamageAmount;
    }
    public void UpgradeSpashRadius()
    {
        shot.SplashRadius += upgradeSplashRadiusAmount;
    }
}
