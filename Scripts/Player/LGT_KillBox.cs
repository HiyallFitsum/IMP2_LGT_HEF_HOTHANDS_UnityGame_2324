using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LGT_KillBox : MonoBehaviour
{
    
    [SerializeField] CheckPoint checkPointComponent;
    void Update()
    {
    }
    void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("Player")){
            checkPointComponent.playerDead= true;
        }
        else if (!other.gameObject.CompareTag("Fist")&&!other.gameObject.CompareTag("MainCamera") ){
            Debug.Log("destroy: "+ other);
            Destroy(other.gameObject);
        }
    }
}
