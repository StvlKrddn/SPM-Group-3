using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MaterialBehavior : MonoBehaviour
{
    private GameManager gameManager;
    private Color dropTextColor = new Color(164,164,164,255);
    private MeshRenderer mRenderer;
    private bool landed = false;
	private Vector3 originalPosition;
    private Vector3 destinationVector;
    private float xMovement;
    private float zMovement;
    private readonly int framesTimesSeconds = 150;

    [SerializeField] private float yMovement;
    [SerializeField] private float materialValue = 1;
    [SerializeField] private float bobbingSpeed = 5f;
    [SerializeField] private float bobbingStrength = 0.2f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float secondsBeforeLanding = 3;
    [SerializeField] private float blinkDuration = 5;
    [SerializeField] private GameObject changerText;
    [SerializeField] private Transform spawnTextPosition;

    [SerializeField] private float[] xValues = new float[2];
    [SerializeField] private float[] zValues = new float[2];


    // Sverkers remake på materialkoden
    // Han tyckte att att det skulle vara mer hårdkodade värden

	private void Awake()
	{
        mRenderer = GetComponent<MeshRenderer>();
        changerText = Instantiate(changerText, spawnTextPosition.position, spawnTextPosition.rotation, GameManager.Instance.transform.Find("DropTexts"));
		changerText.GetComponentInChildren<Text>().text = materialValue.ToString();
        changerText.GetComponentInChildren<Text>().color = dropTextColor;
        changerText.SetActive(false);
        gameManager = GameManager.Instance;
	}
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        destinationVector = CalculatePosition();
    }

	private void OnEnable()
	{
        mRenderer.enabled = true;
        landed = false;
        transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        destinationVector = CalculatePosition();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private Vector3 CalculatePosition()
    {
        Vector3 destination = new Vector3(Random.Range(xValues[0], xValues[1]), 0, Random.Range(zValues[0], zValues[1]));
        xMovement = (destination.x - transform.position.x) / framesTimesSeconds;
        yMovement = -(secondsBeforeLanding / framesTimesSeconds);
        zMovement = (destination.z - transform.position.z) / framesTimesSeconds;
        return new Vector3(xMovement, yMovement, zMovement);
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
        transform.position += destinationVector;
    }

    private void Bobbing()
    {
        // Bobbing animation
        transform.position = new Vector3(transform.position.x, originalPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingStrength, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
		if (landed != true && (other.gameObject.CompareTag("PlaceForTower") || other.gameObject.CompareTag("Road") || other.gameObject.CompareTag("GameBoard")))
		{
			Landed();
		}
		if (other.gameObject.CompareTag("Tank"))
        {
            gameManager.AddMaterial(materialValue);

            if (changerText != null && spawnTextPosition != null)
            {
                changerText.transform.SetPositionAndRotation(transform.position, transform.rotation);
                changerText.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }

	private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(duration - blinkDuration);
        StartCoroutine(Blink(0.4f));
        yield return new WaitForSeconds(blinkDuration);
        gameObject.SetActive(false);
        yield return null;
    }

    private IEnumerator Blink(float divide)
    {
        mRenderer.enabled = false;
        yield return new WaitForSeconds(divide);
        mRenderer.enabled = true;
        yield return new WaitForSeconds(divide * 2);
        StartCoroutine(Blink(divide / 1.25f));
        yield return null;
    }
}
