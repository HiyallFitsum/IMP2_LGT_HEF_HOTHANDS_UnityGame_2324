using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEF_HomingBullet : MonoBehaviour
{
    public Transform target; // The player's transform
    public float speed = 5.0f;
    public float rotateSpeed = 5.0f;
    //public float theHealth = 3f;

    //public GameObject Enemy;

    private void Awake(){
        //target = GameObject.FindWithTag("Player").transform;
        //enemy = GetComponentInChildren<HEF_EnemyScript>();
    }

    void Start(){
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destroy the bullet if there's no target
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(rotateDirection);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the bullet when it collides with the player
            //theHealth -= 1;
            //enemy.TakeDamage(theHealth);
        }
    }
}
