using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject portrait;
    // Start is called before the first frame update
    void Start()
    {
        Information art = portrait.GetComponent<Information>();
        print("info: " + art.GetName() + "\n" + art.GetArtistYear() + "\n" + art.GetDescription());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
