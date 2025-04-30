using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    public int health = 10;
    public GameObject gameOverPanel; 
    public TextMeshProUGUI healthText;
    void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Debug.Log("Player died!");
        }
    }
    
    void GameOver()
    {
       
        Time.timeScale = 0f;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
 
        GetComponent<RobotController>().enabled = false;
       
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }
    public void QuitGame()
    {
        Application.Quit();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // For testing in editor
    #endif
    }
}