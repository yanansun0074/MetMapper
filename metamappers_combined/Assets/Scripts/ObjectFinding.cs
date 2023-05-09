using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ObjectFinding : MonoBehaviour
{
    TargetsManager ObjFinder;
    List<GameObject> targets;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
        ObjFinder = GameObject.Find("TargetManager").GetComponent<TargetsManager>();
        

    }

    public void AddTarget()
    {    
        GameObject target = this.GetComponent<ObjButton>().artwork;

        ObjFinder.AddTarget(target);     

        
    }

    
}
