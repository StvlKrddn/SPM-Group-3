using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowEffect", menuName = "StatusEffect/SlowEffect", order = 2)]
public class SlowEffect : StatusEffect
{
    [SerializeField] private float slowAmount;

    private float effectTimer;
    private DamageHandler target;

    public void UpgradeSlowAmount(float amount, bool isPercentual)
    {
        slowAmount = base.UpgradeValue(slowAmount, amount, isPercentual);
    }

    public override void Applied(GameObject target)
    {
        effectTimer = 0f;
        this.target = target.GetComponent<DamageHandler>();

        // TODO: Slow down target movement speed
    }

    public override void UpdateEffect()
    {
        if (effectTimer < effectDuration)
        {
            effectTimer += Time.deltaTime;
        }
        else
        {
            target.RemoveStatusEffect(this);
        }
    }

    public override void Removed()
    {
        // TODO: Return to normal speed
    }

    public float GetSlowAmount { get { return slowAmount; } }
}
