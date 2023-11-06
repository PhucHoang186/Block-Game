using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public MapSpawner mapSpawner;

    public static EnvironmentManager Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
