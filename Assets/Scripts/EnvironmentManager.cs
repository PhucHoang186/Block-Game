using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum LevelType
{
    Real_Level,
    Tutorial_Intro_Game,
    Tutorial_Rotate_Button,
}

public class EnvironmentManager : MonoBehaviour
{
    public MapSpawner mapSpawner;
    public LevelType levelType;

    void Start()
    {
        GameManager.Instance.EnvManager = this;
    }

    public void ShowEndPointIntro()
    {
        mapSpawner.Objective.ShowEndPointIntro();
    }

    public void ShowPlayerIntro()
    {
        mapSpawner.Player.ShowPlayerIntro();
    }
}
