using UnityEngine;

public class Mailbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Package"))
        {
            Package package = other.GetComponent<Package>();
            if (package != null && !package.isDelivered)
            {
                package.MarkAsDelivered();
                
                // Reward the robot
                RobotController robot = FindObjectOfType<RobotController>();
                if (robot != null)
                {
                    robot.batteryLevel = Mathf.Min(robot.batteryLevel * 1.2f, robot.maxBattery);
                    Debug.Log($"Robot battery increased to {robot.batteryLevel}%");
                }
                
                SpawnManager.Instance.RespawnObjects();
                
                // Destroy current objects
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}