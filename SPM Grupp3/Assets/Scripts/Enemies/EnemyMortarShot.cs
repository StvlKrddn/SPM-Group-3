using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMortarShot : MonoBehaviour
{
    [SerializeField] private float speed = 20;
    private int phase = 1;
    private Transform target;
    public float damage;
    [SerializeField] private GameObject radius;
    private ParticleSystem[] particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            case 1:
            transform.Translate(speed * Time.deltaTime * Vector3.up, Space.World);
            break;

            case 2:
            transform.Translate(speed * Time.deltaTime * Vector3.down, Space.World);
            break;
        }
    }

	private void OnBecameInvisible()
	{
        if (phase == 1)
        {
            if (FindObjectOfType<TankState>())
            {
                TankState[] tanks;
                tanks = FindObjectsOfType<TankState>();
                foreach (TankState tank in tanks)
                {
                    if (target == null || Vector2.Distance(tank.transform.position, transform.position) < Vector3.Distance(target.position, transform.position))
                    {
                        target = tank.transform;
                    }
                }
                if (radius.transform.parent != null)
                {
                    Shot();
                }
                phase = 2;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void Shot()
    {
        radius.SetActive(true);
        transform.position = new Vector3(target.position.x, transform.position.y + 10, target.position.z);
        radius.transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
        if (radius.transform.parent != null)
        {
            radius.transform.parent = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (phase == 2)
        {
            if (collider.gameObject.CompareTag("PlaceForTower") || collider.gameObject.CompareTag("Road") || collider.gameObject.CompareTag("Tank"))
            {
                Destroy(radius);
                StartCoroutine(Particle());
            }
        }
        
    }

    private IEnumerator Particle()
    {
        phase = 3;
        GetComponent<MeshRenderer>().enabled = false;
        particle[0].transform.GetComponent<SphereCollider>().enabled = true;
        foreach (ParticleSystem p in particle)
        {
            p.Play();
        }
        yield return new WaitForSeconds(particle[0].main.duration + 0.1f);
        Destroy(gameObject);
    }
}
