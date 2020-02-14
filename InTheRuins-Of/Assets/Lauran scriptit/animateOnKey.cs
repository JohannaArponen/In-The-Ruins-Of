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
    public KeyCode MyKey1;
    public KeyCode Mykey2;
    public KeyCode Mykey3;
    public string MyTrigger;

 
 void Update () 
 {
        
        if (Input.GetKey(MyKey1))
     {
            print("THIS SHOULD WORK BY NOW");
            transform.Rotate(0, 0, -9);
     }

     if (Input.GetKey(Mykey2))
        {
            print("THIS TOO SHOULD WORK BY NOW");
            transform.Rotate(0, 5, 0);
        }
        if (Input.GetKey(Mykey3))
        {
            print("THIS TOO SHOULD WORK BY NOW");
            transform.Rotate(3, 0, 0);
        }
    }
}
