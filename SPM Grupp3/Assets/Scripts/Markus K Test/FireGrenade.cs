using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGrenade : MonoBehaviour
{
    [SerializeField] private int damage;
    private SphereCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<SphereCollider>();
    }

	public void OnCollisionEnter(Collision collision)
	{
        if (collision.collider.tag == "Enemy")
        {
	        StartCoroutine(Detonate());
        }
	}

	private IEnumerator Detonate()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, collider.radius);
        foreach (Collider c in enemies)
        {
            if (c.GetComponent<EnemyController>())
            {
                c.GetComponent<EnemyController>().TakeDamage(damage);
            }
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
