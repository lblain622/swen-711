using System;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed;
    private float distance;

    void Start()
    {
        
    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) *Mathf.Rad2Deg;

        transform.position =
            Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward*angle);
    }
        
}
