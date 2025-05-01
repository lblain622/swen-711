using UnityEngine;
using System; // Required for Action

public class EnemyAI : MonoBehaviour
{
    [Header("Combat Settings")]
    public int health = 4; // Matches your original design
    public int contactDamage = 1;
    public float speed = 2f;

    // DamageSystem event
    public event Action OnDeath;

    private Transform player;
 
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found!");
            enabled = false;
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        // Check if player is in stealth and stationary
        RobotController playerRobot = player.GetComponent<RobotController>();
        if (playerRobot != null && playerRobot.inStealthFromStationary)
        {
            return; // Ignore player in stealth
        }

        // Normal movement toward player
        transform.position = Vector2.MoveTowards(
            transform.position, 
            player.position, 
            speed * Time.deltaTime
        );
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        OnDeath?.Invoke(); // Notify spawner
        Destroy(gameObject, 0.1f); // Small delay for event propagation
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                Debug.Log("hit player");
                playerHealth.TakeDamage(contactDamage);
            }else
            {
                Debug.LogWarning("Player object missing PlayerHealth component!");
            }
            Die();
            
        }
    }
}