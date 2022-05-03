using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades
{
    [SerializeField] private float upgradeDamageAmount;
    [SerializeField] private float upgradeRangeAmount;
    [SerializeField] private float upgradeFireRateAmount;
    [SerializeField] private float upgradePoisonTickDamageAmount;
    [SerializeField] private float upgradePoisonTicksAmount;
    [SerializeField] private float upgradeSlowProcAmount;
    [SerializeField] private float upgradeSplashDamageAmount;
    [SerializeField] private float upgradeSplashRadiusAmount;

    private Shot shoot;
    private Vector3 radiusUpdate;

    // Start is called before the first frame update
/*    void Awake()
    {
        shoot = shot.GetComponent<Shot>();
        radiusUpdate = new Vector3(upgradeRangeAmount, 0, upgradeRangeAmount);
    }

    public void UpgradeDamage()
    {
        ShotDamage += upgradeDamageAmount;
    }
    public void UpgradeRange()
    {
        range += upgradeRangeAmount;
        radius.transform.localScale += radiusUpdate;
    }

    public void UpgradeFireRate()
    {
        fireRate += upgradeFireRateAmount;
    }
    public void UpgradeDPS()
    {
        PoisonDamagePerTick += upgradePoisonTickDamageAmount;
    }
    public void UpgradeSlowProc()
    {
        SlowProc += upgradeSlowProcAmount;
    }
    public void UpgradeSplashDamage()
    {
        SplashDamage += upgradeSplashDamageAmount;
    }
    public void UpgradeSpashRadius()
    {
        SplashRadius += upgradeSplashRadiusAmount;
    }*/
}
