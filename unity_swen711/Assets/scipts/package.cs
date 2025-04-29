using UnityEngine;

public class Package : MonoBehaviour
{
    public bool isDelivered = false;  


    public void MarkAsDelivered()
    {
        isDelivered = true;
        gameObject.SetActive(false);  
        Debug.Log("Package Delivered!");
    }
}