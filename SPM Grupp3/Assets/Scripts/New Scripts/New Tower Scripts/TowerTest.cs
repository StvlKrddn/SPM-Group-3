using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BoxCollider))]
public class TowerTest : Upgradable
{
    [Header("Setup")]
    [SerializeField] protected float turnSpeed = 10f;
    [SerializeField] protected LayerMask enemyMask;

    [Header("Stats")]
    [SerializeField] protected float range;

    protected string enemyTag = "Enemy";
    protected Transform target;
    protected BoxCollider boxCollider;
    protected bool isTargeting;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FindTarget());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private IEnumerator FindTarget()
    {
        Dictionary<Transform, Vector3> targets = new Dictionary<Transform, Vector3>();
        Transform closestTarget = null;
        float minDistance = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(transform.position, range, enemyMask);
        foreach (Collider targetCollider in colliders)
        {
            if (targetCollider.gameObject.Equals(gameObject))
            {
                continue;
            }
            isTargeting = true;

            Transform target = targetCollider.transform;
            float distance = Vector3.Distance(transform.position, target.position);
            Vector3 direction = target.position - transform.position;

            Debug.DrawLine(transform.position, target.position, Color.blue);

            targets.Add(target, direction);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestTarget = target;
            }
            yield return null;
        }

        if (isTargeting)
        {
            Vector3 targetDirection = targets[closestTarget];
            Debug.DrawLine(transform.position, closestTarget.position, Color.red);
            LockOnTarget(targetDirection);
        }
    }

    private void LockOnTarget(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
