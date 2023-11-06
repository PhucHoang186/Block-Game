using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu]
public class LevelConfig : ScriptableObject
{
    public List<LevelData> levelDatas;
    public int currentLevel;
    public int maxLevel;

    public LevelData GetCurrentLevel()
    {
        if (currentLevel >= levelDatas.Count)
        {
            currentLevel = levelDatas.Count - 1;
        }
        return levelDatas[currentLevel];
    }

    public void SetCurrentLevel(int index)
    {
        currentLevel = index;
    }

    public LevelData GetLevelDataIndex(int index)
    {
        if (index >= levelDatas.Count)
            return null;
        return levelDatas[index];
    }

    public LevelData GetNextLevel()
    {
        currentLevel++;
        if (currentLevel >= levelDatas.Count)
        {
            Debug.Log(" Reach Max Level");
            currentLevel = levelDatas.Count - 1;
        }
        if (currentLevel > maxLevel)
        {
            maxLevel = currentLevel;
        }
        return levelDatas[currentLevel];
    }


    public void UpdateLevel()
    {
        currentLevel++;
        maxLevel = currentLevel;
        maxLevel = Mathf.Clamp(maxLevel, 0, levelDatas.Count);
    }

    public bool CheckIfReachMaxLevel()
    {
        return currentLevel >= levelDatas.Count;

    }

    public void ResetLevel()
    {
        currentLevel = 0;
        maxLevel = 0;
    }
}

[Serializable]
public class LevelData
{
    public string levelScene;
}
