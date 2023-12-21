using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEF_EnemyScript : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform player;
    public Transform enemy;
    public GameObject Enemy;

    [SerializeField] float coolDown = 3.01f;
    private bool canShoot = true;
    private bool castWasInitiated = false;
    private float timer = 0.0f;

    public float sphereCastLength = 10.0f;

    [SerializeField] float health, maxHealth = 3f;
    [SerializeField] HEF_EnemyHealthBar healthBar;
    [SerializeField] Fist LFistScript;
    [SerializeField] Fist RFistScript;

    public int EnemyHealth = 4;

    private void Awake(){
        //healthBar = enemyHealthBar.GetComponent<HEF_EnemyHealthBar>();
    }
    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    private void FixedUpdate()
    {
        //EnemyShootTrigger();
        timer += Time.deltaTime;
        if (timer >= coolDown)
        {
            canShoot = true;
            timer = 0.0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sphereCastLength);
    }

    private void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("Fist")){
            //EnemyHealth -= 1;
            //TakeDamage(EnemyHealth);
        }

    }

    void OnTriggerStay(Collider col)
    {
         enemy.LookAt(player.position);
            if (canShoot)
            {
                if (BulletPrefab != null)
                {
                    GameObject clone = Instantiate(BulletPrefab, transform.position, transform.rotation);
                    HEF_HomingBullet bullet = clone.GetComponent<HEF_HomingBullet>();
                    bullet.target = player; // Set the player as the target
                    Debug.Log("Its shooting");
                    canShoot = false;
                    // StartCoroutine(ResetCooldown());
                }
            }
    }

    void EnemyShootTrigger(){
        RaycastHit hit;
        LayerMask targetPlayer = 1 << 8;

    //public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);//
    //Physics.SphereCast(transform.position, sphereCastLength, Vector3.zero, out hit, 0f, targetPlayer)    
        if(Physics.SphereCast(transform.position, sphereCastLength, Vector3.zero, out hit, 0f, targetPlayer))
        {
            Debug.Log("i'm here");
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Fist"))
            {
                castWasInitiated = true;
                Debug.Log(hit.collider);
                Debug.Log("We gotta hit");
            }
            else
            {
                castWasInitiated = false;
            }
        }

        if (castWasInitiated)
        {
            enemy.LookAt(player.position);
            if (canShoot)
            {
                if (BulletPrefab != null)
                {
                    GameObject clone = Instantiate(BulletPrefab, transform.position, transform.rotation);
                    HEF_HomingBullet bullet = clone.GetComponent<HEF_HomingBullet>();
                    bullet.target = player; // Set the player as the target
                    Debug.Log("Its shooting");
                    canShoot = false;
                    // StartCoroutine(ResetCooldown());
                }
            }
        }

        timer += Time.deltaTime;
        if (timer >= coolDown)
        {
            canShoot = true;
            timer = 0.0f;
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
