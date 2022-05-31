using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMortarShot : MonoBehaviour
{
    [SerializeField] private float speed = 20;
    private int phase = 1;
    private Vector3 target;
    public float damage;
    [SerializeField] private GameObject radius;
    private Vector3 direction;
    private ParticleSystem[] particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponentsInChildren<ParticleSystem>();
    }

	private void OnEnable()
	{
        gameObject.SetActive(true);
        radius.transform.parent = transform;
        phase = 1;
    }

	private void OnDestroy()
	{
        Destroy(radius);
    }

	// Update is called once per frame
	void Update()
    {
        switch (phase)
        {
            case 1:
            direction = Vector3.up;
            break;

            case 2:
            direction = Vector3.down;
            break;

            case 3:
            direction = Vector3.zero;
            break;
        }
        transform.Translate(speed * Time.deltaTime * direction, Space.World);
    }

	private void OnBecameInvisible()
	{
        if (phase == 1)
        {
            if (FindObjectOfType<TankState>())
            {
                TankState[] tanks;
                tanks = FindObjectsOfType<TankState>();

                TankState tankToTarget = tanks[Random.Range(0, tanks.Length)];
                Vector3 tempVector = new Vector3(tankToTarget.transform.position.x + Random.Range(-4, 4), tankToTarget.transform.position.y, tankToTarget.transform.position.z + Random.Range(-4, 4));

                target = tempVector;


                if (radius.transform.parent == true)
                {
                    Shot();
                }
                phase = 2;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void Shot()
    {
        radius.SetActive(true);
        transform.position = new Vector3(target.x, transform.position.y + 10, target.z);
        radius.transform.position = new Vector3(target.x, target.y, target.z);
        if (radius.transform.parent)
        {
            radius.transform.parent = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (phase == 2)
        {
            if (collider.gameObject.CompareTag("Tank") || collider.gameObject.CompareTag("GameBoard"))
            {
                radius.SetActive(false);
                StartCoroutine(Particle());
            }
        }
        
    }

    private IEnumerator Particle()
    {
        phase = 3;
        yield return new WaitForSeconds(0.01f);
        GetComponent<MeshRenderer>().enabled = false;
        particle[0].transform.GetComponent<SphereCollider>().enabled = true;
        foreach (ParticleSystem p in particle)
        {
            p.Play();
        }
        yield return new WaitForSeconds(particle[0].main.duration);
        if (gameObject.activeSelf == true)
        {
            radius.transform.parent = transform;
            gameObject.SetActive(false);
        }
    }
}
