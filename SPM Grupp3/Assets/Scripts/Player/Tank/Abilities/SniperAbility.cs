using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAbility : BulletBehavior
{
    [SerializeField] private float damageOfAbility;
    [SerializeField] private GameObject startObject;
    [SerializeField] private GameObject hitObject;
    private BoxCollider boxCollider;
    private ParticleSystem hitParticle;
    private ParticleSystem startParticle;

    protected override void Start()
	{
		base.Start();
	    hitParticle = hitObject.GetComponent<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider>();
        startParticle = startObject.GetComponent<ParticleSystem>();
	}

	protected override void OnBecameInvisible()
	{
		Destroy(gameObject, 0.01f);
	}

	protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            boxCollider.enabled = false;
            GetComponentInChildren<MeshRenderer>().gameObject.SetActive(false);
            startParticle.Stop();

            EnemyController target = other.GetComponent<EnemyController>();
            EnemyController[] enemyControllers = FindObjectsOfType(typeof(EnemyController)) as EnemyController[];
            foreach (EnemyController enemy in enemyControllers)
            {
                if (target.GetType() == enemy.GetType())
                {
                    enemy.TakeDamage(damageOfAbility);
                }
            }
            hitObject.SetActive(true);
            hitObject.transform.parent = null;
            hitObject.SetActive(true);
            Destroy(gameObject, hitParticle.main.duration + 0.01f);
        }
    }
}
