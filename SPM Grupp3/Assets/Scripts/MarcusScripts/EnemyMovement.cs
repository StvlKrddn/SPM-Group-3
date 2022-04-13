using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
/*    public Vector3 moveDirection;*/
    public float timeBeforeDestruction = 10f;
    public float countdown = 10f;
    private GameManager gM;
    private int currIndex = 0;
    private Transform target;
    private int damage = 1;
    public GameObject[] wayPoints;
/*    public Transform wayPointContainer;*/
    public float minDist; 


/*    private void Start()
    {
        //target = Waypoints.wayPoints[currIndex];
        Transform waypointContainer = transform.Find("Waypoints");
        
        for (int i = 0; i < waypointContainer.childCount; i++)
        {
            wayPoints[i] = waypointContainer.GetChild(i).gameObject;
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        /*Vector3 moveDirection = target.position - transform.position;*/
        float dist = Vector3.Distance(gameObject.transform.position, wayPoints[currIndex].transform.position);

        if (dist > minDist)
        {
            gameObject.transform.LookAt(wayPoints[currIndex].transform.position);
            gameObject.transform.position += gameObject.transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            currIndex++;
            if (currIndex == wayPoints.Length)
            {
                Destroy(gameObject);
                //Base take damage
            }
        }

        /*transform.position += moveSpeed * moveDirection * Time.deltaTime;*/
/*
        if (countdown <= 0f)
        {
            Destroy(gameObject);
            return;
        }
        countdown -= Time.deltaTime;*/

        /*        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
                {
                    NextTarget();
                }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShots"))
        {
            BulletBehavior bullet = other.gameObject.GetComponent<BulletBehavior>();
            //GameObject hitEffektInstance = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    /*    private void NextTarget()
        {
            if (Waypoints.wayPoints.Length - 1 <= currIndex) // Changes waypoint to til the enemy reaches the last waypoint
            {
                EnemyDeathBase();
                return;
            }
            currIndex++;
            target = Waypoints.wayPoints[currIndex];
        }

        private void EnemyDeathBase()
        {
            gM.TakeDamage(damage);
            Destroy(gameObject);
        }*/
}
