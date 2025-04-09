public class PackageUI : MonoBehaviour
{
    public Image packageIcon;
    public Image destinationIcon;
    public TextMeshProUGUI destinationText;

    public void Initialize(Package package)
    {
        packageIcon.sprite = package.icon;
        destinationIcon.sprite = package.destination.icon;
        destinationText.text = package.destination.mailboxID;
        
        // Color coding by type
        packageIcon.color = GetPackageColor(package.type);
    }

    Color GetPackageColor(PackageType type)
    {
        switch(type)
        {
            case PackageType.Fragile: return Color.red;
            case PackageType.Express: return Color.green;
            default: return Color.white;
        }
    }
}