using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{

    public static LoadSceneManager Instance;

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


    public void LoadScene(string sceneName, Action cb = null)
    {
        
        LoadSceneAsync(sceneName, cb, LoadSceneMode.Single);
    }

    public void LoadSceneAsync(string sceneName, Action cb = null, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        StartCoroutine(CorLoadSceneAsync(sceneName, cb, loadSceneMode));
    }

    private IEnumerator CorLoadSceneAsync(string sceneName, Action cb = null, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        while (!operation.isDone)
        {
            yield return null;
        }
        cb?.Invoke();
    }

    public void UnLoadSceneAsync(string sceneName, Action cb = null)
    {
        StartCoroutine(UnCorLoadSceneAsync(sceneName, cb));
    }

    private IEnumerator UnCorLoadSceneAsync(string sceneName, Action cb = null)
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
        cb?.Invoke();
    }
}

public class SceneName
{
    public static string MAIN_MENU = "MainMenu";
    public static string LEVEL_SELECT = "LevelSelect";
    public static string MAIN_LEVEL = "MainLevel";
}