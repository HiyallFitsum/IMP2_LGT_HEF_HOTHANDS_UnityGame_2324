using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEF_HomingBullet : MonoBehaviour
{
    public Transform target;
    public bool toPlayer = false;
    public bool bomb = false; 
    public float speed = 5.0f;
    public float bombSpeed = 5.0f;
    public float rotateSpeed = 5.0f;
    private float theHealth = 3f;
    public Transform enemy;
    public Transform player;
    private HEF_EnemyScript wasd = null;

    //public GameObject Enemy;

    private void Awake(){
        player = GameObject.FindWithTag("Player").transform;
        enemy = GameObject.FindWithTag("Enemy").transform;
    }

    void Start(){
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); 
            return;
        }

        if(bomb == false){
            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(rotateDirection);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }else{
            transform.position = Vector3.Slerp(transform.position, target.position, bombSpeed);
            Debug.Log("Saay aayyy");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && toPlayer)
        {
            Destroy(gameObject); // Destroy the bullet when it collides with the player
        }else if(other.gameObject.CompareTag("Enemy") && target == enemy){
            Destroy(gameObject);
            wasd = other.gameObject.GetComponent<HEF_EnemyScript>();
            if (wasd != null){
                theHealth -= 1;
                wasd.TakeDamage(theHealth);
                wasd=null;
            }
        }
    }
}
