using UnityEngine;

public class RobotPlanner : MonoBehaviour
{
    private RobotController controller;

    void Start()
    {
        controller = GetComponent<RobotController>();
    }

    public void PlanStealth()
    {
        controller.SetMode(RobotMode.Stealth);
    }

    public void PlanCombat()
    {
        controller.SetMode(RobotMode.Combat);
    }

    public void PlanBalanced()
    {
        controller.SetMode(RobotMode.Balanced);
    }
}