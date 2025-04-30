using UnityEngine;

public class RobotMonitor : MonoBehaviour
{
    private RobotController robot;
    private RobotMemory memory;

    void Start()
    {
        robot = GetComponent<RobotController>();
        memory = GetComponent<RobotMemory>();
    }

    public bool IsLowPower() => robot.batteryLevel < 20f;

    public bool EnemyNearby()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy")) return true;
        }
        return false;
    }

    public bool IsInDangerZone()
    {
        return memory != null && memory.IsDangerous(transform.position);
    }
}