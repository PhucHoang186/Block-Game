using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] LevelConfig levelConfig;
    [SerializeField] Button playButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button exitButton;

    private void Start()
    {
        TransitionManager.Instance.OnCallTransitionIn(false);
        playButton.onClick.AddListener(OnPlayButtonPress);
        continueButton.onClick.AddListener(OnContinueButtonPress);
        exitButton.onClick.AddListener(OnExitButtonPress);
        continueButton.transform.parent.gameObject.SetActive(levelConfig.maxLevel > 0);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayButtonPress);
        continueButton.onClick.RemoveListener(OnContinueButtonPress);
        exitButton.onClick.RemoveListener(OnExitButtonPress);
    }

    public void OnPlayButtonPress()
    {
        levelConfig.ResetLevel();
        LoadLevelSelectScene();
    }

    public void OnContinueButtonPress()
    {
        LoadLevelSelectScene();
    }

    public void OnExitButtonPress()
    {
        Application.Quit();
    }

    private void LoadLevelSelectScene()
    {
        StartCoroutine(CorLoadLevelSelectScene());
    }

    private IEnumerator CorLoadLevelSelectScene()
    {
        TransitionManager.Instance.OnCallTransitionIn(true);
        yield return new WaitForSeconds(1f);
        LoadSceneManager.Instance.LoadScene(SceneName.LEVEL_SELECT);
    }

}
