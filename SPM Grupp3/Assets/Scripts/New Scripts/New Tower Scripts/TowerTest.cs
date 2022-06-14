using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BoxCollider), typeof(ActionHandler))]
public abstract class TowerTest : MonoBehaviour, IActionPerformer
{
    [Header("Setup")]
    [SerializeField] protected float turnSpeed = 10f;

    protected Transform currentTarget;
    protected BoxCollider boxCollider;
    protected bool targetInRange;
    protected bool allowedToPerformAction;
    protected ActionHandler actionHandler;
    protected float range;
    protected float actionFrequency;
    protected bool areaOfEffect;
    protected ActionType actionType;
    protected LayerMask affectedLayer;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        actionHandler = GetComponent<ActionHandler>();

        allowedToPerformAction = true;

    }

    void UpdateAction()
    {
        range = actionHandler.Range;
        actionFrequency = actionHandler.Frequency;
        areaOfEffect = actionHandler.AreaOfEffect;
        actionType = actionHandler.ActionType;
        affectedLayer = actionType.AffectedLayer;
    }

    void Update()
    {
        UpdateAction();

        StartCoroutine(FindTarget());

        if (targetInRange && allowedToPerformAction)
        {
            StartCoroutine(Weapon());
        }
    }

    private IEnumerator FindTarget()
    {
        Dictionary<Transform, Vector3> targets = new Dictionary<Transform, Vector3>();
        Transform closestTarget = null;
        float minDistance = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(transform.position, range, affectedLayer);
        if (colliders.Length == 0)
        {
            currentTarget = null;
            targetInRange = false;
        }
        else
        {
            targetInRange = true;
            foreach (Collider targetCollider in colliders)
            {
                if (targetCollider == null || targetCollider.gameObject.Equals(gameObject))
                {
                    continue;
                }

                if (areaOfEffect)
                {
                    Debug.DrawLine(transform.position, targetCollider.transform.position, Color.red);
                }
                else
                {
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
                }
                yield return null;
            }

            if (targetInRange && !areaOfEffect)
            {
                Vector3 targetDirection = targets[closestTarget];
                currentTarget = closestTarget;
                if (closestTarget != null) Debug.DrawLine(transform.position, closestTarget.position, Color.red);
                LockOnTarget(targetDirection);
            }
        }

        void LockOnTarget(Vector3 direction)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    private void Shoot()
    {
        if (areaOfEffect)
        {
            AreaOfEffect();
        }
        else
        {
            SingleTarget();
        }

        void AreaOfEffect()
        {
            Collider[] targetsInRange = Physics.OverlapSphere(
                position: transform.position,
                radius: range,
                layerMask: affectedLayer
            );
            foreach (Collider collider in targetsInRange)
            {
                if (collider.GetComponent<DamageHandler>())
                {
                    IDamageable target = collider.GetComponent<DamageHandler>();
                    HitTarget(target);
                }
            }
        }

        void SingleTarget()
        {
            print("Shooting at a single target");
            bool hit = Physics.Raycast(
                origin: transform.position,
                direction: transform.forward,
                hitInfo: out RaycastHit raycastHit,
                maxDistance: range,
                layerMask: affectedLayer
            );

            if (hit && raycastHit.collider.GetComponent<DamageHandler>())
            {
                IDamageable target = raycastHit.collider.GetComponent<DamageHandler>();
                HitTarget(target);
            }
        }
    }

    public void HitTarget(IDamageable target)
    {
        target.TakeHit(actionType as DamageType);
    }

    public void ApplyBuff(IBuffable target)
    {
        target.ApplyBuff(actionType as BuffType);
    }

    private IEnumerator Weapon()
    {
        Shoot();
        allowedToPerformAction = false;
        yield return new WaitForSeconds(1 / actionFrequency);
        allowedToPerformAction = true;
    }

    public void AddActionType<A>() where A : ActionType
    {

    }

    public void RemoveActionType<A>() where A : ActionType
    {

    }
}
