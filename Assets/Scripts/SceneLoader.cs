using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField, Min(0)]
    private float _timeBetweenOpenClose = 0.5f;

    [SerializeField, Min(0)]
    private float _waitAfterStartup = 1f;

    private SceneTransitions _transitions;

    public event Action StartLoadingScene;
    public event Action FinishedLoadingScene;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _transitions = GetComponentInChildren<SceneTransitions>();
    }


    private IEnumerator Start()
    {
        _transitions.Hide(); // "Close elevator door" on scene Startup
        yield return new WaitForSeconds(_waitAfterStartup);
        StartCoroutine(_transitions.PlayOpeningAnimationCR());
    }


    public IEnumerator LoadScene(int levelID)
    {
        StartLoadingScene?.Invoke();
        yield return StartCoroutine(_transitions.PlayClosingAnimationCR());
        yield return StartCoroutine(LoadeSceneAsync(levelID));
        AudioManager.Instance.PlayElevatorDing();
        yield return new WaitForSeconds(_timeBetweenOpenClose);
        yield return StartCoroutine(_transitions.PlayOpeningAnimationCR());
        FinishedLoadingScene?.Invoke();
    }

    private IEnumerator LoadeSceneAsync(int levelID)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync($"Level {levelID}");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator LoadLevel(int levelID)
    {
        yield return StartCoroutine(LoadScene(levelID));
    }

    public IEnumerator QuitGame()
    {
        yield return StartCoroutine(_transitions.PlayClosingAnimationCR());
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
