using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateOnKey : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public KeyCode MyKey;
 public string MyTrigger;
 
 void Update () 
 {
     if (Input.GetKey(MyKey))
     {
         GetComponent<Animator>().SetTrigger(MyTrigger);
     }
 }
}
