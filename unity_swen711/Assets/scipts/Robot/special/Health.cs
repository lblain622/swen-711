using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 5;
    public TextMeshProUGUI healthText;
    public GameObject gameOverPanel;
    public float stealthRegenRate = 2f; // Health per second in stealth
    
    private int currentHealth;
    private bool isDead = false;
    private bool isRegenerating = false;
    private float regenRate = 0f;

    public AudioSource source; 

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        // Handle R key restart only when dead
        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.RestartGame();
        }
        
        // Health regeneration
        if (isRegenerating)
        {
            int regenAmount = Mathf.RoundToInt(1* Time.deltaTime);
            Debug.Log("Healing:" + regenAmount);
            if (regenAmount > 0) // Only heal if regenRate is positive
            {
                currentHealth = Mathf.Min(maxHealth, currentHealth + regenAmount);
                UpdateHealthUI();
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        
        currentHealth = Mathf.Max(0, currentHealth - amount);
        UpdateHealthUI();
        
        // Exit stealth if damaged
        RobotController controller = GetComponent<RobotController>();
        if (controller != null && controller.inStealthFromStationary)
        {
            controller.ExitStationaryStealth();
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        source.Stop();
        isDead = true;
        Time.timeScale = 0f;
        GetComponent<RobotController>().enabled = false;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
        }
    }

    public void StartRegen(float rate)
    {
        isRegenerating = true;
        regenRate = rate;
    }

    public void StopRegen()
    {
        isRegenerating = false;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        isRegenerating = false;
        UpdateHealthUI();
        GetComponent<RobotController>().enabled = true;
    }
}