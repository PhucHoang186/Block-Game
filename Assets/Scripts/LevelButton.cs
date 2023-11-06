using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] TMP_Text levelText;
    [SerializeField] Image lockLevelIcon;
    [SerializeField] Button levelButton;
    private Action<LevelButton> onLoadLevelSceneCb;

    public void InitAction(Action<LevelButton> onLoadLevelScene)
    {
        onLoadLevelSceneCb = onLoadLevelScene;
    }

    public void Init(int level, bool isLock)
    {
        levelText.text = level.ToString();
        lockLevelIcon.gameObject.SetActive(isLock);
        levelButton.interactable = !isLock;
    }

    public void LoadLevelScene()
    {
        onLoadLevelSceneCb?.Invoke(this);
    }
}
