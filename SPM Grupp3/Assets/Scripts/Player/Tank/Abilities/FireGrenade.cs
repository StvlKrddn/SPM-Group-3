using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGrenade : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private Outline outline;
    [SerializeField] private int damage;
    [SerializeField] private float timer;
    [SerializeField] private AudioClip dynamiteExplosionSound;
    [SerializeField] private AudioClip dynamitePlaceSound;


    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        sphereCollider = GetComponent<SphereCollider>();
		StartCoroutine(TooLate());
        EventHandler.InvokeEvent(new PlaySoundEvent("Dynamite Placed", dynamitePlaceSound));
    }

	public IEnumerator TooLate()
	{
		yield return new WaitForSeconds(timer);
		StartCoroutine(Detonate());
	}

	private IEnumerator Detonate()
    {
        outline.enabled = false;
        EventHandler.InvokeEvent(new PlaySoundEvent("Dynamite Explode", dynamiteExplosionSound));
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
