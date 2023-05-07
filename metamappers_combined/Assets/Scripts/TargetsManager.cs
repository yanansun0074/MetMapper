using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class TargetsManager : MonoBehaviour
{
    public List<GameObject> targets;
    private Color oldColor;
    ArrowSpawner arrowSpawner;

    // Start is called before the first frame update
    void Start()
    {
        targets = new List<GameObject>();
        arrowSpawner = GameObject.Find("ArrowSpawner").GetComponent<ArrowSpawner>();
    }

    public void AddTarget(GameObject tar)
    {
        targets.Add(tar);
        HighlightTarget();
    }

    public void RemoveTarget()
    {
        targets.RemoveAt(0);
    }
    // Update is called once per frame
    void Update()
    {

        
    }

    public void HighlightTarget()
    {
        if (targets.Count >1)
        {
            RemoveHighlight(targets[targets.Count -2]);
            RemovePath(targets[targets.Count -2]);
        }
        if(targets[targets.Count-1] != null)
        {
            GameObject target = targets[targets.Count-1];
            // target.SetActive(false);
            Renderer r = target.GetComponent(typeof(Renderer)) as Renderer;
            oldColor = r.material.GetColor("_Color");
            Color newColor = new Color(oldColor.r, oldColor.g + 0.3f, oldColor.b + 0.1f, oldColor.a);
            r.material.SetColor("_Color", newColor);
            GeneratePath();
        }
    }

    public void RemoveHighlight(GameObject old_obj)
    {
        if(old_obj != null)
        {
            // old_obj.SetActive(true);
            Renderer r = old_obj.GetComponent(typeof(Renderer)) as Renderer;
            r.material.SetColor("_Color", oldColor);
        }
    }

    void RemovePath(GameObject obj)
    {

    }

    void GeneratePath()
    {
        arrowSpawner.NextTarget();
    }
}
