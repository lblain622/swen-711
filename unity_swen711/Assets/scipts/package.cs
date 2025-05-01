using UnityEngine;

public class Package : MonoBehaviour
{
    public bool isDelivered = false;
    
    public void MarkAsDelivered()
    {
        isDelivered = true;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(1);
        }
        Debug.Log("Package delivered!");
    }
}