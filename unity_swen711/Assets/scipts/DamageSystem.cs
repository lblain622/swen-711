using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(float amount)
    {
        health -= (int)amount;
        if (health <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}