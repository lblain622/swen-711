using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI batteryText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        
        RobotController player = FindObjectOfType<RobotController>();
        if (player != null && batteryText != null)
        {
            
            if (player.batteryLevel <= 0)
            {
                batteryText.text= "Battery is empty. Robot cannot move!";
            }
            else
            {
                batteryText.text = "Battery: " + Mathf.RoundToInt(player.batteryLevel) + "%";
            }
        }
    }
}