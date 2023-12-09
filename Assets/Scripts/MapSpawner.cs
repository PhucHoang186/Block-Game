using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [Header("Preferences")]
    [SerializeField] GameObject pathPrefab;
    [SerializeField] List<GameObject> offPathPrefabs;
    [SerializeField] PlayerCharacter playerPrefab;
    [SerializeField] GameObject flagPrefab; // end point
    [Space(10)]
    [Header("Settings")]
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;

    public PlayerCharacter Player {get; set;}
    public GameObject Objective {get; set;}

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SpawnMap();
        SpawnInteractObjects();
    }

    private void SpawnMap()
    {
        // get path
        GridManager.Instance.Init();
        Node startNode = GridManager.Instance.GetNodeByPosition(startPoint.position);
        Node endNode = GridManager.Instance.GetNodeByPosition(endPoint.position);
        PathFinding.Instance.FindPath(startNode, endNode);
        // spawn map
        var path = GridManager.Instance.path;
        var grids = GridManager.Instance.Grids;
        if (path != null && path.Count > 0)
        {

            foreach (var node in grids)
            {
                // spawn path
                if (path.Contains(node.Value))
                {
                    var pathObj = Instantiate(pathPrefab, this.transform);
                    pathObj.transform.SetPositionAndRotation(node.Value.transform.position, Quaternion.identity);
                }
                // spawn surrounding
                else
                {
                    var surroundingObj = Instantiate(GetRandomPrefab(offPathPrefabs), this.transform);
                    surroundingObj.transform.SetPositionAndRotation(node.Value.transform.position, Quaternion.identity);
                    node.Value.IsPlaced = true;
                }
            }
        }
    }

    private void SpawnInteractObjects()
    {
        Player = Instantiate(playerPrefab, transform);
        Player.transform.position = startPoint.position;
        Player.CurrentNodeOn = GridManager.Instance.GetNodeByPosition(startPoint.position);
        
        var endFlagObj = Instantiate(flagPrefab, transform);
        endFlagObj.transform.position = endPoint.position;
        var endNode = GridManager.Instance.GetNodeByPosition(endPoint.position);
        Objective = endFlagObj;
        endNode.IsEndNode = true;
    }

    public GameObject GetRandomPrefab(List<GameObject> randomPrefabList)
    {
        var index = Random.Range(0, randomPrefabList.Count);
        return randomPrefabList[index];
    }
}
