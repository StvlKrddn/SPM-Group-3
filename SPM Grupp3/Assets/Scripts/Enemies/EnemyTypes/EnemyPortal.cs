using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : EnemyController
{
	[SerializeField] private float timer;
	private bool opened = false;
	private BoxCollider boxCollider;
	private int randomWaypoint;

	protected override void Awake()
	{
		base.Awake();

		boxCollider = GetComponent<BoxCollider>();
		boxCollider.enabled = false;
		RandomizeTargets();
		StartCoroutine(OpenPortal());
		//Start Particle
		//Make model invisible
	}

	private void RandomizeTargets()
	{
		randomWaypoint = Random.Range(1, Waypoints.wayPoints[path].Length - 2); //For balancing reasons (and not last target)
		timer *= randomWaypoint;
		currWaypointIndex = randomWaypoint;
		transform.position = Waypoints.wayPoints[path][randomWaypoint].position;
	}

    protected override void Update()
	{
		if (opened)
		{
			MoveStep();
		}
    }

	private IEnumerator OpenPortal()
	{
		yield return new WaitForSeconds(timer);
		opened = true;
		boxCollider.enabled = true;
		//Stop particle
		//Stop invis
		yield return null;
	}
}
