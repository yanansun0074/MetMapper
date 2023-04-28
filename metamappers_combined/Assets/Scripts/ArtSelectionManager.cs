using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArtSelectionManager : MonoBehaviour
{
    // Tutorial for raycasting in VR: https://www.youtube.com/watch?v=sPl1tPp7xt4
    public Canvas raycastCanvas; // canvas that displays selection UI
    
    float leftHand; // this is to hold the value of the button click on controller
    GameObject selectedObject;
    GameObject spawnedObject;
    
    public static TextMeshProUGUI nameText;
    public static TextMeshProUGUI artistYearText;
    public static TextMeshProUGUI descriptionText;
    
    // originally I wanted to spawn the UI in front of camera, but wasn't working
    // may come back to this
    [SerializeField]
    OVRCameraRig ovrCameraRig;
    
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
        
        nameText = GameObject.FindGameObjectWithTag("NameText").GetComponent<TextMeshProUGUI>();
        artistYearText = GameObject.FindGameObjectWithTag("ArtistYearText").GetComponent<TextMeshProUGUI>();
        descriptionText = GameObject.FindGameObjectWithTag("DescriptionText").GetComponent<TextMeshProUGUI>();
        
        // we want the canvas to be hidden at first, no selected object
        raycastCanvas.enabled = false;
        raycastCanvas.GetComponent<Collider>().enabled = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get the input from left controller index trigger
        leftHand = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
                
        // if left hand trigger is pressed
        if (leftHand > 0.4)
        {
            // getting the laser pointer instance
            // check if raycast hit an artpiece
            // from 'CustomLaserPointer.cs" file
            // also make sure there is no selected or spawned object
            // so we are not spawning more than one at a time
            if (CustomLaserPointer.instance.LaserHitArt() && spawnedObject == null && selectedObject == null)
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
            Vector3 position = spawnedObject.GetComponent<Collider>().bounds.center;
            ovrCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
            // if the thumbstick is being moved horizontally and the horizontal position is greater than the vertical position
            //if (leftThumbstickHorizontal != 0 && Mathf.Abs(leftThumbstickHorizontal) > Mathf.Abs(leftThumbstickVertical))
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
            {
                // rotate spawned object about y
                float angle = Mathf.Atan(leftThumbstickHorizontal) * Mathf.Rad2Deg;
                //spawnedObject.transform.rotation *= Quaternion.Euler(0f, -angle * Time.deltaTime * 5, 0f);
                spawnedObject.transform.RotateAround(position, spawnedObject.transform.up, -angle * Time.deltaTime * 5);
            }
            
            // if the thumbstick is being moved vertically and the vertical position is greater than the horizontal position
            //else if (leftThumbstickVertical != 0 && Mathf.Abs(leftThumbstickVertical) > Mathf.Abs(leftThumbstickHorizontal))
            else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
            {
                // rotate spawned object about x
                float angle = Mathf.Atan(leftThumbstickVertical) * Mathf.Rad2Deg;
                //spawnedObject.transform.rotation *= Quaternion.Euler(-angle * Time.deltaTime * 5, 0f, 0f);
                spawnedObject.transform.RotateAround(position, spawnedObject.transform.right, -angle * Time.deltaTime * 5);
            }
        }
    }
    
    // when close button is clicked on UI
    void CloseUI()
    {
        // destroy the spawned object and stop showing canvas
        Destroy(spawnedObject);
        raycastCanvas.enabled = false;
        raycastCanvas.GetComponent<Collider>().enabled = false;
        
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
        raycastCanvas.GetComponent<Collider>().enabled = true;
        
        // grab hit info, which art piece is selected
        RaycastHit hit = CustomLaserPointer.instance.getHit();
        selectedObject = hit.transform.gameObject;
                
        // get the art piece name (just the game object name for now)
        // and display it in UI
        Information art = selectedObject.GetComponent<Information>();

        nameText.text = art.GetName();
        artistYearText.text = art.GetArtistYear();
        descriptionText.text = art.GetDescription();
        
        //nameText.text = nameString;
        //artistYearText.text = artistYearString;
        //descriptionText.text = descriptionString;
        
        // spawn the same art piece game object
        //ovrCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
        //var position = ovrCameraRig.centerEyeAnchor.position;
        //spawnedObject = Instantiate(selectedObject, position + new Vector3(0,0,position.z * 1), selectedObject.transform.rotation);
        
        //Vector3 position = selectedObject.transform.position + (selectedObject.transform.forward * selectedObject.GetComponent<Collider>().bounds.size.x * 2);
        Vector3 position = selectedObject.transform.position;
        //spawnedObject = Instantiate(selectedObject, position + (selectedObject.transform.forward * 8) + (selectedObject.transform.up * 3), selectedObject.transform.rotation);
        spawnedObject = Instantiate(selectedObject, position + (selectedObject.transform.forward * 9) + (selectedObject.transform.up * -1), selectedObject.transform.rotation);
        
        // move the spawned object down by 2 units
        // might need to change this so that it is moved relative to the floor
        // so that it works well for all artpieces
        spawnedObject.transform.position += spawnedObject.transform.right * 1;
        
        Quaternion rotation = Quaternion.LookRotation(selectedObject.transform.forward * -1, Vector3.up);
        raycastCanvas.transform.rotation = rotation;
        raycastCanvas.transform.position = position + selectedObject.transform.up * 2;
        raycastCanvas.transform.Translate(Vector3.forward * -5f);
        
        // scale the object down so that the user can see it easily
        spawnedObject.transform.localScale = new Vector3(selectedObject.transform.localScale.x * 1.05f, selectedObject.transform.localScale.y * 1.05f, selectedObject.transform.localScale.z * 1.05f);
    }
}
