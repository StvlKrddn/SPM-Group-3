using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITankWeapon
{
    public void UpgradeFirerate(float modifier);
    public void UpgradeDamage(float modifier);
    public void UpgradeRange(float modifier);
}
