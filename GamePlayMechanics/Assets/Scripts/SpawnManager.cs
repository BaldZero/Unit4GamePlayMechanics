using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject enemyPrefab;
    public GameObject powerUpPrefab;
    public int enemyCount;
    public int waveNumber = 1;
    
    private float spawnRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        Instantiate(powerUpPrefab, MakeSpawnPos(), powerUpPrefab.transform.rotation);
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
            Instantiate(powerUpPrefab, MakeSpawnPos(), powerUpPrefab.transform.rotation);
        }
    }
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, MakeSpawnPos(), enemyPrefab.transform.rotation);
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
