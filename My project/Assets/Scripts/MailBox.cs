public class Mailbox : MonoBehaviour
{
    [Header("Identification")]
    public string mailboxID;
    public Color highlightColor = Color.yellow;
    
    [Header("Delivery")]
    public float holdDuration = 1f;
    public GameObject deliveryPrompt;

    private Material originalMaterial;
    private bool hasPackagesAssigned;

    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
        deliveryPrompt.SetActive(false);
    }

    public void AssignMailbox(Mailbox self)
    {
        mailboxID = "MB_" + self.GetInstanceID();
        GameManager.Instance.mailboxAssignments.Add(this, new List<Package>());
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        hasPackagesAssigned = CheckForAssignedPackages();
        deliveryPrompt.SetActive(hasPackagesAssigned);
        HighlightMailbox(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        deliveryPrompt.SetActive(false);
        HighlightMailbox(false);
    }

    bool CheckForAssignedPackages()
    {
        return GameManager.Instance.mailboxAssignments[this].Count > 0;
    }

    void HighlightMailbox(bool enable)
    {
        GetComponent<Renderer>().material.color = enable ? highlightColor : originalMaterial.color;
    }

    public void AttemptDelivery(Robot robot)
    {
        if (!hasPackagesAssigned) return;
        
        StartCoroutine(DeliveryCoroutine(robot));
    }

    IEnumerator DeliveryCoroutine(Robot robot)
    {
        float timer = 0;
        while (timer < holdDuration)
        {
            if (!Input.GetKey(KeyCode.Space)) yield break;
            
            timer += Time.deltaTime;
            // Update progress UI here
            yield return null;
        }
        
        GameManager.Instance.DeliverPackages(this);
        robot.OnDeliveryComplete();
    }
}