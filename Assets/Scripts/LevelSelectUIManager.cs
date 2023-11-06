using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUIManager : MonoBehaviour
{
    [SerializeField] LevelConfig levelConfig;
    [SerializeField] LevelButton levelButtonPrefab;
    [SerializeField] Transform content;
    private List<LevelButton> levelButtons;

    void Start()
    {
        TransitionManager.Instance.OnCallTransitionIn(false);
        GenerateLevelButtons();
    }

    public void OnBackButtonPress()
    {
        LoadMainMenuScene();
    }

    private void GenerateLevelButtons()
    {
        levelButtons = new();
        for (int i = 0; i < levelConfig.levelDatas.Count; i++)
        {
            var levelButton = Instantiate(levelButtonPrefab, content);
            levelButton.InitAction(LoadLevelScene);
            levelButton.Init(i, i > levelConfig.maxLevel);
            levelButtons.Add(levelButton);
        }
    }

    void LoadLevelScene(LevelButton levelButton)
    {
        int index = levelButtons.IndexOf(levelButton);
        if (index >= levelButtons.Count)
        {
            return;
        }
        LoadLevelSelectScene(index);
    }

    private void LoadLevelSelectScene(int levelIndex)
    {
        StartCoroutine(CorLoadLevelSelectScene(levelIndex));
    }

    private IEnumerator CorLoadLevelSelectScene(int levelIndex)
    {
        var levelData = levelConfig.GetLevelDataIndex(levelIndex);
        if (levelData != null)
        {
            TransitionManager.Instance.OnCallTransitionIn(true);
            yield return new WaitForSeconds(1f);
            levelConfig.SetCurrentLevel(levelIndex);
            LoadSceneManager.Instance.LoadScene(SceneName.MAIN_LEVEL);
        }
    }

    private void LoadMainMenuScene()
    {
        StartCoroutine(CorLoadMainMenuScene());
    }

    private IEnumerator CorLoadMainMenuScene()
    {
        TransitionManager.Instance.OnCallTransitionIn(true);
        yield return new WaitForSeconds(1f);
        LoadSceneManager.Instance.LoadScene(SceneName.MAIN_MENU);

    }
}
