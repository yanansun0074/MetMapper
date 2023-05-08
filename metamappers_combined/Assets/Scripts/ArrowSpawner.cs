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
            NextTarget();
        }
        // current_target = GameObject.Find("Wakizashi4");
        // Vector3 player_pos = new Vector3(Player.position.x, 0.1f, Player.position.z);
        // Vector3 target_pos = new Vector3(current_target.transform.position.x, 0.1f, current_target.transform.position.z);
        // Path.enabled = true;
        // Vector3 direction = target_pos - player_pos;
        
        // Path.SetPosition(0, new Vector3(Player.position.x, 0.1f, Player.position.z) + Vector3.up  * PathHeightOffset);
        // //Path.SetPosition(1, new Vector3(current_target.transform.position.x, 0.1f, current_target.transform.position.z) + Vector3.up  * PathHeightOffset);
        // Path.SetPosition(1, direction * 0.1f + player_pos + Vector3.up * PathHeightOffset);
        // DrawPathCoroutine = StartCoroutine(DrawPathToTarget());
    }
    public void NextTarget()
    {
        if (ObjFinder.targets.Count > 0)
        {
            Path.enabled = true;
            current_target = ObjFinder.targets[ObjFinder.targets.Count -1];
            
            
            // Way1: Direct line between player & object
            // Path.SetPosition(0, new Vector3(Player.position.x, 0.1f, Player.position.z) + Vector3.up  * PathHeightOffset);
            // Path.SetPosition(1,new Vector3(current_target.transform.position.x, 0.1f, current_target.transform.position.z) + Vector3.up  * PathHeightOffset);

            // Way2: Arrow pointing at the direction
            Vector3 player_pos = new Vector3(Player.position.x, 0.1f, Player.position.z);
            Vector3 target_pos = new Vector3(current_target.transform.position.x, 0.1f, current_target.transform.position.z);
            Path.enabled = true;
            Vector3 direction = target_pos - player_pos;
            
            Path.SetPosition(0, direction * 0.05f + player_pos + Vector3.up  * PathHeightOffset);
            Path.SetPosition(1, direction * 0.12f + player_pos + Vector3.up * PathHeightOffset);

            // Way3: NavMesh AI Finding
            // if (DrawPathCoroutine != null)
            // {
            //     StopCoroutine(DrawPathCoroutine);
            // }
            // DrawPathCoroutine = StartCoroutine(DrawPathToTarget());

            // current_target = GameObject.Find("Landscape");
            update = true;
        }

    }

    private IEnumerator DrawPathToTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(PathUpdateSpeed);
        NavMeshPath path = new NavMeshPath();

        // current_target = GameObject.Find("Landscape");


        while (current_target != null)
        {
            if (NavMesh.CalculatePath(new Vector3(Player.position.x, 0.3f, Player.position.z), new Vector3(current_target.transform.position.x, 0.3f, current_target.transform.position.z), NavMesh.AllAreas, path))
            {
                current_target.SetActive(false);
                Debug.Log(path.corners.Length);
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
