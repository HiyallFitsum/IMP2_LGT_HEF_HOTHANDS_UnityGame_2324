using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEF_HenchmanMovement : MonoBehaviour
{
    public Transform henchman;
    public Transform henchmanPos;
    public float duration = 30.0f;
    public float startTime;
    public Transform distance;
    public float t = 0;
    public bool Moving = false;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        t = (Time.time - startTime) / duration;
        henchman.position =  Vector3.SmoothDamp(henchman.position, henchmanPos.position, ref velocity, duration);
        // if(Moving){
        //     duration = 30.0f;
        //     Debug.Log("Henchman is not colliding (SmoothStep) ");
        //     henchman.position = new Vector3(Mathf.Slerp(henchman.position.x, henchmanPos.position.x, t), Mathf.SmoothStep(henchman.position.y, henchmanPos.position.y, t), Mathf.SmoothStep(henchman.position.z, henchmanPos.position.z, t));
        // }

    }

    void OnTriggerEnter(Collider other){
        // if(other.gameObject.CompareTag("Henchman")){
        //     Debug.Log("Henchman is in collider (MoveTowards) ");
        //     Moving = false;
        //     duration = 1000.0f;
        //     henchman.position = new Vector3(Mathf.MoveTowards(henchman.position.x, henchmanPos.position.x, t), Mathf.MoveTowards(henchman.position.y, henchmanPos.position.y, t), Mathf.MoveTowards(henchman.position.z, henchmanPos.position.z, t));     
        // }
        // else{
        //     Moving = true;
        // }
    }
        
}
