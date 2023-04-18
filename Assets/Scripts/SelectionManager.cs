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
    GameObject selectedObject;
    GameObject spawnedObject;
    
    [SerializeField]
    Camera OVRCamera;
    
    [SerializeField]
    Button close;
    
    float leftThumbstickHorizontal;
    float leftThumbstickVertical;
    
    // Start is called before the first frame update
    void Start()
    {
        selectedObject = null;
        spawnedObject = null;
        raycastCanvas = GameObject.FindGameObjectWithTag("RaycastCanvas").GetComponent<Canvas>();
        raycastText = GameObject.FindGameObjectWithTag("RaycastText").GetComponent<TextMeshProUGUI>();
                
        raycastCanvas.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        leftHand = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        
        // raycastText.text = leftHand.ToString("N4");
        
        // if left hand trigger is pressed
        if (leftHand > 0.5)
        {
            //raycastText.text = "after trigger";

            if (CustomLaserPointer.instance.LaserHit() && spawnedObject == null && selectedObject == null)
            {
                RaycastHit hit = CustomLaserPointer.instance.getHit();
                selectedObject = hit.transform.gameObject;
                
                StartCoroutine(showArtInfo());
            }
        }
        
        Button closeButton = close.GetComponent<Button>();
        closeButton.onClick.AddListener(CloseUI);
        
        leftThumbstickHorizontal = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        leftThumbstickVertical = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        
        if (selectedObject != null && spawnedObject != null)
        {
            /*if (joystick.x != 0 || joystick.y != 0)
            {
                float angle = Mathf.Atan2(joystick.y, joystick.x) * Mathf.Rad2Deg;
                transform.Rotate(new Vector3(0, angle, 0));
            }*/
            var rotation = Quaternion.LookRotation(spawnedObject.transform.position);
             
            if (leftThumbstickHorizontal != 0 && Mathf.Abs(leftThumbstickVertical) > Mathf.Abs(leftThumbstickHorizontal))
            {
                float angle = Mathf.Atan(leftThumbstickHorizontal) * Mathf.Rad2Deg;
                spawnedObject.transform.rotation *= Quaternion.Euler(0f, -angle * Time.deltaTime * 5, 0f);
            }
            
            else if (leftThumbstickVertical != 0 && Mathf.Abs(leftThumbstickHorizontal) > Mathf.Abs(leftThumbstickVertical))
            {
                float angle = Mathf.Atan(leftThumbstickVertical) * Mathf.Rad2Deg;
                spawnedObject.transform.rotation *= Quaternion.Euler(-angle * Time.deltaTime * 5, 0f, 0f);
            }

        }
    }
    
    void CloseUI()
    {
        Destroy(spawnedObject);
        raycastCanvas.enabled = false;
        selectedObject = null;
        spawnedObject = null;
    }
    
    // coroutine to give time to detect laser
    IEnumerator showArtInfo()
    {
        yield return new WaitForSeconds(0.5f);
        
        raycastCanvas.enabled = true;
        
        // grab hit info, which art piece is selected
        RaycastHit hit = CustomLaserPointer.instance.getHit();
        
        selectedObject = hit.transform.gameObject;
        
        art = hit.transform.gameObject.name;
        
        raycastText.text = "Name: " + art;
        
        spawnedObject = Instantiate(selectedObject, selectedObject.transform.position + selectedObject.transform.forward * 2, selectedObject.transform.rotation);
        
        spawnedObject.transform.position += new Vector3(0, -5, 0);
        
        spawnedObject.transform.localScale = new Vector3(selectedObject.transform.localScale.x * 0.15f, selectedObject.transform.localScale.y * 0.15f, selectedObject.transform.localScale.z * 0.15f);
    }
}
