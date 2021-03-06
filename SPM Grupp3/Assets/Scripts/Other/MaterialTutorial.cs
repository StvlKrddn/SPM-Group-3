using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTutorial : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed = 5f;
    [SerializeField] private float bobbingStrength = 0.2f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private int amountOfMaterialGained; 
    private GameManager gameManager;
    private Rigidbody rb;
    private bool landed = false;


    public GameObject objectToDisable;
    public GameObject objectToEnable;

    public GameObject anotherGameObjectToEnable;

 //   public GameObject anotherGameObjectToEnableAgain;

    private Vector3 originalPosition;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        Throw();
        StartCoroutine(SelfDestruct());
    }

    void Update()
    {
        if (landed == true)
        {
            Bobbing();
        }
        Invoke(nameof(Landed), 1.5f);
    }

    private void Landed()
    {
        landed = true;
    }

    private void Throw()
    {
        transform.position += transform.right * 10;
        transform.position += transform.up * 10;
    }

    private void Bobbing()
    {
        // Bobbing animation
        transform.position = new Vector3(transform.position.x, originalPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingStrength, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tank"))
        {
            gameManager.AddMaterial(amountOfMaterialGained);
            Destroy(gameObject);
            objectToDisable.SetActive(false);
            anotherGameObjectToEnable.SetActive(true);
            objectToEnable.SetActive(true);
      //      anotherGameObjectToEnableAgain.SetActive(true);
        }

  
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
        yield return null;
    }
}
