using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEF_EnemyTwoMovement : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float regularSpeed = 4f;
    public float rotationSpeed = 5f;
    public float armRotationSpeed = 180f; // Rotation speed for the arms
    public float breakTime = 1f;
    public Transform leftArm;
    public Transform rightArm;
    private int LookAtOnce = 0;

    private Transform player;
    [SerializeField] bool isDashing = false;
    private float armRotationOffset;

    public Rigidbody rb;

    private float timer = 0f;
    public float coolDown = 10f;

    private Transform posToLook;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
    }
    void MoveTowardsPlayer()
    {
        rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
        isDashing = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer += Time.deltaTime;
            if (timer > (coolDown/2))
            {
                rb.velocity = new Vector3(0, 0, 0);
            }
            if (timer > coolDown)
            {
                if(LookAtOnce == 0){
                    posToLook = player.transform;
                    transform.LookAt(posToLook);
                }
                isDashing = true;
                MoveTowardsPlayer();
                timer = 0.0f;
                LookAtOnce += 1;
            } else {
                LookAtOnce = 0;
                isDashing = false;
            }
        } else{
            isDashing = false;
        }
    }
}
