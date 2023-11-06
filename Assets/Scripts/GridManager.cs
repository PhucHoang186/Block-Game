using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public GridGenerator gridGenerator;
    [SerializeField] bool showGrid;
    [SerializeField] float nodeSize;
    public List<Node> path;
    private Camera cam;
    public Dictionary<Vector3, Node> Grids { get; set; }

    // Init player, object position
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        cam = Camera.main;
    }

    public void Init()
    {
        Grids = gridGenerator.Init(Grids, nodeSize);
    }

    public Node GetNodeById(int _x, int _y)
    {
        if (Grids.ContainsKey(new Vector3(_x, 0f, _y)))
        {
            return Grids[new Vector3(_x, 0f, _y)];
        }
        else
        {
            return null;
        }
    }

    public Node GetNodeByPosition(Vector3 _pos)
    {
        float x = Mathf.Ceil(_pos.x / nodeSize);
        float z = Mathf.Ceil(_pos.z / nodeSize);

        var nodeId = new Vector3(x, 0f, z);
        if (Grids.ContainsKey(nodeId))
        {
            return Grids[nodeId];
        }
        else
        {
            return null;
        }
    }

    
    public Node GetNextNode(Vector3 direction, Node currentNode)
    {
        int x = currentNode.X;
        int y = currentNode.Y;

        Vector3 newNodeLocaltion = new Vector3(x + direction.x, 0f, y + direction.z);
        if (GridManager.Instance.Grids.ContainsKey(newNodeLocaltion))
        {
            return GridManager.Instance.Grids[newNodeLocaltion];
        }
        return null;
    }

    public Vector3 GetNodeIDByPosition(Vector3 _pos)
    {
        Vector3 newId = new Vector3(_pos.x / nodeSize, 0f, _pos.z / nodeSize);
        return newId;
    }

    public List<Node> GetNeighborNode(Node _node)
    {
        List<Node> neighborNodes = new List<Node>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Mathf.Abs(i) == Mathf.Abs(j))
                    continue;
                int x = _node.X + i;
                int y = _node.Y + j;
                if (x >= 0 && x < gridGenerator.weight && y >= 0 && y < gridGenerator.height && GetNodeById(x, y) != null)
                {
                    neighborNodes.Add(Grids[new Vector3(x, 0f, y)]);
                }
            }
        }
        return neighborNodes;
    }
    public List<Node> GetNodesInRange(Node _startNode, int _range)
    {
        List<Node> inRangeNodes = new List<Node>();
        List<Node> previousStepNodes = new List<Node>();
        inRangeNodes.Add(_startNode);
        previousStepNodes.Add(_startNode);
        int step = 0;
        while (step < _range)
        {
            var neighborNodes = new List<Node>();
            foreach (Node node in previousStepNodes)
            {
                neighborNodes.AddRange(GridManager.Instance.GetNeighborNode(node));
            }
            inRangeNodes.AddRange(neighborNodes);
            previousStepNodes = neighborNodes;
            step++;
        }
        return inRangeNodes;
    }

    public List<Node> GetGridList()
    {
        List<Node> nodeList = Grids.Values.ToList();
        return nodeList;
    }

    void OnDrawGizmos()
    {
        if (showGrid)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < gridGenerator.weight; i++)
            {
                for (int j = 0; j < gridGenerator.height; j++)
                {
                    Gizmos.DrawCube(new Vector3(i * nodeSize, 0f, j * nodeSize), Vector3.one * (nodeSize - 0.2f));
                }
            }
        }
    }

}