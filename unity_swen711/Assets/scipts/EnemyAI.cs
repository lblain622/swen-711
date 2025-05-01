using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    public float baseSpeed = 2f;
    private float currentSpeed;
    private float distance;
    private RobotController playerController;
    public int health = 4;
    public int contactDamage = 1;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Player' found!");
        }
        playerController = player.GetComponent<RobotController>();
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        // Adjust behavior based on player mode
        if (playerController != null && playerController.currentMode == RobotMode.Stealth)
        {
            // In stealth mode, only chase if very close
            float detectionRange = playerController.stealthDetectionRange;
            if (Vector2.Distance(transform.position, player.position) > detectionRange)
            {
                return; // Don't chase if too far in stealth mode
            }
            currentSpeed = baseSpeed * 0.7f; // Slower pursuit in stealth mode
        }
        else
        {
            currentSpeed = baseSpeed; // Normal speed otherwise
        }

        Vector2 direction = player.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(
            this.transform.position, 
            player.position, 
            currentSpeed * Time.deltaTime
        );
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle player contact
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(contactDamage);
                Destroy(gameObject); 
            }
        }
    }
}