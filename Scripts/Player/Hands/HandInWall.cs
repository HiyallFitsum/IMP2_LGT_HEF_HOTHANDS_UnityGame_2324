using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HandInWall : MonoBehaviour
{
    [SerializeField] bool fistIsTouching;
    [SerializeField] Fist FistScript;//this has to be the script on the corresponding shoot fist
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider other){
        //Debug.Log("hand is Colliding with: " + other.gameObject.name);
        if (!other.CompareTag("Enemy"))
        FistScript.HandisClipping=true;
    }
    void OnCollisionStay(Collision other){
        //Debug.Log("hand is Colliding with: " + other.gameObject.name);
        if (!other.gameObject.CompareTag("Enemy"))
        FistScript.HandisClipping=true;
    }
    void OnTriggerExit(Collider other){
        if (!other.CompareTag("Enemy"))
        FistScript.HandisClipping=false;
    }
    void OnCollisionExit(Collision other){
        if (!other.gameObject.CompareTag("Enemy"))
        FistScript.HandisClipping=false;
    }
}
