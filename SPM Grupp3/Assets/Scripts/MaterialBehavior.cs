using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBehavior : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed = 5f;
    [SerializeField] private float bobbingStrength = 0.2f;

    private Vector3 originalPosition;

    void Start()
    {
        

        originalPosition = transform.position;
    }

    void Update()
    {
        // Bobbing animation
        transform.position = new Vector3(transform.position.x, originalPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingStrength, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Materials++;
            Destroy(gameObject);
        }
    }
}
