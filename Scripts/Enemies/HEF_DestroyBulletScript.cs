using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEF_DestroyBulletScript : MonoBehaviour
{
    [SerializeField] float bulletLifetime = 5.0f;

    private void Start()
    {
        // Start a coroutine to destroy the bullet after a certain time
        StartCoroutine(DestroyAfterDelay());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Fist"))
        {
            // Destroy the bullet when it collides with the player
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        // Wait for the specified lifetime
        yield return new WaitForSeconds(bulletLifetime);

        // Destroy the bullet after the specified lifetime
        Destroy(gameObject);
    }
}
