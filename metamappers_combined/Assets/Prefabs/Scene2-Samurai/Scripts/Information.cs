using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Information : MonoBehaviour
{
    public string artName;
    public string artistYear;
    public string description;
    
    public string GetName()
    {
        return artName;
    }
        
    public string GetArtistYear()
    {
        return artistYear;
    }
        
    public string GetDescription()
    {
        return description;

    }
}
