﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    public PolygonalMap MapReference;//For referencing the grid class

    public Node StartPosition;//Starting position to pathfind from
    public Node TargetPosition;//Starting position to pathfind to

    private bool _foundPath = false;

    public PolygonalMap getMapReference()
    {

        return MapReference;
    }

    private void Awake()//When the program starts
    {
        MapReference = GetComponent<PolygonalMap>();
    }

    private void Start()
    {

    }

    private void Update()//Every frame
    {
        //FindPath(StartPosition.position, TargetPosition.position);//Find a path to the goal
        //FindPath(StartPosition, TargetPosition);//Find a path to the goal
    }

    public void SetStartPos(Node pos)
    {
        StartPosition = pos;
    }

    public void SetTargetPos(Node pos)
    {
        TargetPosition = pos;
    }


    public void FindPath(Node StartNode, Node TargetNode)
    {
        List<Node> OpenList = new List<Node>();//List of nodes for the open list
        HashSet<Node> ClosedList = new HashSet<Node>();//Hashset of nodes for the closed list

        OpenList.Add(StartNode);//Add the starting node to the open list to begin the program
        _foundPath = false;

        while (OpenList.Count > 0)//Whilst there is something in the open list
        {
            Node CurrentNode = OpenList[0];//Create a node and set it to the first item in the open list
            for (int i = 1; i < OpenList.Count; i++)//Loop through the open list starting from the second object
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];//Set the current node to that object
                }
            }
            OpenList.Remove(CurrentNode);//Remove that from the open list
            ClosedList.Add(CurrentNode);//And add it to the closed list

            if (CurrentNode == TargetNode)//If the current node is the same as the target node
            {
                _foundPath = true;
                GetFinalPath(StartNode, TargetNode);//Calculate the final path
            }

            if (MapReference.GetNeighboringNodes(CurrentNode).Count == 0)
                Debug.Log("node has no neighbors??");

            foreach (Node NeighborNode in MapReference.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
            {
                
                if (ClosedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }
                float MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);//Get the F cost of that neighbor

                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    NeighborNode.igCost = MoveCost;//Set the g cost to the f cost
                    NeighborNode.ihCost = GetManhattenDistance(NeighborNode, TargetNode);//Set the h cost
                    NeighborNode.ParentNode = CurrentNode;//Set the parent of the node for retracing steps

                    if (!OpenList.Contains(NeighborNode))//If the neighbor is not in the openlist
                    {
                        OpenList.Add(NeighborNode);//Add it to the list
                    }
                }
            }

        }

        if(!_foundPath)
            MapReference.FinalPath.Clear();
    }



    void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();//List to hold the path sequentially 
        Node CurrentNode = a_EndNode;//Node to store the current node being checked

        while (CurrentNode != a_StartingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.ParentNode;//Move onto its parent node
        }

        FinalPath.Add(a_StartingNode);
        FinalPath.Reverse();//Reverse the path to get the correct order

        //if(FinalPath.Count >)
        MapReference.FinalPath = FinalPath;//Set the final path

    }

    float GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        /*int ix = Mathf.Abs(a_nodeA.iGridX - a_nodeB.iGridX);//x1-x2
        int iy = Mathf.Abs(a_nodeA.iGridY - a_nodeB.iGridY);//y1-y2

        return ix + iy;//Return the sum*/
        //return Vector3.Distance(a_nodeA.vPosition, a_nodeB.vPosition);//WRONG : THIS IS NOT THE MANHATTEN DISTANCE :)
        return Mathf.Abs((a_nodeA.vPosition.x - a_nodeB.vPosition.x) + (a_nodeA.vPosition.z - a_nodeB.vPosition.z));
    }
}
