using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    
    [Header("Spawn Settings")]
    public List<Transform> spawnPoints = new List<Transform>();
    public float minDistanceBetweenObjects = 3f;
    
    [Header("Prefabs")]
    public GameObject mailboxPrefab;
    public GameObject packagePrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes if needed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (spawnPoints.Count < 2)
        {
            Debug.LogError("Assign at least 2 spawn points in Inspector!");
            return;
        }

        SpawnInitialObjects();
    }

    public void SpawnInitialObjects()
    {
        // Clear any existing objects first
        ClearExistingObjects();

        // Get valid spawn positions
        if (TryGetValidSpawnPositions(out Vector3 mailboxPos, out Vector3 packagePos))
        {
            Instantiate(mailboxPrefab, mailboxPos, Quaternion.identity);
            Instantiate(packagePrefab, packagePos, Quaternion.identity);
            Debug.Log($"Spawned objects at {mailboxPos} and {packagePos}");
        }
        else
        {
            Debug.LogError("Failed to find valid spawn positions!");
        }
    }
    public void RespawnObjects() 
    {
        // Clear existing objects
        ClearExistingObjects();

        // Get valid spawn positions
        if (TryGetValidSpawnPositions(out Vector3 mailboxPos, out Vector3 packagePos))
        {
            Instantiate(mailboxPrefab, mailboxPos, Quaternion.identity);
            Instantiate(packagePrefab, packagePos, Quaternion.identity);
            Debug.Log($"Respawned objects at {mailboxPos} and {packagePos}");
        }
    }

    private bool TryGetValidSpawnPositions(out Vector3 mailboxPos, out Vector3 packagePos)
    {
        mailboxPos = Vector3.zero;
        packagePos = Vector3.zero;

        // Try multiple combinations to find valid positions
        for (int i = 0; i < 10; i++)
        {
            int rand1 = Random.Range(0, spawnPoints.Count);
            int rand2 = Random.Range(0, spawnPoints.Count);

            if (rand1 == rand2) continue;

            Vector3 pos1 = spawnPoints[rand1].position;
            Vector3 pos2 = spawnPoints[rand2].position;

            if (Vector3.Distance(pos1, pos2) >= minDistanceBetweenObjects)
            {
                mailboxPos = pos1;
                packagePos = pos2;
                return true;
            }
        }
        return false;
    }

    private void ClearExistingObjects()
    {
        // Clean up any existing objects
        var existingMailboxes = FindObjectsOfType<Mailbox>();
        var existingPackages = FindObjectsOfType<Package>();

        foreach (var obj in existingMailboxes) Destroy(obj.gameObject);
        foreach (var obj in existingPackages) Destroy(obj.gameObject);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        foreach (Transform point in spawnPoints)
        {
            Gizmos.DrawWireSphere(point.position, 0.5f);
        }
    }
    #endif
}