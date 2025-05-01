using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 10;
    public TextMeshProUGUI healthText;
    public GameObject gameOverPanel;
    
    private int currentHealth;
    private bool isDead = false;

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
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        
        currentHealth = Mathf.Max(0, currentHealth - amount);
        UpdateHealthUI();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
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

    // Called when game restarts
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        UpdateHealthUI();
        GetComponent<RobotController>().enabled = true;
    }
}