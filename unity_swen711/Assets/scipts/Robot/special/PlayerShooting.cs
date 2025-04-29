using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab to instantiate
    public Transform gunBarrel; // The point from which the bullet will be fired

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to fire
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        // Instantiate the bullet at the gun barrel's position, facing the same direction
        GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
        
        // Activate the bullet (make it visible)
        bullet.GetComponent<Bullet>().ActivateBullet();
    }
}