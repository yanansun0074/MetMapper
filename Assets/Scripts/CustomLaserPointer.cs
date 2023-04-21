using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomLaserPointer : MonoBehaviour
{
    // Tutorial for raycasting in VR: https://www.youtube.com/watch?v=sPl1tPp7xt4
    
    public static CustomLaserPointer instance;
    
    public Transform handTransform;

    RaycastHit hit;
    
    // create instance of laser pointer
    void Awake()
    {
        instance = this;
    }
    
    public bool LaserHit()
    {
        // raycast follows laser line rendering, get hit from collision of ray
        if (Physics.Raycast (handTransform.transform.position, handTransform.forward, out hit))
        {
            // if laser hits art piece
            if (hit.transform.tag == "Art")
            {
                return true;
            }
        }
        
        return false;
    }
    
    // get hit from most recent collision
    public RaycastHit getHit()
    {
        return hit;
    }
}
