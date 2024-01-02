using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fist : MonoBehaviour
{
    [SerializeField] Image[] crosshair;
    
    private HEF_EnemyScript wasd = null;

    [Header("MainCamera")]
    [SerializeField] GameObject mainCamera;
    [Header("CameraFists")]
    public GameObject camFist;
    [SerializeField] MeshRenderer CamFistRenderer;
    [Header("ShootingFists")]
    public GameObject ShootFist;
    public GameObject OtherShootFist;
    [SerializeField] Vector3 TempShootFistPos = new Vector3(1000f,-100f,1000f);
    [SerializeField] string AxisToShoot;//Fire1 or //Fire2
    public Rigidbody ShootFistRB;
    [SerializeField] string enemyTag="Enemy";

    private GameObject homingTarget;
    public float distance;
    public float speed=8;

    public bool HandisClipping;//Changed by the HandInWall script.
    public bool reloaded;
    public float dropTime;//time until fist reloads after hitting something
    public float flyingTime;//time until fist auto reloads after firing- so it doesnt fly forever
    float flyingCounter=0;
    float dropCounter=0;
    

    public FistState state;
    public enum FistState{
        flying,//is flying but didnt hit anything yet
        homing,
        reloaded,//when the fist is teleported away-is also ready to shoot
        falling,//after the fist hits something
    }
    // Start is called before the first frame update
    
    public float power = 1000.0F;
    void Start()
    { 
        ShootFist.transform.position = TempShootFistPos;
        distance= speed*flyingTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CrossHairControl();
        StateChecker();
        //get imput, move shoot fist to camera fist & disable camFist mesh, add force to shoot fist
        if(Input.GetAxis(AxisToShoot)!=0&&reloaded&&HandisClipping==false){
            ReplaceFist();
            Shoot();
            reloaded=false;//the fist is reloaded when it is teleported away
        }
    }
    //void ReplaceFist(Gam)
    void ReplaceFist(){//
        ShootFist.transform.position= camFist.transform.position;
        ShootFist.transform.rotation= camFist.transform.rotation;
        CamFistRenderer.enabled = false;
    }
    void DropTheFist(){
        ShootFistRB.useGravity = true;
            state = FistState.falling;
            dropCounter = dropTime;
    }
    ///Funtion ReplaceFist
    ///Parameters: none
    ///Intended action description
    void ReloadFist(){ 
        ShootFistRB.useGravity = false;
        CamFistRenderer.enabled = true;
        ShootFist.transform.position = TempShootFistPos;
        ShootFistRB.velocity= new Vector3(0f,0f,0f);
        ShootFistRB.angularVelocity= new Vector3(0f,0f,0f);
        ShootFist.transform.rotation= new Quaternion(0f,0f,0f,1);
        state = FistState.reloaded;
    }
    void OnCollisionEnter(Collision other){
        if((state==FistState.flying || state==FistState.homing))
            DoDamage(other);
        DropTheFist();
        ShootFistRB.velocity = Vector3.ClampMagnitude(ShootFistRB.velocity, ShootFistRB.velocity.magnitude*.5f);
    }
    void Shoot(){
        RaycastHit hit;
           Physics.Raycast(mainCamera.transform.position,  mainCamera.transform.forward*distance, out hit);
           //Debug.Log("Raycast Shot");
            if(hit.collider != null)
            {   
                if(hit.collider.gameObject.CompareTag(enemyTag)){
                    homingTarget=hit.collider.gameObject;
                    state=FistState.homing;
                
                }else{
                    transform.LookAt(hit.point);
                    ShootFistRB.AddForce(ShootFist.transform.forward * speed,ForceMode.Impulse);
                    state = FistState.flying;
                }
                //ShootFist.transform.forward = hit.point - ShootFist.transform.position;
            } else{
               ShootFistRB.velocity= new Vector3(0f,0f,0f);
                ShootFistRB.AddForce(ShootFist.transform.forward * speed,ForceMode.Impulse);
                state = FistState.flying;
            }
            flyingCounter= flyingTime;//starts auto reload
            
    }
    void StateChecker(){//Controls timers depending on state_____//when the state is reloaded it teleports it back
        //if the fist hit something start a timer for dropTime
        if (state==FistState.flying || state == FistState.homing){
            if(flyingCounter<=0){
            DropTheFist();
            }
            else
            flyingCounter-=Time.deltaTime;
        }
        if (state==FistState.falling){
            if(dropCounter<=0){
            ReloadFist();
            }
            else{
            dropCounter-=Time.deltaTime;
            
            }
        }//automatically reload after flyingTime secconds
        if (state==FistState.reloaded){
            reloaded=true;
        }
        if (state==FistState.homing){
            transform.LookAt(homingTarget.transform.position);
            ShootFistRB.velocity=new Vector3(0,0,0);
            ShootFistRB.AddForce(ShootFist.transform.forward * speed,ForceMode.Impulse);
        }
    }
    void CrossHairControl(){ 
        RaycastHit hit;
           Physics.Raycast(mainCamera.transform.position,  mainCamera.transform.forward*distance, out hit);
           if(hit.collider != null)
            {   
                if(hit.collider.gameObject.CompareTag(enemyTag)){
                    for (int i=0; i<crosshair.Length;i++){
                        crosshair[i].color = new Color(1f,0f,0f,1f);
                        }
                    
                }
                else{
                    for (int i=0; i<crosshair.Length;i++){
                        crosshair[i].color = new Color(1f,1f,1f,1f);
                    }
                }

            } else{
                for (int i=0; i<crosshair.Length;i++){
                        crosshair[i].color = new Color(1f,1f,1f,1f);
                }
            }
    }
    void DoDamage(Collision other){
        if (other.gameObject.CompareTag(enemyTag))
            wasd = other.gameObject.GetComponent<HEF_EnemyScript>();
            if (wasd != null){
                wasd.EnemyHealth-=1;
                wasd=null;
            } 
    }
}
