using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLaserPointer : MonoBehaviour
{
    public static CustomLaserPointer instance;
    
    public Transform handTransform;
    
    RaycastHit hit;
    
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
