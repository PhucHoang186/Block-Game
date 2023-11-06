using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCheat : MonoBehaviour
{
    [SerializeField] LevelConfig levelConfig;

    public void ResetLevel()
    {
        levelConfig.ResetLevel();
    }
}
