using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum FTUE_State
{
    NewFTUE,
    Intro_Level_Select,
    Intro_Level_0,
    Intro_Level_1,
}

public class GameDataManager : MonoBehaviour
{
    [SerializeField] FTUE_State cheatFTUEState;
    public static GameDataManager Instance;
    public LevelConfig levelConfig;

    public bool IsTutorialLevel
    {
        get
        {
            return levelConfig.currentLevel == 0;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ResetSave()
    {
        levelConfig.ResetLevel();
        SaveFTUEStep(FTUE_State.NewFTUE);
    }

    public ActionType GetLevelActionButtonType()
    {
        return levelConfig.GetLevelActionButtonType();
    }

    public void SaveFTUEStep(FTUE_State fTUE_State)
    {
        PlayerPrefs.SetInt("FTUE_State", (int)fTUE_State);
    }

    public int GetFTUEState()
    {
        int value = PlayerPrefs.GetInt("FTUE_State", 0);
        return value;
    }
    [Button]
    public void CheatFTUEState()
    {
        PlayerPrefs.SetInt("FTUE_State", (int)cheatFTUEState);
    }
}
