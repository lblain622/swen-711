using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Required References")]
    public SpawnManager spawnManager;
    public GameObject robotPrefab; // Reference to your robot prefab
    public Transform robotSpawnPoint;

    private GameObject currentRobot;

    void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        // Spawn or reset robot
        if (currentRobot == null)
        {
            currentRobot = Instantiate(robotPrefab, robotSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            currentRobot.GetComponent<RobotController>().ResetRobot();
            currentRobot.transform.position = robotSpawnPoint.position;
        }

        // Initialize object spawning
        if (spawnManager != null)
        {
            spawnManager.SpawnInitialObjects();
        }
        else
        {
            Debug.LogError("SpawnManager reference missing!");
        }

        Debug.Log("Game initialized with robot");
    }
}