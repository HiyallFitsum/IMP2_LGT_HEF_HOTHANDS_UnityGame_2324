using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEF_EnemyTwoMovement : MonoBehaviour
{
    public float dashSpeed = 5f;
    public float rotationSpeed = 5f;
    public float armRotationSpeed = 180f; // Rotation speed for the arms
    public float breakTime = 1f;
    public Transform leftArm;
    public Transform rightArm;

    private Transform player;
    private bool isDashing = false;
    private float armRotationOffset;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(DashRoutine());
    }

    void Update()
    {
        if (isDashing)
        {
            RotateTowardsPlayer();
            MoveTowardsPlayer();
            RotateArms();
        }
        else
        {
            RotateArms();
        }
    }

    IEnumerator DashRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(breakTime);
            isDashing = true;
            yield return new WaitForSeconds(0.1f); // adjust the time the enemy looks menacing
            isDashing = false;
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, -0.3f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void MoveTowardsPlayer()
    {
        transform.position += transform.forward * dashSpeed * Time.deltaTime;
    }

    void RotateArms()
    {
        float rotationAmount = Mathf.Sin(Time.time * 3f) * 30f; // Adjust the rotation speed and amplitude
        armRotationOffset += armRotationSpeed * Time.deltaTime;
        leftArm.localRotation = Quaternion.Euler(armRotationOffset, 0f, rotationAmount);
        rightArm.localRotation = Quaternion.Euler(armRotationOffset, 0f, -rotationAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DashRoutine());
        }
    }
}
