using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    private BoxCollider boxCollider;
    private void Awake() 
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update() 
    {
        Collider[] colliders = Physics.OverlapBox(
            center: transform.position,
            halfExtents: boxCollider.size / 2,
            Quaternion.identity,
            layerMask: layerMask
        );

        for (int i = 0; i < colliders.Length; i++)
            {
                Collider collision = colliders[i];
                if (collision == boxCollider) continue;
                bool penetration = Physics.ComputePenetration(
                    boxCollider,
                    transform.position,
                    transform.rotation,
                    collision,
                    collision.transform.position,
                    collision.transform.rotation,
                    out Vector3 direction,
                    out float distance
                    );
                if (penetration)
                {
                    collision.transform.position += direction * distance;
                }
            }
    }
}
