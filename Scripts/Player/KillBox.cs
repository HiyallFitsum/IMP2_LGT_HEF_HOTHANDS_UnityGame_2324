using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    [SerializeField] float killBoxTopYValue;
    [SerializeField] CheckPoint checkPointComponent;

    void Update()
    {
        if(transform.position.y<killBoxTopYValue){
            checkPointComponent.playerDead= true;
        }
    }
}
