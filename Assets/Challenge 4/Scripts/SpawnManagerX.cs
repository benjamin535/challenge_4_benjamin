using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    private float spawnRangeX = 10f;
    private float spawnZMin = 15f; // set min spawn Z
    private float spawnZMax = 25f; // set max spawn Z

    public int enemyCount;
    public int waveCount = 1;
    public float enemySpeed = 50f; // Initial speed of enemies

    public GameObject player; 

    void Update()
    {
        // Count enemies in the scene
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // Spawn a new wave if no enemies are left
        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }
    }

    // Generate random spawn position for powerups and enemy balls
    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }

    // Spawn enemy wave with increasing difficulty
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15); // Position powerups closer to the player

        // Spawn a powerup if none exists
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }

        // Spawn enemies and set their speed
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);

            // Pass the enemy speed to the enemy's script if it exists
            EnemyX enemyScript = enemy.GetComponent<EnemyX>();
            if (enemyScript != null)
            {
                enemyScript.SetSpeed(enemySpeed);
            }
        }

        // Increase wave count and enemy speed for the next wave
        waveCount++;
        enemySpeed += 5f; // Increment speed each wave
        
        // Reset player position to start position
        ResetPlayerPosition();
    }

    // Move player back to starting position
    void ResetPlayerPosition()
    {
        if (player != null)
        {
            player.transform.position = new Vector3(0, 1, -7);
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
            }
        }
    }
}
