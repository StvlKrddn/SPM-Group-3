using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material : MonoBehaviour
{
    public float timer = 0;
    private float activeTime = 3f;
    private int materialWorth = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= activeTime)
        {
            Destroy(gameObject);
        }
    }

    /* if needed

	private void OnTriggerEnter(Collider other)
	{
        if (other.tag == "tank") //change to tank
        {
            FindObjectOfType<GameManager>().AddMaterial(materialWorth);
            Destroy(this);
        }
    }

    */
}
