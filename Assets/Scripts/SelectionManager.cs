using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    // Tutorial for raycasting in VR: https://www.youtube.com/watch?v=sPl1tPp7xt4
    public Canvas raycastCanvas; // canvas that displays selection UI
    public TextMeshProUGUI raycastText; // text on UI
    float leftHand; // this is to hold the value of the button click on controller
    public string artName;
    GameObject selectedObject;
    GameObject spawnedObject;
    
    // originally I wanted to spawn the UI in front of camera, but wasn't working
    // may come back to this
    [SerializeField]
    Camera OVRCamera;
    
    // close button on UI
    [SerializeField]
    Button close;
    
    // get float values for the thumbstick to control rotation
    float leftThumbstickHorizontal;
    float leftThumbstickVertical;
    
    // Start is called before the first frame update
    void Start()
    {
        // on start there is no selected object or spawned object
        selectedObject = null;
        spawnedObject = null;
        
        // canvas that displays selction UI
        raycastCanvas = GameObject.FindGameObjectWithTag("RaycastCanvas").GetComponent<Canvas>();
        
        // text on UI
        raycastText = GameObject.FindGameObjectWithTag("RaycastText").GetComponent<TextMeshProUGUI>();
        
        // we want the canvas to be hidden at first, no selected object
        raycastCanvas.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get the input from left controller index trigger
        leftHand = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
                
        // if left hand trigger is pressed
        if (leftHand > 0.5)
        {
            // getting the laser pointer instance
            // check if raycast hit an artpiece
            // from 'CustomLaserPointer.cs" file
            // also make sure there is no selected or spawned object
            // so we are not spawning more than one at a time
            if (CustomLaserPointer.instance.LaserHit() && spawnedObject == null && selectedObject == null)
            {
                // set the selected object to the hit object
                RaycastHit hit = CustomLaserPointer.instance.getHit();
                selectedObject = hit.transform.gameObject;
                
                // start coroutine
                StartCoroutine(showArtInfo());
            }
        }
        
        Button closeButton = close.GetComponent<Button>();
        closeButton.onClick.AddListener(CloseUI);
        
        // get input from left controller thumbstick
        leftThumbstickHorizontal = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        leftThumbstickVertical = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        
        // there must be a selected and spawned object to rotate
        if (selectedObject != null && spawnedObject != null)
        {
            // if the thumbstick is being moved horizontally and the horizontal position is greater than the vertical position
            if (leftThumbstickHorizontal != 0 && Mathf.Abs(leftThumbstickHorizontal) > Mathf.Abs(leftThumbstickVertical))
            {
                // rotate spawned object about y
                float angle = Mathf.Atan(leftThumbstickHorizontal) * Mathf.Rad2Deg;
                spawnedObject.transform.rotation *= Quaternion.Euler(0f, -angle * Time.deltaTime * 5, 0f);
            }
            
            // if the thumbstick is being moved vertically and the vertical position is greater than the horizontal position
            else if (leftThumbstickVertical != 0 && Mathf.Abs(leftThumbstickVertical) > Mathf.Abs(leftThumbstickHorizontal))
            {
                // rotate spawned object about x
                float angle = Mathf.Atan(leftThumbstickVertical) * Mathf.Rad2Deg;
                spawnedObject.transform.rotation *= Quaternion.Euler(-angle * Time.deltaTime * 5, 0f, 0f);
            }

        }
    }
    
    // when close button is clicked on UI
    void CloseUI()
    {
        // destroy the spawned object and stop showing canvas
        Destroy(spawnedObject);
        raycastCanvas.enabled = false;
        
        // no selected object anymore
        selectedObject = null;
        spawnedObject = null;
    }
    
    // coroutine to give time to detect laser
    IEnumerator showArtInfo()
    {
        yield return new WaitForSeconds(0.5f);
        
        // show canvas now
        raycastCanvas.enabled = true;
        
        // grab hit info, which art piece is selected
        RaycastHit hit = CustomLaserPointer.instance.getHit();
        selectedObject = hit.transform.gameObject;
        
        // get the art piece name (just the game object name for now)
        // and display it in UI
        artName = hit.transform.gameObject.name;
        raycastText.text = "Name: " + artName;
        
        // spawn the same art piece game object
        spawnedObject = Instantiate(selectedObject, selectedObject.transform.position + selectedObject.transform.forward * 2, selectedObject.transform.rotation);
        
        // move the spawned object down by 2 units
        // might need to change this so that it is moved relative to the floor
        // so that it works well for all artpieces
        spawnedObject.transform.position += new Vector3(0, -2, 0);
        
        // scale the object down so that the user can see it easily
        spawnedObject.transform.localScale = new Vector3(selectedObject.transform.localScale.x * 1.3f, selectedObject.transform.localScale.y * 1.3f, selectedObject.transform.localScale.z * 1.3f);
    }
}
