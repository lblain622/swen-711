using UnityEngine;
using TMPro;
public enum RobotMode { Balanced, Stealth, Combat, Speed }

public class RobotController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI batteryText;
    
    [Header("Movement Settings")]
    public float maxBattery = 100f;
    public float baseSpeed = 5f;
    public float batteryLevel = 100f;
    public float batteryDrainRate = 1f;
    public float rotationSpeed = 10f;
    public float rotationOffset = -90f;
    public bool lockMode = false;
    
    [Header("Mode Settings")]
    public RobotMode currentMode = RobotMode.Balanced;
    public float combatDamageMultiplier = 1.5f;
    public float combatDamageReduction = 0.7f;
    public float stealthDamageMultiplier = 0.7f;
    public float stealthDetectionRange = 3f;
    public float speedVisionReduction = 0.6f;
    public float speedHealthReduction = 0.7f;
    public float healthRegenRate = 2f; 
    public float stealthActivationTime = 2f; // Changed from stealthActivationDelay

    public AudioSource source; 
    
    // Private variables
    private Package currentPackage;
    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private WeaponSystem weaponSystem;
    private DamageSystem damageSystem;
    private float defaultCameraSize;
    private float moveSpeed;
    private PlayerHealth playerHealth;
    private float stationaryTimer = 0f;
    public bool inStealthFromStationary = false;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        weaponSystem = GetComponent<WeaponSystem>();
        damageSystem = GetComponent<DamageSystem>();
        rb = GetComponent<Rigidbody2D>();
        
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on this GameObject.");
        }
        
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            defaultCameraSize = mainCamera.orthographicSize;
        }

        moveSpeed = baseSpeed;
    }

    void Awake()
    {
        SetupRigidbody();
        
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found in scene!");
            }
        }
    }

    void SetupRigidbody()
    {
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearDamping = 0f;
            rb.angularDamping = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    void Update()
    {
        HandleMovement();
        RotateToMouse();
        DrainBattery();
        CheckStealthActivation();
    }

    public void changeSpeed(float speed){
        baseSpeed = speed;
    }

    public void CheckStealthActivation()
    {
        if (!IsMoving())
        {
            stationaryTimer += Time.deltaTime;
            
            if (stationaryTimer >= stealthActivationTime && !inStealthFromStationary)
            {
                EnterStationaryStealth();
            }
        }
        else
        {
            if (inStealthFromStationary)
            {
                ExitStationaryStealth();
            }
            stationaryTimer = 0f;
        }
    }

    void EnterStationaryStealth()
    {
        inStealthFromStationary = true;
        SetMode(RobotMode.Stealth);
        lockMode = true;
        batteryDrainRate *= 1.5f;
        if (modeText != null)
        {
            modeText.text = $"Mode: {currentMode}";
        }
        
        if (playerHealth != null)
        {
            playerHealth.StartRegen(healthRegenRate);
        }
    }

    public void ExitStationaryStealth()
    {
        inStealthFromStationary = false;
        lockMode = false;
        SetMode(RobotMode.Balanced);
        batteryDrainRate = 1f;
        
        if (playerHealth != null)
        {
            playerHealth.StopRegen();
        }
    }

    public void ResetRobot()
    {
        batteryLevel = maxBattery;
        SetMode(RobotMode.Balanced);
        stationaryTimer = 0f;
        inStealthFromStationary = false;
        
        if (playerHealth != null)
        {
            playerHealth.StopRegen();
        }
    }

    void HandleMovement()
    {
        movementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (rb != null)
        {
            rb.linearVelocity = movementInput * moveSpeed;
        }
        else
        {
            Debug.LogError("Rigidbody2D is null. Cannot move the robot.");
        }
    }

    void DrainBattery()
    {
        batteryLevel -= batteryDrainRate * Time.deltaTime;
        batteryLevel = Mathf.Clamp(batteryLevel, 0, 100);

        if (batteryLevel <= 0)
        {
            baseSpeed = 0;  
            source.Stop();

            Debug.Log("Battery is empty. Robot cannot move!");
        }

        if (batteryText != null)
        {
            batteryText.text = batteryLevel <= 0 ? 
                "Battery is empty. Robot cannot move!" : 
                $"Battery: {Mathf.RoundToInt(batteryLevel)}%";
        }
    }

    public bool IsMoving()
    {
        return movementInput.magnitude > 0.1f;
    }

    public void SetMode(RobotMode mode)
    {
        if (lockMode) return;
        currentMode = mode;
        if (modeText != null)
        {
            modeText.text = $"Mode: {currentMode}";
        }

        switch (mode)
        {
            case RobotMode.Balanced:
                moveSpeed = baseSpeed;
                batteryDrainRate = 1f;
                ResetModeEffects();
                break;
                
            case RobotMode.Stealth:
                moveSpeed = baseSpeed * .7f;
                batteryDrainRate = 1.2f;
                if (weaponSystem != null) weaponSystem.damageMultiplier = stealthDamageMultiplier;
                break;
                
            case RobotMode.Combat:
                moveSpeed = baseSpeed * .8f;
                batteryDrainRate = 1.5f;
                if (weaponSystem != null) weaponSystem.damageMultiplier = combatDamageMultiplier;
                if (damageSystem != null) damageSystem.damageReduction = combatDamageReduction;
                break;
                
            case RobotMode.Speed:
                moveSpeed = baseSpeed * 3f;
                batteryDrainRate = 2f;
                if (mainCamera != null) mainCamera.orthographicSize = defaultCameraSize * speedVisionReduction;
                if (damageSystem != null) damageSystem.healthMultiplier = speedHealthReduction;
                break;
        }
    }

    private void ResetModeEffects()
    {
        if (weaponSystem != null) weaponSystem.damageMultiplier = 1f;
        if (damageSystem != null)
        {
            damageSystem.damageReduction = 1f;
            damageSystem.healthMultiplier = 1f;
        }
        if (mainCamera != null) mainCamera.orthographicSize = defaultCameraSize;
    }

    void RotateToMouse()
    {
        if (mainCamera == null) return;
        
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        
        float angle = Mathf.LerpAngle(
            transform.rotation.eulerAngles.z, 
            targetAngle, 
            rotationSpeed * Time.deltaTime
        );
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}