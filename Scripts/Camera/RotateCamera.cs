using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float sensX = 2.5f;
    public float sensY = 2.5f;

    public Transform orientation;

    float xRotation;
    float yRotation;
    public bool invertedCamera = false;

    public Vector3 screenPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
        if(invertedCamera == true){
            sensX = sensX*-1;
            sensY = sensY*-1;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    { 
        
        screenPosition = Input.mousePosition;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        xRotation -=mouseY;
        yRotation +=mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f,90f);

        transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
        orientation.rotation = Quaternion.Euler(0,yRotation,0);

    }
}
