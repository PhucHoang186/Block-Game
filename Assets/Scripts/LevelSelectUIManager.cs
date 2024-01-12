using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUIManager : MonoBehaviour
{
    [SerializeField] LevelButton levelButtonPrefab;
    [SerializeField] Transform content;
    [SerializeField] LevelSelectTutorial levelSelectTutorial;
    private LevelConfig levelConfig;
    private List<LevelButton> levelButtons;

    void Start()
    {
        levelConfig = GameDataManager.Instance.levelConfig;
        TransitionManager.Instance.OnCallTransitionIn(false);
        GenerateLevelButtons();
        if (levelConfig.maxLevel == 0)
            levelSelectTutorial.ShowFTUE(levelButtons.Count - 1, content);
    }

    public void OnBackButtonPress()
    {
        LoadMainMenuScene();
    }

    private void GenerateLevelButtons()
    {
        levelButtons = new();
        for (int i = levelConfig.levelDatas.Count - 1; i >= 0; i--)
        {
            var levelButton = Instantiate(levelButtonPrefab, content);
            levelButton.InitAction(LoadLevelScene);
            levelButton.Init(i + 1, i > levelConfig.maxLevel);
            levelButtons.Add(levelButton);
        }
        levelButtons.Reverse();
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
