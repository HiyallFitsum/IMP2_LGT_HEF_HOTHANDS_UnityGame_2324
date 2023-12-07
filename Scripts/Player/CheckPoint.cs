using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{


    [SerializeField] GameObject[] respawnPoints;//This should be the checkpoint objects in order------First Checkpoint should be at top and last checkpoint should be at bottom

    public bool playerDead = false;
    public int latestCheckPoint=0;

    // Start is called before the first frame update
    void Start()
    {
       // Physics.IgnoreCollision(GetComponent<Collider>(), respawnPoints[latestCheckPoint].GetComponent<Collider>());//this ignores collision of the default checkpoint
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDead){
            teleportPlayer();
            playerDead = false;
        }
    }
    void teleportPlayer(){
        transform.position = respawnPoints[latestCheckPoint].transform.position;
    }
    void OnTriggerEnter(Collider other){
        for(int i=0; i<respawnPoints.Length; i++)
        {
            if(other.gameObject == respawnPoints[i]){//
                if(i==latestCheckPoint+1){
                    latestCheckPoint=i;
                    Debug.Log("Highest Checkpoint set to:"+i);
                    //Physics.IgnoreCollision(GetComponent<Collider>(), respawnPoints[i].GetComponent<Collider>());
                }
            }
        }
    }
}
