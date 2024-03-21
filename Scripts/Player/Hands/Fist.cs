using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Fist : MonoBehaviour
{
    [SerializeField] Image[] crosshair;
    
    private HEF_EnemyScript enemyScript = null;
    private LGT_TargetScript targetScript = null;

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

    [SerializeField] Rigidbody playerRb;
    public float explosionRadius = 5.0F;
    public float explosionPower = 10.0F;
    
    [SerializeField] GameObject particles;
    [SerializeField] GameObject particlesTwo;

    public FistState state;
    public enum FistState{
        flying,//is flying but didnt hit anything yet
        homing,
        reloaded,//when the fist is teleported away-is also ready to shoot
        falling,//after the fist hits something
    }
    // Start is called before the first frame update
    
    public float power = 1000.0F;
    private float timer = 0.0f;

    public bool shootingLeft = false;
    public bool shootingRight = false;

    //private GameObject audioManager;
    HEF_AudioManager audioManager; 

    private void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<HEF_AudioManager>();
    }

    void Start()
    { 
        ShootFist.transform.position = TempShootFistPos;
        distance= (speed*flyingTime)*1.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CrossHairControl();
        StateChecker();
        //get imput, move shoot fist to camera fist & disable camFist mesh, add force to shoot fist
        if(AxisToShoot == "Fire1"){
            if(shootingLeft&&reloaded&&HandisClipping==false){
                ReplaceFist();
                Shoot();
                reloaded=false;//the fist is reloaded when it is teleported away
            }
        }else if(AxisToShoot == "Fire2"){
            if(shootingRight&&reloaded&&HandisClipping==false){
                ReplaceFist();
                Shoot();
                reloaded=false;//the fist is reloaded when it is teleported away
            }
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
        audioManager.PlaySFX(audioManager.FistExplosion);
    }
    void Shoot(){
        //audioManager.PlaySFX(audioManager.FistShoot);

        RaycastHit hit;
           Physics.Raycast(mainCamera.transform.position,  mainCamera.transform.forward, out hit, distance);
           
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
           Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, distance);
           if(hit.collider != null)
            {   
                if(hit.collider.gameObject.CompareTag(enemyTag)){
                    for (int i=0; i<crosshair.Length;i++){
                        crosshair[i].color = new Color(1f,0f,0f,1f);
                        }
                    
                }else if(hit.collider.gameObject.CompareTag("Bullet")){
                    for (int i=0; i<crosshair.Length;i++){
                        crosshair[i].color = new Color(0.192f,0.875f,1f,1f);
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
            enemyScript = other.gameObject.GetComponent<HEF_EnemyScript>();
            if(enemyScript == null)
            {
                targetScript = other.gameObject.GetComponent<LGT_TargetScript>();
                if(targetScript!= null){
                    targetScript.EnemyHealth-=1;
                    targetScript=null;
                }
            } else if(enemyScript != null){
                enemyScript.EnemyHealth-=1;
                enemyScript=null;
            }   
        if(other.gameObject.CompareTag("Bullet"))
        {
            if((state==FistState.flying || state==FistState.homing)){
                Destroy(other.gameObject);
            }
            Vector3 explosionPos = transform.position;
            playerRb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F);
            Instantiate(particlesTwo, explosionPos, transform.rotation);
        }else{
            Vector3 explosionPos = transform.position;
            playerRb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F);
            Instantiate(particles, explosionPos, transform.rotation);
        }
 
    }

    public void OnShootLeftFist(InputAction.CallbackContext ctx){
        if(ctx.phase == InputActionPhase.Started){
            shootingLeft = true;
        }else if(ctx.phase == InputActionPhase.Canceled){
            shootingLeft = false;
        }
    }

    public void OnShootRightFist(InputAction.CallbackContext ctx){
        if(ctx.phase == InputActionPhase.Started){
            shootingRight = true;
        }else if(ctx.phase == InputActionPhase.Canceled){
            shootingRight = false;
        }
    }


    
}
