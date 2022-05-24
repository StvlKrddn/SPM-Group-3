using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankExplosion : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;
    [SerializeField] float explosionForce;
    [SerializeField] Transform explosionOrigin;
    [SerializeField] float explosionRadius;

    void Start()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Rigidbody>())
            {
                child.GetComponent<Rigidbody>().AddExplosionForce(
                    explosionForce: explosionForce,
                    explosionPosition: explosionOrigin.position,
                    explosionRadius: explosionRadius
                );
            }
        }

        Destroy(gameObject, 3);
    }
}
