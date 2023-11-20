using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public float GCost { get; set; }
    public float HCost { get; set; }
    public float FCost
    {
        get
        {
            return GCost + HCost;
        }
    }

    public Node parent { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsPlaced { get; set; }
    public bool IsEndNode { get; set; }
}
