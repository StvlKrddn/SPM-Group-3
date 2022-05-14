using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankExplosion : MonoBehaviour
{
    [SerializeField] private float explosionForce;
    [SerializeField] private int explosionRadius;
    [SerializeField] private Transform explosionPoint;

    private ParticleSystem system;
    private Rigidbody[] rigidBodies;

    private void Start() 
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.08f);
        rigidBodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidBodies)
        {
            rb.AddExplosionForce(
                explosionForce: explosionForce * 100,
                explosionPosition: explosionPoint.position,
                explosionRadius: explosionRadius
                );
        }
    }
}
