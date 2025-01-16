using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public bool isStart, isEnd;
    public Vector3 vPosition;//The world position of the node.

    public Node ParentNode;//For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.

    public float igCost;//The cost of moving to the next square.
    public float ihCost;//The distance to the goal from this node.

    public float FCost { get { return igCost + ihCost; } }//Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.

    /*public Node(Vector3 a_vPos)//Constructor
    {
        vPosition = a_vPos;//The world position of the node.
    }*/


    public bool connectedToNode(Node otherNode, LayerMask whatIsNode)
    {
        bool connected = false;

        //if(!(isStart && otherNode.isEnd || isEnd && otherNode.isStart))//start node and end node arent connected to each other
        {
            //cast a ray from this node to the other node to check if they are connected or obstructed by an obstacle
            //they are not obstructed if another node is in the way
            Ray ray = new Ray(vPosition, otherNode.vPosition - vPosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Vector3.Distance(vPosition,otherNode.vPosition), whatIsNode))
            {
                Debug.Log("the ray hit: " + hit.collider.gameObject);
            }

            if (!Physics.Raycast(ray, Vector3.Distance(vPosition,otherNode.vPosition), whatIsNode))
            {
                connected = true;
            }

        }
        return connected;
    }

    public void SetPosition(Vector3 pos)
    {
        vPosition = pos;
    }

}
