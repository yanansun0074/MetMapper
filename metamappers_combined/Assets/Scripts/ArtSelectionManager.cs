using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArtSelectionManager : MonoBehaviour
{
    // Tutorial for raycasting in VR: https://www.youtube.com/watch?v=sPl1tPp7xt4
    public Canvas raycastCanvas; // canvas that displays selection UI
    
    public Transform leftHandTransform;
    float leftHand; // this is to hold the value of the button click on controller
    GameObject selectedObject;
    GameObject spawnedObject;
    
    public static TextMeshProUGUI nameText;
    public static TextMeshProUGUI artistYearText;
    public static TextMeshProUGUI descriptionText;
    
    // originally I wanted to spawn the UI in front of camera, but wasn't working
    // may come back to this
    GameObject centerEye;
    Vector3 headsetPos;
    
    // close button on UI
    [SerializeField]
    Button close;
    
    [SerializeField]
    public GameObject canvasSpawn;
    
    // get float values for the thumbstick to control rotation
    float leftThumbstickHorizontal;
    float leftThumbstickVertical;
    
    private Quaternion relativeRotation;
    
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
            
            /*if (leftHand > 0.4)
            {
                Rigidbody rigidbody = spawnedObject.GetComponent<Rigidbody>();
                float speed = rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg;
                //spawnedObject.transform.rotation *= Quaternion.Inverse(leftHandTransform.rotation);
                if (Mathf.Abs(leftHandTransform.rotation.y) > Mathf.Abs(leftHandTransform.rotation.x))
                    //spawnedObject.transform.RotateAround(position, spawnedObject.transform.up, -leftHandTransform.eulerAngles.y * Time.deltaTime * 0.5f);
                {
                    var q = Quaternion.AngleAxis(-leftHandTransform.eulerAngles.y, spawnedObject.transform.up);
                    float angle;
                    Vector3 axis;
                    q.ToAngleAxis(out angle, out axis);
                    rigidbody.angularVelocity = axis * angle * Mathf.Deg2Rad;
                    //rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(spawnedObject.transform.up * -leftHandTransform.eulerAngles.y * Time.deltaTime * 0.5f));
                }
                else if (Mathf.Abs(leftHandTransform.rotation.x) > Mathf.Abs(leftHandTransform.rotation.y))
                {
                    var q = Quaternion.AngleAxis(-leftHandTransform.eulerAngles.x, spawnedObject.transform.right);
                    float angle;
                    Vector3 axis;
                    q.ToAngleAxis(out angle, out axis);
                    rigidbody.angularVelocity = axis * angle * Mathf.Deg2Rad;
                }
                    //rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(spawnedObject.transform.right * -leftHandTransform.eulerAngles.x * Time.deltaTime * 0.5f));
                //spawnedObject.transform.RotateAround(position, spawnedObject.transform.right, -leftHandTransform.eulerAngles.x * Time.deltaTime * 0.5f);

            }*/
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
        
        centerEye = GameObject.Find("CenterEyeAnchor");
        headsetPos = centerEye.transform.position;
        
        Vector3 lookDirection = centerEye.transform.forward;
        /*raycastCanvas.transform.position = selectedObject.transform.position;
        raycastCanvas.transform.position += selectedObject.transform.up * 2;
                
        //raycastCanvas.transform.LookAt(centerEye.transform);
        Vector3 lookDirection = centerEye.transform.forward;
        lookDirection.y = 0;
       
        raycastCanvas.transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        raycastCanvas.transform.Translate(Vector3.forward * -2f);*/
        
        Vector3 position = new Vector3(canvasSpawn.transform.position.x, canvasSpawn.transform.position.y + 0.2f, canvasSpawn.transform.position.z);
        raycastCanvas.transform.position = position;
        raycastCanvas.transform.rotation = canvasSpawn.transform.rotation;
        raycastCanvas.transform.position += selectedObject.transform.up * 4;
        
        // spawn the same art piece game object
        //spawnedObject = Instantiate(selectedObject, position + (selectedObject.transform.forward * 8) + (selectedObject.transform.up * 3), selectedObject.transform.rotation);
        spawnedObject = Instantiate(selectedObject, position + (raycastCanvas.transform.forward * -3) + (selectedObject.transform.up * -1), Quaternion.Inverse(raycastCanvas.transform.rotation));
        
        spawnedObject.transform.rotation = Quaternion.LookRotation(lookDirection * -1, Vector3.up);
        //spawnedObject.transform.rotation = Quaternion.Inverse(canvasSpawn.transform.rotation);
        
        // move the spawned object down by 2 units
        // might need to change this so that it is moved relative to the floor
        // so that it works well for all artpieces
        spawnedObject.transform.position += spawnedObject.transform.right * 4;
        spawnedObject.transform.position += spawnedObject.transform.up * 4;

        // scale the object down so that the user can see it easily
        spawnedObject.transform.localScale = new Vector3(selectedObject.transform.localScale.x * 1.05f, selectedObject.transform.localScale.y * 1.05f, selectedObject.transform.localScale.z * 1.05f);
    }
}
