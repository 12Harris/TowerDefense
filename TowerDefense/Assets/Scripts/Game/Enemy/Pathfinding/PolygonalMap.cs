using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonalMap : MonoBehaviour
{
    public List<Node> nodeArray;
    List<Node> visitedNodes;
    public List<GameObject> visualNodes;
    public Transform start, end;
    private Node startNode, endNode;
    public List<Node> FinalPath;//The completed path
    Pathfinding pathfinding;
    public bool pathBuilt = false;
    public bool setupcomplete = false;
    public LayerMask whatIsNode;
    
    [SerializeField]
    private GameObject nodePrefab;


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

    private void GenerateNodes()
    {
        for(float z = -14.5f; z <= 14.5f; z+= 1f)
        {
            for(float x = -9.5f; x <= 9.5f; x += 1f)
            {
                var pos = new Vector3(x,0.5f,z);
                var nodeGO = Instantiate(nodePrefab,new Vector3(x,.5f,z),Quaternion.identity);
                //nodeArray.Add(new Node(pos));
                var node = nodeGO.AddComponent<Node>();
                node.vPosition = pos;
                nodeArray.Add(node);
            }
        }
    }

    void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        nodeArray = new List<Node>();

        visitedNodes = new List<Node>();

        GenerateNodes();

        setupcomplete = true;

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
    void Update()
    {
        pathBuilt = false;

        for (int i = 0; i < nodeArray.Count; i++)
        {
            Node current = nodeArray[i];

            foreach(var neighbor in GetNeighboringNodes(current))
            {
                Debug.DrawRay(current.vPosition,neighbor.vPosition - current.vPosition, Color.green);
            }
        }

        //pathfinding.FindPath(startNode, endNode);
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

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 3f, whatIsNode))
                {
                    if(hit.collider.gameObject.GetComponent<Node>() == a_NeighborNode)
                        continue;
                    NeighborList.Add(hit.collider.gameObject.GetComponent<Node>());
                }
            }
        }
        return NeighborList;//Return the neighbors list.
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
    }
}
