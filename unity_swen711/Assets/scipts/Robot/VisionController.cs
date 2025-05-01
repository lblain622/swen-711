using UnityEngine;

public class VisionController : MonoBehaviour
{
    public GameObject visionMaskPrefab;
    public float stealthVisionMultiplier = 1.5f;
    
    private GameObject visionMask;
    private float baseVision;
    private RobotController robotController;

    void Start()
    {
        robotController = GetComponent<RobotController>();
        CreateVisionMask();
    }

    void CreateVisionMask()
    {
        if (visionMaskPrefab == null) return;
        
        visionMask = Instantiate(visionMaskPrefab, transform.position, Quaternion.identity);
        visionMask.transform.SetParent(transform);
        baseVision = visionMask.transform.localScale.x;
        UpdateVision();
    }

    void Update()
    {
        UpdateVision();
    }

    void UpdateVision()
    {
        if (visionMask == null || robotController == null) return;

        float visionScale = baseVision;
        
        if (robotController.inStealthFromStationary)
        {
            visionScale *= stealthVisionMultiplier;
        }

        visionMask.transform.localScale = new Vector3(visionScale, visionScale, 1f);
    }
}