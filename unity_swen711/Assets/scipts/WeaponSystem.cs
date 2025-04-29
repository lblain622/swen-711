using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public float damage = 10f;
    public float energyCost = 5f;

    private PowerManagement powerManagement;

    void Start()
    {
        powerManagement = GetComponent<PowerManagement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && powerManagement.currentBattery >= energyCost)
        {
            Attack();
        }
    }

    void Attack()
    {
        // Example of damage dealing, e.g., to an enemy
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<DamageSystem>().TakeDamage(damage);
            }
        }

        powerManagement.currentBattery -= energyCost;
    }
}