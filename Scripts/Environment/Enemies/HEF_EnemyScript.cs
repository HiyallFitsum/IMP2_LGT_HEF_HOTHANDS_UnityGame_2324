using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEF_EnemyScript : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform player;
    private Transform enemy;
    public GameObject Enemy;
    public bool enemyTwo = false;

    [SerializeField] float coolDown = 3.01f;
    private bool canShoot = true;
    private float timer = 0.0f;

    [SerializeField] float health, maxHealth = 4f;
    [SerializeField] HEF_EnemyHealthBar healthBar;

    public float EnemyHealth = 4f;

    private void Awake(){
        enemy = GameObject.Find("EnemyOne - Shooter").transform;
    }
    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= coolDown)
        {
            canShoot = true;
            timer = 0.0f;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(enemyTwo == false){
            enemy.LookAt(player.position);
                if (canShoot)
                {
                    if (BulletPrefab != null)
                    {
                        GameObject clone = Instantiate(BulletPrefab, transform.position, transform.rotation);
                        HEF_HomingBullet bullet = clone.GetComponent<HEF_HomingBullet>();
                        bullet.target = player;
                        bullet.toPlayer = true;
                        Debug.Log("Its shooting");
                        canShoot = false;
                    }
                }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health = damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(Enemy);
    }
}
