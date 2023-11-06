using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Play,
    Win,
    Lose,
    Replay
}

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelConfig levelConfig;
    public static GameManager Instance;
    public GameState gameState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        OnChangeState(GameState.Play);
    }

    public void OnChangeState(GameState newState)
    {
        if (gameState == newState)
            return;
        gameState = newState;
        switch (gameState)
        {
            case GameState.Play:
                OnStartGame();
                break;
            case GameState.Win:
                OnWinGame();
                break;
            case GameState.Replay:
                OnReplayGame();
                break;
            case GameState.Lose:
                OnLoseGame();
                break;
        }
    }

    private void OnStartGame()
    {
        LoadSceneManager.Instance.LoadSceneAsync(levelConfig.GetCurrentLevel().levelScene, () =>
        {
            TransitionManager.Instance.OnCallTransitionIn(false);
            UIManager.Instance.ResetCommands();
            OnChangeState(GameState.Play);
        });
    }

    //they are the same for now
    private void OnWinGame()
    {
        LoadNextScene();
    }

    private void OnReplayGame()
    {
        LoadCurrentScene();
    }

    private void OnLoseGame()
    {
        LoadCurrentScene();
    }

    private void LoadNextScene()
    {
        StartCoroutine(CorLoadNextScene());
    }

    private IEnumerator CorLoadNextScene()
    {
        TransitionManager.Instance.OnCallTransitionIn(true);
        yield return new WaitForSeconds(1f);
        LoadSceneManager.Instance.UnLoadSceneAsync(levelConfig.GetCurrentLevel().levelScene);
        levelConfig.UpdateLevel();
        if (levelConfig.CheckIfReachMaxLevel())
        {
            LoadSceneManager.Instance.LoadScene(SceneName.MAIN_MENU);
        }
        else
        {
            OnChangeState(GameState.Play);
        }
    }

    private void LoadCurrentScene()
    {
        StartCoroutine(CorLoadCurrentScene());
    }

    private IEnumerator CorLoadCurrentScene()
    {
        TransitionManager.Instance.OnCallTransitionIn(true);
        yield return new WaitForSeconds(1f);
        LoadSceneManager.Instance.UnLoadSceneAsync(levelConfig.GetCurrentLevel().levelScene);
        OnChangeState(GameState.Play);
    }

    public void LoadMainMenuScene()
    {
        StartCoroutine(CorLoadMainMenuScene());
    }

    private IEnumerator CorLoadMainMenuScene()
    {
        TransitionManager.Instance.OnCallTransitionIn(true);
        yield return new WaitForSeconds(1f);
        LoadSceneManager.Instance.LoadScene(SceneName.MAIN_MENU);

    }

    public void LoadLevelSelectScene()
    {
        StartCoroutine(CorLoadLevelSelectScene());
    }

    private IEnumerator CorLoadLevelSelectScene()
    {
        TransitionManager.Instance.OnCallTransitionIn(true);
        yield return new WaitForSeconds(1f);
        LoadSceneManager.Instance.LoadScene(SceneName.MAIN_MENU);

    }
}
