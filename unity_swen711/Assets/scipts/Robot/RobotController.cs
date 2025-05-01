using UnityEngine;
using TMPro;
public enum RobotMode { Balanced, Stealth, Combat, Speed }

public class RobotController : MonoBehaviour
{
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI batteryText;
    private Package currentPackage;
    public float maxBattery = 100f;
    public float baseSpeed = 5f;
    public float batteryLevel = 100f;
    public float batteryDrainRate = 1f;
    private Vector2 movementInput;
    public RobotMode currentMode = RobotMode.Balanced;
    private Rigidbody2D rb;
    private Camera mainCamera;
    public float rotationSpeed = 10f;
    public float rotationOffset = -90f; // Adjust for sprite facing direction
    public float combatDamageMultiplier = 1.5f;
    public float combatDamageReduction = 0.7f;
    public float stealthDamageMultiplier = 0.7f;
    public float stealthDetectionRange = 3f;
    public float speedVisionReduction = 0.6f;
    public float speedHealthReduction = 0.7f;
    private WeaponSystem weaponSystem;
    private DamageSystem damageSystem;
    private float defaultCameraSize;
    private float moveSpeed;

    void Start()
    {
        weaponSystem = GetComponent<WeaponSystem>();
        damageSystem = GetComponent<DamageSystem>();
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on this GameObject.");
        }
        if (mainCamera != null)
        {
            defaultCameraSize = mainCamera.orthographicSize;
        }

        moveSpeed = baseSpeed;
    }

    void Awake()
    {
        SetupRigidbody();
        
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in scene!");
        }
    }

    void SetupRigidbody()
    {
        if (rb != null)
        {
            rb.gravityScale = 0f; // Disable gravity
            rb.linearDamping = 0f; // No linear drag
            rb.angularDamping = 0f; // No rotational drag
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent physics rotation
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    void Update()
    {
        HandleMovement();
        RotateToMouse();
        DrainBattery();
   

        // Switch modes Override
        //if (Input.GetKeyDown(KeyCode.Alpha1)) SetMode(RobotMode.Balanced);
        //if (Input.GetKeyDown(KeyCode.Alpha2)) SetMode(RobotMode.Stealth);
        //if (Input.GetKeyDown(KeyCode.Alpha3)) SetMode(RobotMode.Combat);
        //if (Input.GetKeyDown(KeyCode.Alpha4)) SetMode(RobotMode.Speed);
    }
    public void ResetRobot()
    {
        // Reset battery
        batteryLevel = maxBattery;
        
        // Reset mode
        SetMode(RobotMode.Balanced);
        
        
        // Reset position (optional)
        // transform.position = Vector3.zero;
        

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
            moveSpeed = 0;  
            Debug.Log("Battery is empty. Robot cannot move!");
        }

        if ( batteryText != null)
        {

            if (batteryLevel <= 0)
            {
                batteryText.text = "Battery is empty. Robot cannot move!";
            }
            else
            {
                batteryText.text = "Battery: " + Mathf.RoundToInt(batteryLevel) + "%";
            }
        }
    }

    public void SetMode(RobotMode mode)
    {
        currentMode = mode;

        // Update the mode text
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
                moveSpeed = baseSpeed * 1.2f; 
                batteryDrainRate = 0.8f;
                if (weaponSystem != null) weaponSystem.damageMultiplier = stealthDamageMultiplier;
                break;
            case RobotMode.Combat:
                moveSpeed = baseSpeed * 0.8f;
                batteryDrainRate = 1.5f;
                if (weaponSystem != null) weaponSystem.damageMultiplier = combatDamageMultiplier;
                if (damageSystem != null) damageSystem.damageReduction = combatDamageReduction;
                break;
            case RobotMode.Speed:
                moveSpeed = baseSpeed * 1.8f;
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
        
        // Smooth rotation
        float angle = Mathf.LerpAngle(
            transform.rotation.eulerAngles.z, 
            targetAngle, 
            rotationSpeed * Time.deltaTime
        );
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
