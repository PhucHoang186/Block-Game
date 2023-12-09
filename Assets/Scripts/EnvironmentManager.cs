using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum LevelType
{
    Real_Level,
    Tutorial,
}

public class EnvironmentManager : MonoBehaviour
{
    public MapSpawner mapSpawner;
    public LevelType levelType;

    void Start()
    {
        GameManager.Instance.EnvManager = this;
    }

}
