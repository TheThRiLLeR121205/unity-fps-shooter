using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;               // Enemy prefab
    public BoxCollider spawnArea;                // Area where enemies can spawn
    public int initialEnemies = 5;               // Number of enemies at start
    public float respawnTime = 5f;               // Time to respawn after death
    public float minDistanceBetweenEnemies = 2f; // Prevent overlap

    void Start()
    {
        for (int i = 0; i < initialEnemies; i++)
        {
            SpawnEnemyAtRandomPosition();
        }
    }

    void SpawnEnemyAtRandomPosition()
    {
        Vector3 spawnPos = GetRandomFreePosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            // When enemy dies, respawn it after respawnTime
            health.OnDeath += () => StartCoroutine(RespawnEnemy(spawnPos));
        }
    }

    Vector3 GetRandomFreePosition()
    {
        Vector3 spawnPos;
        int attempts = 0;

        do
        {
            spawnPos = GetRandomPositionInBox();
            attempts++;
            if (attempts > 50) break; // avoid infinite loop
        }
        while (!IsPositionFree(spawnPos, minDistanceBetweenEnemies));

        return spawnPos;
    }

    Vector3 GetRandomPositionInBox()
    {
        // Use bounds of BoxCollider to get world-space min/max
        Vector3 min = spawnArea.bounds.min;
        Vector3 max = spawnArea.bounds.max;

        float x = Random.Range(min.x, max.x);
        float z = Random.Range(min.z, max.z);

        // Adjust Y so enemy sits on the ground
        float y = spawnArea.transform.position.y + 1f; // if capsule height = 2

        return new Vector3(x, y, z);
    }

    bool IsPositionFree(Vector3 pos, float radius)
    {
        return !Physics.CheckSphere(pos, radius); // true if no collider overlaps
    }

    IEnumerator RespawnEnemy(Vector3 spawnPos)
    {
        yield return new WaitForSeconds(respawnTime);
        SpawnEnemyAtRandomPosition();
    }
}
