using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnemy : EnemyController
{
    public override void TakeDamage(float damage)
    {
        print(damage);
    }

    protected override void Update(){}
}
