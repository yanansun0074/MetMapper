using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public GameObject layer;
    public Canvas raycastCanvas;
    public TextMeshProUGUI raycastText;
    float leftHand;
    public string art;
    
    // Start is called before the first frame update
    void Start()
    {
        raycastCanvas = GameObject.FindGameObjectWithTag("RaycastCanvas").GetComponent<Canvas>();
        raycastText = GameObject.FindGameObjectWithTag("RaycastText").GetComponent<TextMeshProUGUI>();
        
        raycastCanvas.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        leftHand = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        
        // raycastText.text = leftHand.ToString("N4");
        
        // if left hand trigger is pressed
        if (leftHand > 0)
        {
            //raycastText.text = "after trigger";

            if (CustomLaserPointer.instance.LaserHit())
            {
                StartCoroutine(getArtInfo());
            }
        }
    }
    
    // coroutine to give time to detect laser
    IEnumerator getArtInfo()
    {
        yield return new WaitForSeconds(0.5f);
        
        // grab hit info, which art piece is selected
        RaycastHit hit = CustomLaserPointer.instance.getHit();
        
        art = hit.transform.gameObject.name;
        
        raycastText.text = "Raycast hit: " + art;
    }
}
