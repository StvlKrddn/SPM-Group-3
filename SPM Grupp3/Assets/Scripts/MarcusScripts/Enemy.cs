using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    public GameObject enemy;
    public float spawnColdown = 5f;
    public float timer = 2f;

    private GameObject enemyInstance = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {            
        if (timer <= 0f)
        {
            StartCoroutine(SpawnEnemy());
                
            timer = spawnColdown;
        }
        timer -= Time.deltaTime;
    }

    IEnumerator SpawnEnemy()
    {
        float amountToSpawn = Random.Range(1f, 7f);
        for (int i = 0; i < amountToSpawn; i++)
        {
            enemyInstance = Instantiate(enemy, transform.position, transform.rotation);
            enemyInstance.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
