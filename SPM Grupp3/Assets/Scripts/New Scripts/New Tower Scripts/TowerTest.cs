using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BoxCollider))]
public abstract class TowerTest : MonoBehaviour, IDamageDealer
{
    [Header("Setup")]
    [SerializeField] protected float turnSpeed = 10f;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected bool isAreaOfAffect;

    [Header("Stats")]
    [SerializeField] protected float range = 10;
    [SerializeField] protected float fireRate = 1;
    [SerializeField] private List<DamageType> damageTypes = new List<DamageType>();

    protected Dictionary<string, DamageType> currentDamageTypes = new Dictionary<string, DamageType>();
    protected Transform currentTarget;
    protected BoxCollider boxCollider;
    protected bool targetInRange;
    protected bool allowedToFire;

    public Dictionary<string, DamageType> DamageTypes { get => currentDamageTypes; }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (damageTypes.Count == 0)
        {
            DamageType normalDamage = Resources.Load<DamageType>(nameof(NormalDamage));
            damageTypes.Add(normalDamage);
        }
        foreach (DamageType damageType in damageTypes)
        {
            currentDamageTypes.Add(nameof(damageType), damageType);
        }
        allowedToFire = true;

        AddDamageType<NormalDamage>();
    }

    void Update()
    {
        StartCoroutine(FindTarget());

        if (targetInRange && allowedToFire)
        {
            StartCoroutine(Weapon());
        }
    }

    private IEnumerator FindTarget()
    {
        Dictionary<Transform, Vector3> targets = new Dictionary<Transform, Vector3>();
        Transform closestTarget = null;
        float minDistance = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(transform.position, range, enemyLayer);
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

                if (isAreaOfAffect)
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

            if (targetInRange && !isAreaOfAffect)
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
        if (isAreaOfAffect)
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
                enemyLayer
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
                layerMask: enemyLayer
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
        foreach (DamageType damageType in currentDamageTypes.Values)
        {
            target.TakeHit(damageType);
        }
    }

    private IEnumerator Weapon()
    {
        Shoot();
        allowedToFire = false;
        yield return new WaitForSeconds(1 / fireRate);
        allowedToFire = true;
    }

    public void AddDamageType<D>() where D : DamageType
    {
        string damageTypeName = typeof(D).ToString();
        DamageType newDamageType = Resources.Load<DamageType>(damageTypeName);
        currentDamageTypes.Add(damageTypeName, newDamageType);
    }

}
