using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMortarShot : MonoBehaviour
{
    private int phase = 1;
    private Vector3 target;
    private Vector3 direction;
    private ParticleSystem[] particle;
    private int radiusYRotation = 0;
    private Quaternion radiusRotation;
    [SerializeField] private float speed = 20;
    [SerializeField] private GameObject mortarAim;
    [SerializeField] private AudioClip mortarHitSound;

    public float Damage;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponentsInChildren<ParticleSystem>();
    }

	private void OnEnable()
	{
        gameObject.SetActive(true);
        mortarAim.transform.parent = transform;
        phase = 1;
    }

	private void OnDestroy()
	{
        Destroy(mortarAim);
    }

	// Update is called once per frame
	void Update()
    {
        switch (phase) //Checks phase of shot
        {
            case 1:
                direction = Vector3.up;
            break;

            case 2:
                direction = Vector3.down;
                radiusYRotation++;
                radiusRotation = Quaternion.Euler(90f, radiusYRotation, 0);
                mortarAim.transform.rotation = radiusRotation;
                if (radiusYRotation == 360)
                {
                    radiusYRotation = 0;
                }

                break;

            case 3:
            direction = Vector3.zero;
            break;
        }

        if(direction == Vector3.down)
        {
            mortarAim.GetComponent<Animator>().SetTrigger("Activate");
        }

        transform.Translate(speed * Time.deltaTime * direction, Space.World);
    }

	private void OnBecameInvisible()
	{
        if (phase == 1)
        {
            FindTarget();
        }
    }
    private void FindTarget()
    {
        if (FindObjectOfType<TankState>()) //Finds a random target with an offset
        {
            TankState[] tanks;
            tanks = FindObjectsOfType<TankState>();

            TankState tankToTarget = tanks[Random.Range(0, tanks.Length)];
            Vector3 tempVector = new Vector3(tankToTarget.transform.position.x + Random.Range(-4, 4), tankToTarget.transform.position.y, tankToTarget.transform.position.z + Random.Range(-4, 4));
            target = tempVector;
            mortarAim.SetActive(true);

            if (mortarAim.transform.parent == true)
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

    private void Shot()
    {
        transform.position = new Vector3(target.x, transform.position.y + 10, target.z);
        mortarAim.transform.position = new Vector3(target.x, target.y, target.z);
        if (mortarAim.transform.parent)
        {
            mortarAim.transform.parent = null;
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
            mortarAim.transform.parent = transform;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (phase == 2)
        {
            if (collider.gameObject.CompareTag("Tank") || collider.gameObject.CompareTag("GameBoard"))
            {
                EventHandler.InvokeEvent(new PlaySoundEvent("MortarShot Explode", mortarHitSound));
                mortarAim.SetActive(false);
                StartCoroutine(Particle());
            }
        }
        
    }
}
