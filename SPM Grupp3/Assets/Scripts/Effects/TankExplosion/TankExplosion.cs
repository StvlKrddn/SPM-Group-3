using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankExplosion : MonoBehaviour
{
    [SerializeField] private float explosionForce;
    [SerializeField] private int explosionRadius;
    [SerializeField] private Transform explosionPoint;

    public Color TankColor;

    private ParticleSystem system;
    private Rigidbody[] rigidBodies;

    private void Start() 
    {
        Renderer tank = transform.Find("TankBody").GetComponent<Renderer>();
        tank.material.color = TankColor;

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
