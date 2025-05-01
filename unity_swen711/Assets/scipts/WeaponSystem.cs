using UnityEngine;

[System.Serializable]
public class WeaponSettings
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float bulletLifetime = 3f;
    public float fireRate = 0.2f;
}

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon Configuration")]
    public WeaponSettings settings;
    
    [Header("Damage Settings")]
    public float baseDamage = 10f;
    public float energyCost = 5f;
    public float damageMultiplier = 1f;
    
    [Header("Mode Multipliers")]
    public float combatDamageMultiplier = 1.5f;
    public float stealthDamageMultiplier = 0.7f;
    public float speedDamageMultiplier = 0.5f;

    private float nextFireTime;
    private PowerManagement powerManagement;
    private RobotController robotController;
    private Camera mainCamera;

    void Start()
    {
        powerManagement = GetComponent<PowerManagement>();
        robotController = GetComponent<RobotController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleShootingInput();
    }

    void HandleShootingInput()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            if (powerManagement != null && powerManagement.currentBattery >= energyCost)
            {
                FireBullet();
                nextFireTime = Time.time + settings.fireRate;

                //powerManagement.currentBattery -= energyCost;

            }
        }
    }

    void FireBullet()
    {
        if (settings.bulletPrefab == null || settings.firePoint == null)
        {
            Debug.LogWarning("Bullet prefab or fire point not assigned!");
            return;
        }

        // Calculate bullet direction (towards mouse)
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)settings.firePoint.position).normalized;
        
        // Create bullet
        GameObject bullet = Instantiate(
            settings.bulletPrefab,
            settings.firePoint.position,
            Quaternion.identity
        );

        // Configure bullet
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(
                direction,
                settings.bulletSpeed,
                settings.bulletLifetime,
                CalculateDamage()
            );
        }
    }

    float CalculateDamage()
    {
        float damage = baseDamage * damageMultiplier;
        
        if (robotController != null)
        {
            switch (robotController.currentMode)
            {
                case RobotMode.Combat:
                    damage *= combatDamageMultiplier;
                    break;
                case RobotMode.Stealth:
                    damage *= stealthDamageMultiplier;
                    break;
                case RobotMode.Speed:
                    damage *= speedDamageMultiplier;
                    break;
                // Balanced mode uses base multiplier
            }
        }
        
        return damage;
    }
}