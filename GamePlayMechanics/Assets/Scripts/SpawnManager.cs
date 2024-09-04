using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] enemyPrefabs;
    public GameObject[] powerUpPrefabs;
    public int enemyCount;
    public int waveNumber = 1;
    
    private float spawnRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);

        int randomPowerup = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[randomPowerup], MakeSpawnPos(), powerUpPrefabs[randomPowerup].transform.rotation);
    }
    

    // Update is called once per frame
    void Update()
    {
        //finds howw many enemies are in the scene
        enemyCount = FindObjectsOfType<Enemy>().Length;

        //spawns more enemies when the count is 0
        if(enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            int randomPowerup = Random.Range(0, powerUpPrefabs.Length);
            Instantiate(powerUpPrefabs[randomPowerup], MakeSpawnPos(), powerUpPrefabs[randomPowerup].transform.rotation);
        }
    }
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {

            int randomEnemy = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomEnemy], MakeSpawnPos(), enemyPrefabs[randomEnemy].transform.rotation);
        }
    }

    private Vector3 MakeSpawnPos()
    {
        float spawnX = Random.Range(-spawnRange, spawnRange);
        float spawnZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnX, 0, spawnZ);

        return randomPos;
    }
}
