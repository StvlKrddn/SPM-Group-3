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
	private SkinnedMeshRenderer skinnedMeshRenderer;
	private ParticleSystem[] pS;
	[SerializeField] private float spawnDuration;
	[SerializeField] private AnimationCurve fadeIn;

	protected override void OnEnable()
	{
		base.OnEnable();
		spawnDuration /= randomWaypoint;
		color = new Color(color.r, color.g, color.b, 0);
		skinnedMeshRenderer.material.color = color;
		opened = false;
		if (animator != null)
		{
			animator.enabled = false;
		}
		Spawn();
	}

	protected override void Awake()
	{
		base.Awake();
		boxCollider = GetComponent<BoxCollider>();
		canvas = GetComponentInChildren<Canvas>();
		skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
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
		color = skinnedMeshRenderer.material.color;
		color = new Color(color.r, color.g, color.b, 0);
		foreach (ParticleSystem particle in pS)
		{
			particle.Stop();
			var main = particle.main;
			main.duration = spawnDuration - 0.5f;
		}
		skinnedMeshRenderer.enabled = true;
	}

	private void RandomizeTargets()
	{
		randomWaypoint = Random.Range(1, wayPoints[Path].Length - 2); //For balancing reasons (and not last target)
		spawnDuration *= randomWaypoint;
		currentWaypointIndex = randomWaypoint;
		target = wayPoints[Path][currentWaypointIndex];
		transform.position = wayPoints[Path][currentWaypointIndex].position;
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
		skinnedMeshRenderer.material.color = color;
	}

	private IEnumerator OpenPortal()
	{
		yield return new WaitForSeconds(0.01f);
		foreach (ParticleSystem particle in pS)
		{
			particle.Play();
		}
		yield return new WaitForSeconds(spawnDuration);
		if (animator != null)
		{
			animator.enabled = true;
		}
		opened = true;
		boxCollider.enabled = true;
		canvas.enabled = true;
	}
}
