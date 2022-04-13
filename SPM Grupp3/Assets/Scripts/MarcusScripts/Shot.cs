using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Transform target;
    public float shotSpeed = 1f;
    [SerializeField] private float shotDamage = 50f;
    public GameObject hitEffect;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = shotSpeed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        EnemyController enemyTarget = target.GetComponent<EnemyController>();
        GameObject effectInstance = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(effectInstance, 1f);

        //enemyTarget.TakeDamage(shotDamage);
        Destroy(target.gameObject);
        Destroy(gameObject);
    }
}