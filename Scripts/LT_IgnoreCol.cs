using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LT_IgnoreCol : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToIgnore;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < objectsToIgnore.Length; i++)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), objectsToIgnore[i].GetComponent<Collider>());
        }

        /*
        int i=0;
        while(i < objectsToIgnore.Length)
        {
            //stuff
            i++;
        }*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
