using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float speed = 10;
    private Transform target;
    private int currIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        target = Waypoints.wayPoints[currIndex];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
        direction.Normalize();
        transform.Translate(speed * Time.deltaTime * direction);

        Debug.DrawRay(transform.position, direction, Color.red);


        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            NextTarget();
        }
    }

    private void NextTarget()
    {
        if (Waypoints.wayPoints.Length - 1 <= currIndex)
        {
            currIndex = -1;
            //EnemyDeath();
            //return;
        }
        currIndex++;
        target = Waypoints.wayPoints[currIndex];
    }

    private void EnemyDeath()
    {
        //Drop material
        Destroy(gameObject);
    }
}
