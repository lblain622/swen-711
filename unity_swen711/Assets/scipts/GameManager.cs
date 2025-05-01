using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    
    public TextMeshProUGUI batteryText;
    
    [Header("Restart Settings")]
    public float holdToRestartDuration = 1.5f;
   
    
    private float restartHoldTimer = 0f;
    private PlayerHealth playerHealth;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializePlayer();
        Time.timeScale = 1f; // Ensure game is unpaused on load
    }

    void InitializePlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ResetHealth();
            }
        }
    }

    void Update()
    {
        HandleRestartInput();
        UpdateUI();
    }

    void HandleRestartInput()
    {
        if (Input.GetKey(KeyCode.R))
        {
            restartHoldTimer += Time.unscaledDeltaTime;
            
            

            if (restartHoldTimer >= holdToRestartDuration)
            {
                RestartGame();
            }
        }
        else if (restartHoldTimer > 0)
        {
            restartHoldTimer = 0f;
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
            scoreText.text = $"Score: {score}";
        }
        
        // Update battery text if needed
        RobotController player = FindObjectOfType<RobotController>();
        if (player != null && batteryText != null)
        {
            batteryText.text = $"Battery: {Mathf.RoundToInt(player.batteryLevel)}%";
        }
    }

    public void RestartGame()
    {
        restartHoldTimer = 0f;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}