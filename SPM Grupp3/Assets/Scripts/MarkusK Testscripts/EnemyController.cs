using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameManager gM;
    private float speed = 10;
    private Transform target;
    private int currIndex = 0;
    private int damage = 1;
    private int moneyDrop = 1;

    // Start is called before the first frame update

    private void Awake()
    {
        gM = FindObjectOfType<GameManager>();
    }

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
    }

    private void EnemyDeath()
    {
        Destroy(gameObject);
        //Get moneyDrop
        //Drop material
    }
}
