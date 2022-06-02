using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Tank/Weapon", order = 1)]
public class TankWeapon : ScriptableObject
{
    public float damage = 25;
    public float fireRate = 0.2f;
    public float spread = 20f;
    public float range = 20f;
    public float bulletSpeed = 35f;
    public int penetratonCount = 2;
    public bool penetrating = false;

    [Header("Bullet prefab: ")]
    public GameObject bulletPrefab;
}
