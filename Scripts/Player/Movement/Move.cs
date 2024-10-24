using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{   
    private Camera mainCamera;
    public float fovMinimum = 85.0f;
    float fovSpeedRatio = 80f;
    float targetFov;
    
    private float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;

    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;
    bool readyToJump;
    [SerializeField] float fallMultiplier=2.5f;
    [SerializeField] float lowJumpMultiplier=2f;

    [SerializeField] Transform orientation;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;

    float playerHeight=2;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] bool isGrounded;
    [SerializeField] float groundCastRad;
    RaycastHit hit;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    public MovementState state;
    public enum MovementState{
        walking,
        sprinting,
        air,
        sprintAir,
    }
    // Start is called before the first frame update
    void Start()
    {
        
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump= true;
    }

    // Update is called once per frame
    private void Update(){
        
        isGrounded = Physics.SphereCast(transform.position,groundCastRad,Vector3.down,out hit, playerHeight/2, whatIsGround);
    //isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight*0.5f + 0.2f, whatIsGround);
    //Debug.DrawRay(transform.position, new Vector3(0f,-1f*playerHeight*0.5f-0.2f,0f),Color.blue,10);
        
        
        StateHandler();
        FovController();

        //handle drag
        if(isGrounded){
            rb.drag = groundDrag;
        }
        else{
            rb.drag = airDrag;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
        MyInput();
        SpeedControl();
        BetterJump();
    }
    private void MyInput(){
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && isGrounded){
            readyToJump= false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void MovePlayer(){
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if(isGrounded)
        rb.AddForce(moveDirection.normalized * moveSpeed*7f, ForceMode.Force);

        else if(state==MovementState.sprintAir)
            rb.AddForce(moveDirection.normalized * moveSpeed*7f*airMultiplier, ForceMode.Force);
        else if(!isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed*7f*airMultiplier, ForceMode.Force);
    }   

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x,0f, rb.velocity.z);
        
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    private void FovController(){
        Vector3 totalVel = new Vector3(rb.velocity.x,0f, rb.velocity.z);
        
        mainCamera.fieldOfView = Mathf.MoveTowards(mainCamera.fieldOfView, targetFov, (walkSpeed+totalVel.magnitude) * Time.deltaTime);
    }
    private void StateHandler()
    {   
        Vector3 totalVel = new Vector3(rb.velocity.x,0, rb.velocity.z);
        //sprinting
        if(isGrounded && Input.GetKey(sprintKey)){
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            targetFov = fovMinimum*(1f+((totalVel.magnitude-walkSpeed)/fovSpeedRatio));
        }
        else if(isGrounded){
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            targetFov = fovMinimum;
        }
        else if(Input.GetKey(sprintKey)){
            state=MovementState.sprintAir;
            targetFov = fovMinimum*(1f+((totalVel.magnitude)/fovSpeedRatio));
        } 
        else{
        state=MovementState.air;
        targetFov = fovMinimum*(1f+((totalVel.magnitude)/fovSpeedRatio));
        }
    }

    private void Jump(){
        //reset y velocity
        rb.velocity= new Vector3(rb.velocity.x,0f,rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump(){
        readyToJump=true;
    }
    private void BetterJump(){
        if (rb.velocity.y<0){
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier-1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetKey(jumpKey)){
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier -1) * Time.deltaTime;
        }
    }

}
