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
    
    //debug canvas
    //public TextMeshProUGUI raycastText;
    
    // create instance of laser pointer
    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        //debug canvas
        //raycastText = GameObject.FindGameObjectWithTag("RaycastText").GetComponent<TextMeshProUGUI>();
    }
    
    public bool LaserHit()
    {
        // raycast follows laser line rendering, get hit from collision of ray
        if (Physics.Raycast (handTransform.transform.position, handTransform.forward, out hit))
        {
            //debug canvas
            //raycastText.text = hit.transform.tag;
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
