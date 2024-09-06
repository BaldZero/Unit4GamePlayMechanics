using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed = 3.0f;
    public bool isBoss = false;

    public float spawnInterval;
    private float nextSpawn;

    public int miniEnemySpawnCount;

    private SpawnManager spawnManager;

    private Rigidbody enemyRb;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // direction the enemy moves in. follows the player.
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        //the moving code for the enemy.
        enemyRb.AddForce(lookDirection * speed); 


        if (isBoss)
        {
            if(Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnInterval;
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
