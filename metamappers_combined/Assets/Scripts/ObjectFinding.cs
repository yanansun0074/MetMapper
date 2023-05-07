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
        // targets = new List<GameObject>();
        ObjFinder = GameObject.Find("TargetManager").GetComponent<TargetsManager>();
        // targets = ObjFinder.targets;

    }

    public void AddTarget()
    {
        string obj_name = this.GetComponentInChildren<TextMeshProUGUI>().text;
        // if(targets[0] != null)
        // {
        //     // End the highlight
        //     removeHighlight(targets[0]);
        //     ObjFinder.RemoveTarget();
        //     targets.RemoveAt(0);
        // }
        // this.GetComponentInChildren<TextMeshProUGUI>().text = "success!";
        GameObject target = GameObject.Find(obj_name);
        // target.SetActive(false);
        // if(targets.IndexOf(target) == -1)
        // {
        // targets.Add(target);
        ObjFinder.AddTarget(target);
        // }
        

        ObjFinder.HighlightTarget();
        
    }

    // public void highlightTarget(GameObject target)
    // {
    //     if(target != null)
    //     {
    //         Renderer r = target.GetComponent(typeof(Renderer)) as Renderer;
    //         oldColor = r.material.GetColor("_Color");
    //         Color newColor = new Color(oldColor.r + 0.2f, oldColor.g + 0.2f, oldColor.b + 0.2f, oldColor.a);
    //         r.material.SetColor("_Color", newColor);
    //     }
    // }

    // public void removeHighlight(GameObject old_obj)
    // {
    //     if(old_obj != null)
    //     {
    //         Renderer r = old_obj.GetComponent(typeof(Renderer)) as Renderer;
    //         r.material.SetColor("_Color", oldColor);
    //     }
    // }
}
