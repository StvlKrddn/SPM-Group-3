using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour, IDamageable
{
    private LayerMask targetLayerMask;
    [SerializeField] private float health = 100;
    public float Health { get => health; set => health = value; }

    public void TakeHit(DamageType damageType)
    {
        if (damageType is NormalDamage)
        {
            HitByNormalDamage();
        }
        if (damageType is SplashDamage)
        {
            HitBySplashDamage();
        }
        if (damageType is StatusEffectDamage)
        {
            //ApplyStatusEffect();
        }

        void HitByNormalDamage()
        {
            print("Normal damage taken");
            NormalDamage normalDamage = damageType as NormalDamage;
            TakeDamage(normalDamage.GetDamage);
        }

        void HitBySplashDamage()
        {
            print("Splash damage taken!");
            SplashDamage splashDamage = damageType as SplashDamage;

            Collider[] targetsInRange = Physics.OverlapSphere(
                position: transform.position,
                radius: splashDamage.GetSplashRadius,
                layerMask: 1 << gameObject.layer
            );

            foreach (Collider collider in targetsInRange)
            {
                //print(collider.name);
                if (collider.GetComponent<DamageHandler>())
                {
                    DamageHandler target = collider.GetComponent<DamageHandler>();
                    target.TakeDamage(splashDamage.GetSplashDamage);
                }
            }
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            print("Enemy killed!");
            Destroy(gameObject);
        }
        print(gameObject.name + " health: " + health);
    }
}
