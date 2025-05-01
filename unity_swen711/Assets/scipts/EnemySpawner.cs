using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<Transform> spawnPoints = new List<Transform>();
    public float startSpawnInterval = 10f;
    public float minSpawnInterval = 1.5f;
    public int maxEnemies = 5;
    public float spawnRadius = 2f; // Area around spawn points

    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;

    private int currentEnemies = 0;
    private List<Transform> availableSpawnPoints = new List<Transform>();

    void Start()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        availableSpawnPoints = new List<Transform>(spawnPoints);
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentEnemies < maxEnemies && availableSpawnPoints.Count > 0)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(GetCurrentSpawnInterval());
        }
    }

    void SpawnEnemy()
    {
        // Select random spawn point
        int randomIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[randomIndex];
        
        // Calculate random position around the spawn point
        Vector2 spawnPos = (Vector2)spawnPoint.position + 
                          Random.insideUnitCircle * spawnRadius;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.OnDeath += () => 
            {
                currentEnemies--;
                availableSpawnPoints.Add(spawnPoint); // Make spawn point available again
                Debug.Log($"Enemy died at {spawnPoint.name}. Available points: {availableSpawnPoints.Count}");
            };
        }
        
        availableSpawnPoints.Remove(spawnPoint); // Reserve this spawn point
        currentEnemies++;
        
        Debug.Log($"Enemy spawned at {spawnPoint.name}. Total enemies: {currentEnemies}");
    }

    float GetCurrentSpawnInterval()
    {
        float progress = Mathf.Clamp01(Time.timeSinceLevelLoad / 120f); // Ramp up over 2 minutes
        return Mathf.Lerp(startSpawnInterval, minSpawnInterval, progress);
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        foreach (Transform point in spawnPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(point.position, 0.5f);
            Gizmos.DrawWireSphere(point.position, spawnRadius);
        }
    }
    #endif
}