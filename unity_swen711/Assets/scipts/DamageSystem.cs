using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public int baseHealth = 100;
    public float damageReduction = 1f;  // Modified by RobotController
    public float healthMultiplier = 1f;  // Modified by RobotController
    
    private int currentHealth;

    void Start()
    {
        currentHealth = (int)(baseHealth * healthMultiplier);
    }

    public void TakeDamage(float amount)
    {
        float reducedDamage = amount * damageReduction;
        currentHealth -= (int)reducedDamage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}