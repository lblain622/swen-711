using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    private Vector2 direction;
    private RobotController robotController;

    void Start()
    {
        direction = transform.up;
        robotController = FindObjectOfType<RobotController>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get damage amount based on current mode
            int damage = GetDamageAmount();
            
            // Apply damage to enemy
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }

    private int GetDamageAmount()
    {
        if (robotController == null) return 1;

        switch (robotController.currentMode)
        {
            case RobotMode.Balanced: return 2;
            case RobotMode.Combat: return 4;
            default: return 1; // Stealth and Speed modes
        }
    }

    public void ActivateBullet()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
}