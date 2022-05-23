using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : EnemyController
{
	[SerializeField] private float spawnDuration;
	private bool opened = false;
	private BoxCollider boxCollider;
	private int randomWaypoint;
	private Canvas canvas;
	[SerializeField] private AnimationCurve fadeIn;
	private Color color;
	private MeshRenderer meshRenderer;
	private ParticleSystem[] pS;

	protected override void Awake()
	{
		base.Awake();
		RandomizeTargets();
		MakePortal();
		StartCoroutine(OpenPortal());
	}

	private void MakePortal()
	{
		boxCollider = GetComponent<BoxCollider>();
		boxCollider.enabled = false;
		canvas = GetComponentInChildren<Canvas>();
		canvas.enabled = false;
		meshRenderer = GetComponent<MeshRenderer>();
		color = meshRenderer.material.color;
		color = new Color(color.r, color.g, color.b, 0);
		pS = GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particle in pS)
		{
			var main = particle.main;
			main.duration = spawnDuration - 0.5f;
		}
		meshRenderer.enabled = true;
	}

	private void RandomizeTargets()
	{
		randomWaypoint = Random.Range(1, Waypoints.wayPoints[path].Length - 2); //For balancing reasons (and not last target)
		spawnDuration *= randomWaypoint;
		currWaypointIndex = randomWaypoint;
		transform.position = Waypoints.wayPoints[path][randomWaypoint].position;
	}

    protected override void Update()
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
		yield return new WaitForSeconds(spawnDuration);
		opened = true;
		boxCollider.enabled = true;
		canvas.enabled = true;
		yield return null;
	}
}
