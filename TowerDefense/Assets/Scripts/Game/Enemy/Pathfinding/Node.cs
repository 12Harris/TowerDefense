using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;

public class Node : MonoBehaviour
{

    public bool isStart, isEnd;
    public Vector3 vPosition;//The world position of the node.

    public Node ParentNode;//For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.

    public float igCost;//The cost of moving to the next square.
    public float ihCost;//The distance to the goal from this node.

    public float FCost { get { return igCost + ihCost; } }//Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.

    public GridTower _gridTower;
    /*public Node(Vector3 a_vPos)//Constructor
    {
        vPosition = a_vPos;//The world position of the node.
    }*/

    private void Awake()
    {
        _gridTower = null;
    }


    public void SetPosition(Vector3 pos)
    {
        vPosition = pos;
    }

}
