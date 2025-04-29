using UnityEngine;

public class PowerManagement : MonoBehaviour
{
    public float maxBattery = 100f;
    public float currentBattery;
    public float batteryDrainRate = 1f;

    void Start()
    {
        currentBattery = maxBattery;
    }

    void Update()
    {
        currentBattery -= batteryDrainRate * Time.deltaTime;
        
        if (currentBattery <= 0)
        {
            currentBattery = 0;
            // Handle out of power condition, like stopping movement
        }
    }
}