using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyScript : MonoBehaviour
{
    private float health = 50;

    private void Update()
    {
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            BulletBehavior bullet = other.gameObject.GetComponent<BulletBehavior>();
            health -= bullet.BulletDamage;

            Destroy(other.gameObject);
        }
    }
}
