using UnityEngine;

public class RobotAnalyzer : MonoBehaviour
{
    private RobotMonitor monitor;
    private RobotPlanner planner;

    void Start()
    {
        monitor = GetComponent<RobotMonitor>();
        planner = GetComponent<RobotPlanner>();
    }

    void Update()
    {
        if (monitor.IsLowPower())
            planner.PlanStealth();
        else if (monitor.EnemyNearby())
            planner.PlanCombat();
        else
            planner.PlanBalanced();
    }
}