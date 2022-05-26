using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MaterialBehavior : MonoBehaviour
{
    [SerializeField] private float materialValue = 1;

    [SerializeField] private float bobbingSpeed = 5f;
    [SerializeField] private float bobbingStrength = 0.2f;
    [SerializeField] private float duration = 5f;

    [SerializeField] private GameObject changerText;
    [SerializeField] private Transform spawnTextPosition;

    private Color dropTextColor = new Color(164,164,164,255);

    private GameManager gameManager;
    private Rigidbody rb;
    private bool landed = false;
	private Vector3 direction;

	float x;
	float z;


    public float[] xValues = new float[2];
    public float[] zValues = new float[2];

    private int timeModifier;

	private Vector3 originalPosition;

    private float xMovement;


    public float secondsBeforeLanding = 3; 

    public float yMovement =-0.016666666f;

    private float zMovement;


    private int howManyTimes; 

    Vector3 tempVector;
    void Start()
    {
        //  transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        transform.position = new Vector3(transform.position.x, 3, transform.position.z);


        Vector3 destination = new Vector3(Random.Range(xValues[0], xValues[1]), 0, Random.Range(zValues[0], zValues[1]));

        xMovement = (destination.x - transform.position.x) / 150;
        yMovement = -(secondsBeforeLanding / 150);
        zMovement = (destination.z - transform.position.z) / 150;

        tempVector = new Vector3(xMovement, yMovement, zMovement);
        
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
		direction = Random.insideUnitSphere.normalized;
		/*while ((x > 0.25f && x < -0.25f) || (z > 0.25f && z < -0.25f))
		{
		}
			x = Random.Range(-0.35f, 0.35f);
			z = Random.Range(-0.26f, 0.26f);*/
        direction = new Vector3(-3f, 0, 0);

    }

    void Update()
    {
	//	if (landed == true)
	//	{
	//		Bobbing();
	//	}
	//	else
	//	{
	//		Throw();
	//	}
    }

    private void FixedUpdate()
    {
        if (landed == true)
        {

            
            Bobbing();
        }
        else
        {
            Throw();
        }
    }


    private void Landed()
    {
        originalPosition = transform.position;
		originalPosition.y += 1;

        landed = true;
		StartCoroutine(SelfDestruct());
	}

    private void Throw()
    {
        howManyTimes += 1;


        transform.position += tempVector;
       //     transform.Translate(xMovement, yMovement, zMovement);
       //   transform.position += (direction * Time.fixedDeltaTime);
       //transform.Translate(transform.position * x * Time.smoothDeltaTime);
       //transform.Translate(transform.position * z * Time.smoothDeltaTime);
       //transform.Translate(transform.up * 5 * Time.smoothDeltaTime);
    }

    private void Bobbing()
    {
        // Bobbing animation
        transform.position = new Vector3(transform.position.x, originalPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingStrength, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
		if (landed != true && (other.gameObject.CompareTag("PlaceForTower") || other.gameObject.CompareTag("Road")))
		{
			Landed();
		}
		if (other.gameObject.CompareTag("Tank"))
        {
            gameManager.AddMaterial(materialValue);

            if (changerText != null)
            {
                changerText.GetComponentInChildren<Text>().text = materialValue.ToString();
                changerText.GetComponentInChildren<Text>().color = dropTextColor;

                if (spawnTextPosition != null)
                    Instantiate(changerText, spawnTextPosition.position, spawnTextPosition.rotation);
                else
                {
                    Instantiate(changerText);
                    print("No transform-point for changerText");
                }
            }
            Destroy(gameObject);
        }
    }

	private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
        yield return null;
    }
}
