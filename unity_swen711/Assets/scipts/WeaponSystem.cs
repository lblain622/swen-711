using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public float baseDamage = 10f;
    public float energyCost = 5f;
    public float damageMultiplier = 1f;  // Modified by RobotController

    private PowerManagement powerManagement;

    void Start()
    {
        powerManagement = GetComponent<PowerManagement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && powerManagement.currentBattery >= energyCost)
        {
            Attack();
        }
    }

    void Attack()
    {
        float actualDamage = baseDamage * damageMultiplier;
        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<DamageSystem>().TakeDamage(actualDamage);
            }
        }

        powerManagement.currentBattery -= energyCost;
    }
}