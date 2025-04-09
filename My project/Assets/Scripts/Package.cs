[System.Serializable]
public class Package
{
    public string id;
    public Mailbox destination;
    public PackageType type;
    public Sprite icon;
}

public enum PackageType { Standard, Fragile, Express }