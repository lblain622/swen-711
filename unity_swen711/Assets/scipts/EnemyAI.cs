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
        if (!isDead && player != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                player.position, 
                speed * Time.deltaTime
            );
        }
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
        if (!isDead && collision.gameObject.CompareTag("Player"))
        {
            var playerDamage = collision.gameObject.GetComponent<DamageSystem>();
            if (playerDamage != null)
            {
                playerDamage.TakeDamage(contactDamage);
            }
            Die(); // Enemy dies on contact
        }
    }
}