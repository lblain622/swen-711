using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Bullet speed
    public float lifetime = 5f; // Time before the bullet destroys itself

    private Vector2 direction; // Direction the bullet is moving in

    void Start()
    {
        // Set the bullet's direction based on the player's facing direction
        direction = transform.up; // Assumes the bullet moves forward in the local "up" direction

        // Destroy the bullet after a set lifetime to prevent it from staying forever
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the bullet in the direction it's facing
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy the bullet on impact with any object
        Destroy(gameObject);
    }

    public void ActivateBullet()
    {
        // Make the bullet visible by enabling the renderer
        GetComponent<SpriteRenderer>().enabled = true;
    }
}