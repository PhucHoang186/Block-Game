using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    LoadLevel,
    StartGame,
    Win,
    Lose,
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState gameState;
    private LevelConfig levelConfig;
    private bool isWinGame;
    private bool isLoseGame;
    public EnvironmentManager EnvManager { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        levelConfig = GameDataManager.Instance.levelConfig;
        OnChangeState(GameState.LoadLevel);
    }

    public void OnChangeState(GameState newState)
    {
        if (gameState == newState)
            return;
        gameState = newState;
        switch (gameState)
        {
            case GameState.LoadLevel:
                OnLoadLevel();
                break;
            case GameState.StartGame:
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

    public PlayerCharacter GetPlayerInLevel()
    {
        return EnvManager.mapSpawner.Player;
    }

    public GameObject GetObjectiveInLevel()
    {
        return EnvManager.mapSpawner.Objective.gameObject;
    }
    public void ShowEndPointIntro()
    {
        EnvManager.ShowEndPointIntro();
    }

    public void ShowPlayerIntro()
    {
        EnvManager.ShowPlayerIntro();
    }

    public void CheckShowTutorial()
    {
        if (EnvManager.levelType == LevelType.Tutorial_Intro_Game)
        {
            // show tutorial
            TutorialController.Instance.OnChangeTutorialState(TutorialStep.Show_Intro);
        }
        else if(EnvManager.levelType == LevelType.Tutorial_Rotate_Button)
        {
            TutorialController.Instance.OnChangeTutorialState(TutorialStep.Show_Button_Tutorial);

        }
    }

    private void OnLoadLevel()
    {
        SetWinGame(false);
        SetLoseGame(false);
        LoadSceneManager.Instance.LoadSceneAsync(levelConfig.GetCurrentLevel().levelScene, () =>
        {
            TransitionManager.Instance.OnCallTransitionIn(false);
            OnChangeState(GameState.StartGame);
        });
    }

    private void OnStartGame()
    {
        UIManager.Instance.ResetCommands();
        CheckShowTutorial();
    }

    private void OnWinGame()
    {
        StartCoroutine(CorOnWinGame());
    }

    private IEnumerator CorOnWinGame()
    {
        GetPlayerInLevel().Celebrate();
        yield return new WaitForSeconds(2f); // wait for win animation
        LoadNextScene();
    }

    private void OnLoseGame()
    {
        StartCoroutine(CorOnLoseGame());
    }

    private IEnumerator CorOnLoseGame()
    {
        GetPlayerInLevel().OnLoseGame();
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
            OnChangeState(GameState.LoadLevel);
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
        OnChangeState(GameState.LoadLevel);
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
        LoadSceneManager.Instance.LoadScene(SceneName.LEVEL_SELECT);
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
