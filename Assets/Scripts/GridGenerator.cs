using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int height;
    public int weight;
    [SerializeField] Node nodePref;

    public Dictionary<Vector3, Node> Init(Dictionary<Vector3, Node> _grids, float _nodeSize)
    {
        _grids = new Dictionary<Vector3, Node>();
        GenerateGrid(_grids, _nodeSize);
        return _grids;
    }

    void GenerateGrid(Dictionary<Vector3, Node> _grids, float _nodeSize)
    {
        for (int i = 0; i < weight; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Node newNode = Instantiate(nodePref);
                newNode.transform.parent = transform;
                newNode.transform.position = new Vector3(i, 0f, j) * _nodeSize;
                newNode.X = i;
                newNode.Y = j;
                _grids.Add(new Vector3(i, 0f, j), newNode);
            }
        }
    }
}