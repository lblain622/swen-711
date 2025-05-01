using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float lifetime;
    private float damage;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    
        
        if (GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
    
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<CircleCollider2D>();
        }
    }


    public void Initialize(Vector2 moveDirection, float moveSpeed, float bulletLifetime, float bulletDamage)
    {
        direction = moveDirection;
        speed = moveSpeed;
        lifetime = bulletLifetime;
        damage = bulletDamage;
        
        // Activate bullet
        spriteRenderer.enabled = true;
        Destroy(gameObject, lifetime);
        
        // Rotate bullet to face movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Bullet collided with: {collision.gameObject.name}"); // Debug line

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                Debug.Log("Dealing damage to enemy");
                enemy.TakeDamage(Mathf.RoundToInt(damage));
            }
        }

        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Bullet triggered with: {other.gameObject.name}");
    
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                Debug.Log("Dealing trigger damage to enemy");
                enemy.TakeDamage(Mathf.RoundToInt(damage));
            }
        }
        Destroy(gameObject);
    }
}