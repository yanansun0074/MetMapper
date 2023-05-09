using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Art" || other.gameObject.tag == "Portrait")
        {
            other.transform.Translate(Vector3.forward * 0.1f);;
        }
    }
}
