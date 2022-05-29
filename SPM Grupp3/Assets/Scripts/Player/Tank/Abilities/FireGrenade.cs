using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGrenade : MonoBehaviour
{
    [SerializeField] private int damage;
    private SphereCollider sphereCollider;
    [SerializeField] private float timer;


    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
		StartCoroutine(TooLate());
    }

    /*
	public void OnCollisionEnter(Collision collision)
	{
        if (collision.collider.tag == "Enemy")
        {
	      //  StartCoroutine(Detonate());
        }
	}
    */

	public IEnumerator TooLate()
	{
		yield return new WaitForSeconds(timer);
		StartCoroutine(Detonate());
	}

	private IEnumerator Detonate()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, sphereCollider.radius);
        foreach (Collider c in enemies)
        {
            if (c.GetComponent<EnemyController>())
            {
                c.GetComponent<EnemyController>().HitByFire(damage);
            }
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}