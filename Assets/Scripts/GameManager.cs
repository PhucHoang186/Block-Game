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
}

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelConfig levelConfig;
    public static GameManager Instance;
    public GameState gameState;
    private bool isWinGame;
    private bool isLoseGame;
    public MapSpawner MapSpawner { get; set; }

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
            case GameState.Lose:
                OnLoseGame();
                break;
        }
    }

    private void OnStartGame()
    {
        SetWinGame(false);
        SetLoseGame(false);
        LoadSceneManager.Instance.LoadSceneAsync(levelConfig.GetCurrentLevel().levelScene, () =>
        {
            TransitionManager.Instance.OnCallTransitionIn(false);
            UIManager.Instance.ResetCommands();
            OnChangeState(GameState.Play);
        });
    }

    private void OnWinGame()
    {
        StartCoroutine(CorOnWinGame());
    }

    private IEnumerator CorOnWinGame()
    {
        MapSpawner.Player.Celebrate();
        yield return new WaitForSeconds(2f); // wait for win animation
        LoadNextScene();
    }

    private void OnLoseGame()
    {
        StartCoroutine(CorOnLoseGame());
    }

    private IEnumerator CorOnLoseGame()
    {
        MapSpawner.Player.OnLoseGame();
        yield return new WaitForSeconds(2f); // wait for win animation
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

    public void SetWinGame(bool condition)
    {
        isWinGame = condition;
    }

    public void SetLoseGame(bool condition)
    {
        isLoseGame = condition;
    }

    public void CheckLoseCondition()
    {
        if (!isLoseGame)
            return;
        OnChangeState(GameState.Lose);
    }

    public void CheckWinCondition()
    {
        if (isWinGame)
        {
            OnChangeState(GameState.Win);
        }
        else
        {
            OnChangeState(GameState.Lose);
        }
    }
}
