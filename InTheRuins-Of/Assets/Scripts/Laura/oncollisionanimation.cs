using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oncollisionanimation : MonoBehaviour
{

    public Animation myanimation;

    void OnTriggerEnter()
    {
        
        myanimation.Play();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
