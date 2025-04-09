using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Delivery System")]
    public List<Package> allPackages = new List<Package>();
    public List<Mailbox> allMailboxes = new List<Mailbox>();
    public int packagesPerLevel = 5;
    
    [Header("UI")]
    public PackageUI packageUIPrefab;
    public Transform packageListParent;
    
    private List<Package> currentPackages = new List<Package>();
    private Dictionary<Mailbox, List<Package>> mailboxAssignments = new Dictionary<Mailbox, List<Package>>();

    void Awake()
    {
        Instance = this;
        InitializeDeliverySystem();
    }

    void InitializeDeliverySystem()
    {
        // Clear previous assignments
        mailboxAssignments.Clear();
        foreach (var mb in allMailboxes)
        {
            mailboxAssignments.Add(mb, new List<Package>());
            mb.AssignMailbox(mb); // Initialize mailbox ID
        }

        // Assign random packages
        currentPackages = GetRandomPackages(packagesPerLevel);
        UpdatePackageUI();
    }

    List<Package> GetRandomPackages(int count)
    {
        List<Package> result = new List<Package>();
        
        // Ensure at least one package per mailbox
        foreach (var mb in allMailboxes)
        {
            var pkg = new Package() {
                id = System.Guid.NewGuid().ToString(),
                destination = mb,
                type = (PackageType)Random.Range(0,3)
            };
            result.Add(pkg);
            mailboxAssignments[mb].Add(pkg);
            count--;
        }

        // Add remaining random packages
        while (count-- > 0)
        {
            var randomMB = allMailboxes[Random.Range(0, allMailboxes.Count)];
            var pkg = new Package() {
                id = System.Guid.NewGuid().ToString(),
                destination = randomMB,
                type = (PackageType)Random.Range(0,3)
            };
            result.Add(pkg);
            mailboxAssignments[randomMB].Add(pkg);
        }

        return result;
    }

    public void DeliverPackages(Mailbox mailbox)
    {
        if (!mailboxAssignments.ContainsKey(mailbox)) return;

        foreach (var pkg in mailboxAssignments[mailbox])
        {
            currentPackages.Remove(pkg);
            // Add score based on package type
        }

        UpdatePackageUI();
        
        if (currentPackages.Count == 0)
        {
            Debug.Log("All packages delivered!");
        }
    }

    void UpdatePackageUI()
    {
        // Clear existing UI
        foreach (Transform child in packageListParent)
            Destroy(child.gameObject);

        // Create new UI elements
        foreach (var pkg in currentPackages)
        {
            var ui = Instantiate(packageUIPrefab, packageListParent);
            ui.Initialize(pkg);
        }
    }
}