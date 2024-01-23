using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HEF_Health : MonoBehaviour
{
    // Start is called before the first frame update


    private Image rend;
    private int playerHealth = 3;
    public GameObject healthAlert;
    public float speed;
    public float limit = 0.4f;

    private Color colorToTurnTo;
    private float opacity = 0;
    private bool healthState = false;

    GameObject target;
    private void Start()
    {
       rend = healthAlert.GetComponent<Image>();
    
    }

    // Update is called once per frame
    void Update()
    {
        if(healthState){
                if(opacity < limit){
                    opacity += Time.deltaTime*speed;
                    //opacity += Mathf.Pow(Time.deltaTime, speed);
                    rend.color = new Color(1f,0f,0f,opacity);
                    //Debug.log(opacity);
                }else{
                    healthState = false;
                    //Debug.Log(healthState);
                }
            }
        if(!healthState){
            if(opacity > 0){
                opacity -= Time.deltaTime*speed;
                //opacity -= Mathf.Pow(Time.deltaTime, speed);
                rend.color = new Color(1f,0f,0f,opacity);
                //Debug.Log(opacity);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Bullet"))
        {
            healthState = true;
            playerHealth -= 1;
            //healthAlert.SetActive(true);
        }
    }
}
