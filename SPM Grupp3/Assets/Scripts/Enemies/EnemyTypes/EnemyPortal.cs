using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : EnemyController
{
	private bool opened = false;
	private BoxCollider boxCollider;
	private int randomWaypoint;
	private Canvas canvas;
	private Color color;
	private MeshRenderer meshRenderer;
	private ParticleSystem[] pS;
	[SerializeField] private float spawnDuration;
	[SerializeField] private AnimationCurve fadeIn;

	protected override void OnEnable()
	{
		base.OnEnable();
		spawnDuration /= randomWaypoint;
		color = new Color(color.r, color.g, color.b, 0);
		meshRenderer.material.color = color;
		opened = false;
		Spawn();
	}

	protected override void Awake()
	{
		base.Awake();
		boxCollider = GetComponent<BoxCollider>();
		canvas = GetComponentInChildren<Canvas>();
		meshRenderer = GetComponent<MeshRenderer>();
		pS = GetComponentsInChildren<ParticleSystem>();
		Spawn();
	}

	private void Spawn()
	{
		RandomizeTargets();
		MakePortal();
		StartCoroutine(OpenPortal());
	}

	private void MakePortal()
	{
		boxCollider.enabled = false;
		canvas.enabled = false;
		color = meshRenderer.material.color;
		color = new Color(color.r, color.g, color.b, 0);
		foreach (ParticleSystem particle in pS)
		{
			particle.Stop();
			var main = particle.main;
			main.duration = spawnDuration - 0.5f;
		}
		meshRenderer.enabled = true;
	}

	private void RandomizeTargets()
	{
		randomWaypoint = Random.Range(1, wayPoints[Path].Length - 2); //For balancing reasons (and not last target)
		spawnDuration *= randomWaypoint;
		currentWaypointIndex = randomWaypoint;
		transform.position = wayPoints[Path][randomWaypoint].position;
	}

    protected override void FixedUpdate()
	{
		if (opened)
		{
			MoveStep();
		}
		else
		{
			ChangeAlpha();
		}
	}

	private void ChangeAlpha()
	{
		color = new Color(color.r, color.g, color.b, color.a + (1 / spawnDuration) * Time.deltaTime);
		meshRenderer.material.color = color;
	}

	private IEnumerator OpenPortal()
	{
		yield return new WaitForSeconds(0.01f);
		foreach (ParticleSystem particle in pS)
		{
			particle.Play();
		}
		yield return new WaitForSeconds(spawnDuration);
		opened = true;
		boxCollider.enabled = true;
		canvas.enabled = true;
	}
}
