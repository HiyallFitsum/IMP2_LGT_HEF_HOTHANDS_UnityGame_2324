using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEF_HenchmanAssist : MonoBehaviour
{
    public Transform enemy;
    [SerializeField] float coolDown = 3.01f;
    private bool canShoot = true;
    private float timer = 0.0f;
    public GameObject BulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootBullets();
        //EnemyShootTrigger();
        timer += Time.deltaTime;
        if (timer >= coolDown)
        {
            canShoot = true;
            timer = 0.0f;
        }
    }

    void shootBullets(){
        if (canShoot)
        {
            if (BulletPrefab != null)
            {
                GameObject clone = Instantiate(BulletPrefab, transform.position, transform.rotation);
                HEF_HomingBullet bullet = clone.GetComponent<HEF_HomingBullet>();
                bullet.target = enemy; // Set the player as the target
                canShoot = false;
                // StartCoroutine(ResetCooldown());
            }
        }
    }
}
