using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonalMap : MonoBehaviour
{
    public List<Node> nodeArray;
    List<Node> visitedNodes;
    public List<GameObject> visualNodes;
    public Transform start, end;

    [SerializeField]
    private Node startNode;
    
    [SerializeField]
    private Node endNode;
    public List<Node> FinalPath;//The completed path
    Pathfinding pathfinding;
    public bool pathBuilt = false;
    public bool setupcomplete = false;
    public LayerMask whatIsNode;

    public static PolygonalMap Instance;
    
    [SerializeField]
    private GameObject nodePrefab;

    private int turretLayer = 7;


    public Node getStartNode()
    {
        return startNode;
    }

    public Node getEndNode()
    {
        return endNode;
    }

    public void SetEndNode(Node n)
    {
        endNode = n;
    }

    public Node GetNode(Vector3 pos)
    {
        foreach(var node in nodeArray)
        {
            var v = new Vector3(pos.x,node.vPosition.y,pos.z);
            if(node.vPosition == v)
                return node;
        }
        return null;
    }

    private void GenerateNodes()
    {
        for(float z = -14.5f; z <= 14.5f; z+= 1f)
        {
            for(float x = -9.5f; x <= 9.5f; x += 1f)
            {
                var pos = new Vector3(x,0.3f,z);
                var nodeGO = Instantiate(nodePrefab,pos,Quaternion.identity);
                //nodeArray.Add(new Node(pos));
                var node = nodeGO.AddComponent<Node>();
                node.vPosition = pos;
                nodeArray.Add(node);
            }
        }
        startNode = nodeArray[0];
        endNode = nodeArray[nodeArray.Count-1];
    }

    void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        nodeArray = new List<Node>();

        visitedNodes = new List<Node>();

        GenerateNodes();

        setupcomplete = true;

        Instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        //pathfinding.FindPath(startNode, endNode);//Solange findpath nicht null zur�ckgibt, wird gridreference.FinalPath gespeichert
    }

    public void AddNode(Node n)
    {
        nodeArray.Add(n);
    }

    // Update is called once per frame
    private void Update()
    {

        nodeArray[0].transform.gameObject.name = "PATHSTART";

        pathBuilt = false;

        //Draw Node connections
        /*for (int i = 0; i < nodeArray.Count; i++)
        {
            Node current = nodeArray[i];

            foreach(var neighbor in GetNeighboringNodes(current))
            {
                Debug.DrawRay(current.vPosition,neighbor.vPosition - current.vPosition, Color.green);
            }
        }*/

        //Visualize path
        pathfinding.FindPath(startNode, endNode);

        /*for(int i = 0; i < FinalPath.Count-1; i++)
        {
            Debug.DrawRay(FinalPath[i].vPosition, FinalPath[i+1].vPosition - FinalPath[i].vPosition, Color.red);
        }*/
        pathBuilt = true;
    }

    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();//Make a new list of all available neighbors.

        for(float z = a_NeighborNode.vPosition.z - 1; z <= a_NeighborNode.vPosition.z +1; z+= 1f)
        {
            for(float x = a_NeighborNode.vPosition.x - 1; x <= a_NeighborNode.vPosition.x +1; x+= 1f)
            {
                
                var v = new Vector3(x,0.5f,z);
                Ray ray = new Ray(v + Vector3.up, -Vector3.up);
                bool nextIteration = false;

                RaycastHit hit;

                //if (Physics.Raycast(ray, out hit, 3f, whatIsNode))
                if (Physics.Raycast(ray, out hit, 3f))
                {
                    if(hit.collider.gameObject.GetComponent<Node>() != null)
                    {
                        if(hit.collider.gameObject.GetComponent<Node>() == a_NeighborNode)
                            continue;

                        if(hit.collider.gameObject.GetComponent<Node>()._gridTower != null)
                            continue;

                        //check to see if path is blocked by object
                        var dir = hit.collider.gameObject.GetComponent<Node>().vPosition - a_NeighborNode.vPosition;
                        dir*=0.5f;

                        //Debug.DrawRay(a_NeighborNode.vPosition,dir,Color.red);

                        Collider[] hitColliders = Physics.OverlapSphere(a_NeighborNode.vPosition+dir, 0.1f, 1 << turretLayer);
                        foreach (var hitCollider in hitColliders)
                        {
                            Debug.Log("HIT COLLIDER GO: "+ hitCollider.gameObject);
                            nextIteration = true;
                            //if(hitCollider.gameObject != a_NeighborNode._gridTower.gameObject)
                            {
                                hitCollider.gameObject.name = "BLACK SHEEP";
                                
                            }
                        }

                        if(!nextIteration)
                            NeighborList.Add(hit.collider.gameObject.GetComponent<Node>());
                        Debug.DrawRay(ray.origin,ray.direction,Color.red);
                    }
                    else
                    {
                        //Debug.Log("RAY HITT: " + hit.collider.gameObject);
                    }
                }
            }
        }
        /*float z = a_NeighborNode.vPosition.z - 1;
        float x = a_NeighborNode.vPosition.x - 1;

        float[] xcheck = {a_NeighborNode.vPosition.x, a_NeighborNode.vPosition.x - 1, a_NeighborNode.vPosition.x + 1,  a_NeighborNode.vPosition.x };
        float[] zcheck = {a_NeighborNode.vPosition.z+1, a_NeighborNode.vPosition.z, a_NeighborNode.vPosition.z,  a_NeighborNode.vPosition.z-1};
        
        RaycastHit hit;

        for(int i = 0; i <4;i++)
        {
            var v = new Vector3(xcheck[i],0.5f,zcheck[i]);
            Ray ray = new Ray(v + Vector3.up, -Vector3.up);

            if (Physics.Raycast(ray, out hit, 3f))
            {
                if(hit.collider.gameObject.GetComponent<Node>() != null)
                {
                    NeighborList.Add(hit.collider.gameObject.GetComponent<Node>());
                    Debug.DrawRay(ray.origin,ray.direction,Color.red);
                }
                else
                {
                    Debug.Log("RAY HITT: " + hit.collider.gameObject);
                }
            }
        }*/


        return NeighborList;//Return the neighbors list.
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
    }
}
