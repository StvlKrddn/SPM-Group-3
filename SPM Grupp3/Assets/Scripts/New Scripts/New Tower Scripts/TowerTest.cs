using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BoxCollider))]
public abstract class TowerTest : MonoBehaviour, IActionPerformer
{
    [Header("Setup")]
    [Tooltip("How fast does the Tower turn towards their target")]
    [SerializeField] protected float turnSpeed = 10f;

    [Tooltip("What layer should the Tower target")]
    [SerializeField] protected LayerMask affectedLayer;

    [Header("Stats")]
    [Tooltip("The spherical range of the Tower")]
    [SerializeField] protected float range;

    [Tooltip("What kind of actions will this Tower perform")]
    [SerializeField] protected List<ActionType> actionTypes;

    [Tooltip("Is this an area of effect or single target Tower")]
    [SerializeField] private bool areaOfEffect;

    [Tooltip("How often does the Tower perform its action every second")]
    [SerializeField] private float actionFrequency;

    protected Transform currentTarget;
    protected BoxCollider boxCollider;
    protected bool targetInRange;
    protected bool allowedToPerformAction;

    public List<ActionType> ActionTypes => actionTypes;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();

        allowedToPerformAction = true;

        if (actionTypes.Count == 0)
        {
            ActionType actionType = Resources.Load<NormalDamage>("DamageTypes/CannonTower");
            actionTypes.Add(actionType);
        }
    }

    void Update()
    {
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

            if (closestTarget != null && targetInRange && !areaOfEffect)
            {
                Vector3 targetDirection = targets[closestTarget];
                currentTarget = closestTarget;
                Debug.DrawLine(transform.position, closestTarget.position, Color.red);
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
        target.TakeHit(actionTypes);
    }

    public void ApplyBuff(IBuffable target)
    {
        target.ApplyBuff(actionTypes);
    }

    private IEnumerator Weapon()
    {
        Shoot();
        allowedToPerformAction = false;
        yield return new WaitForSeconds(1 / actionFrequency);
        allowedToPerformAction = true;
    }

    public void AddActionType(ActionType actionType)
    {

    }

    public void RemoveActionType(ActionType actionType)
    {

    }
}
