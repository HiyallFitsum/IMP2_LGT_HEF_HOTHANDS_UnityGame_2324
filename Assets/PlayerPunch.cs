using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Fist1;
    public Animator camAnim;
    public Animator fistAnim;

    private bool canPunch = true;  

    void Start () {
        camAnim = Camera.GetComponent<Animator>();
        fist1Anim = Fist1.GetComponent<Animator>();
    }
    
    void Update()
    {
        if(Input.GetKeyDown("1") && canPunch){
            StartCoroutine(PlayAnimationsAndWait());
        }
    }

    IEnumerator PlayAnimationsAndWait()
    {
        canPunch = false;  
        camAnim.Play("Camera_Punch", -1, 0f);  
        fist1Anim.Play("Punch", -1, 0f);  

        yield return new WaitForSeconds(0.67f);  
        canPunch = true;  
    }
}
