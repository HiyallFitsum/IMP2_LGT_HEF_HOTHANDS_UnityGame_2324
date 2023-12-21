using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEF_EnemyIdle : MonoBehaviour
{
    public float walkSpeed = 2.0f;
    public float idleDuration = 2.0f;
    public float walkDuration = 4.0f;
    public float walkRange = 10.0f;

    private Vector3 targetPosition;
    private float stateTimer = 0.0f;
    private bool isWalking = false;

    public GameObject enemy;

    private void Start()
    {
        // Start in the idle state
        isWalking = false;
        SetRandomTargetPosition();
    }

    private void Update()
    {
        stateTimer += Time.deltaTime;

        if (isWalking)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isWalking = false;
                stateTimer = 0.0f;
                StartCoroutine(IdleState());
            }
        }
        else
        {
            if (stateTimer >= idleDuration)
            {
                // Switch to walking state
                isWalking = true;
                stateTimer = 0.0f;
                SetRandomTargetPosition();
            }
        }
    }

    private void SetRandomTargetPosition()
    {
        // Generate a random position
        targetPosition = new Vector3(Random.Range(-walkRange, walkRange), transform.position.y, Random.Range(-walkRange, walkRange));

    }

    private IEnumerator IdleState()
    {
        yield return new WaitForSeconds(idleDuration);

        isWalking = true;
        SetRandomTargetPosition();
    }
}
