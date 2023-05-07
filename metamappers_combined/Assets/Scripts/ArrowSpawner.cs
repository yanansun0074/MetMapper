using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArrowSpawner : MonoBehaviour
{
    TargetsManager ObjFinder;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private LineRenderer Path;
    public float PathHeightOffset;
    public float SpawnHeightOffset;
    public float PathUpdateSpeed;

    private GameObject current_target;
    private NavMeshTriangulation Triangulation;
    private Coroutine DrawPathCoroutine;
    private bool update;

    private void Awake()
    {
        Triangulation = NavMesh.CalculateTriangulation();
        update = false; 
    }

    // Start is called before the first frame update
    void Start()
    {
        ObjFinder = GameObject.Find("TargetManager").GetComponent<TargetsManager>();
        // NextTarget();
        Path.enabled = false;
        

    }

    void Update(){
        if (update)
        {
            Path.SetPosition(0, Player.position + Vector3.up  * PathHeightOffset);
        }
    }
    public void NextTarget()
    {
        Path.enabled = true;
        if (ObjFinder.targets.Count > 0)
        {
            Path.enabled = true;
            current_target = ObjFinder.targets[ObjFinder.targets.Count -1];
            // if (DrawPathCoroutine != null)
            // {
            //     StopCoroutine(DrawPathCoroutine);
            // }
            // DrawPathCoroutine = StartCoroutine(DrawPathToTarget());
            // for (int i = 0; i < Path.positionCount; i++)
            // {
            //     Path.SetPosition(i, )
            // }

            Path.SetPosition(0, new Vector3(Player.position.x, 0.1f, Player.position.z) + Vector3.up  * PathHeightOffset);
            Path.SetPosition(1,new Vector3(current_target.transform.position.x, 0.1f, current_target.transform.position.z) + Vector3.up  * PathHeightOffset);
            update = true;
        }

    }

    private IEnumerator DrawPathToTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(PathUpdateSpeed);
        NavMeshPath path = new NavMeshPath();

        while (current_target != null)
        {
            if (NavMesh.CalculatePath(Player.position, current_target.transform.position, NavMesh.AllAreas, path))
            {
                Path.positionCount = path.corners.Length;

                for (int i = 0; i < path.corners.Length; i++)
                {
                    Path.SetPosition(i, path.corners[i] + Vector3.up * PathHeightOffset);
                }
            }
            else
            {
                Debug.LogError($"Unable to calculate a path on the NavMesh between {Player.position} and {current_target.transform.position}!");
            }
            yield return Wait;
        }


    }

}
