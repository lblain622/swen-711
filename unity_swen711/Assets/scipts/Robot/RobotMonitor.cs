using UnityEngine;

public class RobotMonitor : MonoBehaviour
{
    private RobotController robot;

    void Start()
    {
        robot = GetComponent<RobotController>();
    }

    public bool IsLowPower()
    {
        return robot.batteryLevel < 20f;
    }

    public bool EnemyNearby()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy")) return true;
        }
        return false;
    }

    public bool CriticalPackage()
    {
        // Placeholder, can expand later
        return false;
    }
}