using UnityEngine;
using TMPro;
public enum RobotMode { Balanced, Stealth, Combat, Speed }

public class RobotController : MonoBehaviour
{
    public TextMeshProUGUI modeText;
    private Package currentPackage;
    public Transform packageCarryPoint;  // Where the package will appear in front of the robot
    public float baseSpeed = 5f;
    public float batteryLevel = 100f;
    public float batteryDrainRate = 1f;
    private Vector2 movementInput;
    public RobotMode currentMode = RobotMode.Balanced;
    private Rigidbody2D rb;
    private Camera mainCamera;
    public float rotationSpeed = 10f;
    public float rotationOffset = -90f; // Adjust for sprite facing direction

    private float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on this GameObject.");
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
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryPickUpPackage();
        }

        // Package delivery when close to mailbox
        if (currentPackage != null && Vector2.Distance(transform.position, currentPackage.transform.position) < 1f)
        {
            TryDeliverPackage();
        }

        // Switch modes
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetMode(RobotMode.Balanced);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetMode(RobotMode.Stealth);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetMode(RobotMode.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetMode(RobotMode.Speed);
    }

    void TryPickUpPackage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Package"));
        foreach (Collider2D hit in hits)
        {
            Package package = hit.GetComponent<Package>();
            if (package != null && !package.isDelivered)
            {
                PickUpPackage(package);
                break;
            }
        }

    }


    void PickUpPackage(Package package)
    {
        if (currentPackage == null)
        {
            currentPackage = package;
            if (packageCarryPoint != null)
            {
                package.transform.position = packageCarryPoint.position;  // Attach the package to the robot
                package.transform.parent = packageCarryPoint;  // Make it follow the robot's position
                package.gameObject.SetActive(true);  // Ensure package is active when picked up
                Debug.Log("Picked up the package.");
            }
            else
            {
                Debug.LogError("PackageCarryPoint is not assigned!");
            }
        }
    }

    void TryDeliverPackage()
    {
        if (currentPackage != null)
        {
            // Assuming there is a mailbox in the scene tagged as "Mailbox"
            Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Mailbox"));
            if (hit != null)
            {
                currentPackage.MarkAsDelivered();  // Mark the package as delivered
                currentPackage = null;  // Remove the reference to the package
                Debug.Log("Package delivered to mailbox.");
            }
            else
            {
                Debug.LogError("No mailbox detected nearby.");
            }
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

        // If a package is carried, ensure it follows the robot
        if (currentPackage != null && packageCarryPoint != null)
        {
            currentPackage.transform.position = packageCarryPoint.position;
        }
    }

    void DrainBattery()
    {
        batteryLevel -= batteryDrainRate * Time.deltaTime;
        batteryLevel = Mathf.Clamp(batteryLevel, 0, 100);

        if (batteryLevel <= 0)
        {
            moveSpeed = 0;  // Stop movement when battery is empty
            Debug.Log("Battery is empty. Robot cannot move!");
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
                break;
            case RobotMode.Stealth:
                moveSpeed = baseSpeed * 0.5f;
                batteryDrainRate = 0.5f;
                break;
            case RobotMode.Combat:
                moveSpeed = baseSpeed * 0.8f;
                batteryDrainRate = 1.5f;
                break;
            case RobotMode.Speed:
                moveSpeed = baseSpeed * 1.5f;
                batteryDrainRate = 2f;
                break;
        }
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
