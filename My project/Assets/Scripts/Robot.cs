using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Robot : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float rotationOffset = -90f; // Adjust for sprite facing direction

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Camera mainCamera;
    public Transform packageSpawnPoint;
    public GameObject[] packageModels;
    private List<Package> carriedPackages = new List<Package>();
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetupRigidbody();
        
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogError("Main camera not found in scene!");
    }

    void SetupRigidbody()
    {
        rb.gravityScale = 0f; // Disable gravity
        rb.linearDamping = 0f; // No linear drag
        rb.angularDamping = 0f; // No rotational drag
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent physics rotation
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        GetInput();
        RotateToMouse();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Mailbox") && Input.GetKey(KeyCode.Space))
        {
            other.GetComponent<Mailbox>().AttemptDelivery(this);
        }
    }
    public void OnDeliveryComplete()
    {
        // Remove package visual
        UpdateCarriedPackages();
    }

    void UpdateCarriedPackages()
    {
        // Clear existing package visuals
        foreach (Transform child in packageSpawnPoint)
            Destroy(child.gameObject);

        // Create new visuals
        foreach (var pkg in carriedPackages)
        {
            var model = Instantiate(GetModelForType(pkg.type), packageSpawnPoint);
            model.transform.localPosition = Vector3.zero;
        }
    }

    GameObject GetModelForType(PackageType type)
    {
        return packageModels[(int)type];
    }
    void FixedUpdate()
    {
        MoveCharacter();
    }

    void GetInput()
    {
        movementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;
    }

    void MoveCharacter()
    {
        rb.linearVelocity = movementInput * moveSpeed;
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