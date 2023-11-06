using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public List<Node> FindPath(Node _startNode, Node _endNode)
    {
        GridManager.Instance.path.Clear();
        List<Node> openNodeList = new List<Node>();
        List<Node> closeNodeList = new List<Node>();
        openNodeList.Add(_startNode);
        while (openNodeList.Count > 0)
        {
            Node currentNode = openNodeList[0];
            for (int i = 1; i < openNodeList.Count; i++)
            {
                if (openNodeList[i].FCost < currentNode.FCost || (openNodeList[i].FCost == currentNode.FCost && openNodeList[i].HCost < currentNode.HCost))
                {
                    currentNode = openNodeList[i];
                }
            }
            openNodeList.Remove(currentNode);
            closeNodeList.Add(currentNode);
            if (currentNode == _endNode)
            {
                return RetracePath(_startNode, _endNode);
            }
            foreach (var neighborNode in GridManager.Instance.GetNeighborNode(currentNode))
            {
                if (neighborNode.IsPlaced || closeNodeList.Contains(neighborNode))
                    continue;
                float newMovementCostToNeighborNode = currentNode.GCost + GetDistance(currentNode, neighborNode);
                if (newMovementCostToNeighborNode < neighborNode.GCost || !openNodeList.Contains(neighborNode))
                {
                    neighborNode.GCost = newMovementCostToNeighborNode;
                    neighborNode.HCost = GetDistance(neighborNode, _endNode);
                    neighborNode.parent = currentNode;
                    if (!openNodeList.Contains(neighborNode))
                    {
                        openNodeList.Add(neighborNode);
                    }
                }
            }
        }
        return null;
    }

    public List<Node> RetracePath(Node _startNode, Node _endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = _endNode;
        while (currentNode != _startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(_startNode);
        path.Reverse();
        GridManager.Instance.path = path;
        return path;
    }

    public float GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.X - nodeB.X);
        int dstY = Mathf.Abs(nodeA.Y - nodeB.Y);
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}