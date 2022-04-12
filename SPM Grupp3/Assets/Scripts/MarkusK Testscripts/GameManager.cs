using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int baseHealth = 100;
    public int material = 0;
    public int money = 350;
    public int currentWave = 0;    
    public int victoryWave = 10;

    public Transform regularEnemy;
    public Transform spawnPosition;

    public bool waveOff = false;
    public float timer = 0;
    public float timeBetweenWave = 3f;
    public float timeBetweenEnemy = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= timeBetweenWave)
        {
            StartCoroutine(SpawnWave());
            timer = 0;
            waveOff = true;
        }

        if (waveOff == false)
        {
            timer += Time.deltaTime;
        }
    }

    private IEnumerator SpawnWave ()
    {
        currentWave++;
        if (currentWave > victoryWave)
        {
            Debug.Log("Victory");
            Victory();
            yield break;
        }

        int waveLength = 0;
        switch (currentWave)
        {
            case 1:
            waveLength = 5;
            StartCoroutine(SpawnEnemies(regularEnemy, 3));
                
            break;

            case 2:
            waveLength = 5;
            StartCoroutine(SpawnEnemies(regularEnemy, 3));
            break;
        }
        yield return new WaitForSeconds(waveLength);
        waveOff = false;
    }

    IEnumerator SpawnEnemies(Transform enemyType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(enemyType, spawnPosition.position, spawnPosition.rotation);
            yield return new WaitForSeconds(timeBetweenEnemy);
        }
        yield return false;
    }

    public void TakeDamage(int damage)
    {
        baseHealth -= damage;
        if (baseHealth <= 0)
        {
            Defeat();
        }
    }


    private void Victory()
    {

    }

    private void Defeat()
    {

    }
}
