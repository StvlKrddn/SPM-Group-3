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

    private GameManager gameManager;
    private Rigidbody rb;
    private bool landed = false;
	private Vector3 direction;

	float x;
	float z;


	private Vector3 originalPosition;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
		direction = Random.insideUnitSphere.normalized;
		while ((x > 0.25f && x < -0.25f) || (z > 0.25f && z < -0.25f))
		{
		}
			//x = Random.Range(-0.35f, 0.35f);
			//z = Random.Range(-0.26f, 0.26f);
        direction = new Vector3(-3f, 0, 0);

    }

    void Update()
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
        transform.position += (direction * Time.fixedDeltaTime);
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
                changerText.GetComponentInChildren<Text>().color = Color.grey;

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
