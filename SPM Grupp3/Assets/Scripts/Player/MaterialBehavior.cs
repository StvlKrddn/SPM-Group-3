using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBehavior : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed = 5f;
    [SerializeField] private float bobbingStrength = 0.2f;
    [SerializeField] private float duration = 5f;
    private GameManager gameManager;
    private Rigidbody rb;
    private bool landed = false;

    private Vector3 originalPosition;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        Throw();
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
        transform.position += transform.right * 7 * Time.deltaTime;
        transform.position += transform.up * 7 * Time.deltaTime;
    }

    private void Bobbing()
    {
        // Bobbing animation
        transform.position = new Vector3(transform.position.x, originalPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingStrength, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
		if (landed != true && other.gameObject.CompareTag("PlaceForTower"))
		{
			Landed();
		}
		if (other.gameObject.CompareTag("Tank"))
        {
            gameManager.AddMaterial(1);
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
