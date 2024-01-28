using System;
using System.Collections;
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


    public IEnumerator LoadScene(int sceneBuildIndex)
    {
        StartLoadingScene?.Invoke();
        yield return StartCoroutine(_transitions.PlayClosingAnimationCR());
        yield return StartCoroutine(LoadSceneAsync(sceneBuildIndex));
        AudioManager.Instance.PlayElevatorDing();
        yield return new WaitForSeconds(_timeBetweenOpenClose);
        yield return StartCoroutine(_transitions.PlayOpeningAnimationCR());
        FinishedLoadingScene?.Invoke();
    }

    private IEnumerator LoadSceneAsync(int sceneBuildIndex)
    {
        string pathToScene = SceneUtility.GetScenePathByBuildIndex(sceneBuildIndex);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator LoadLevel(int sceneBuildIndex)
    {
        yield return StartCoroutine(LoadScene(sceneBuildIndex));
    }

    public IEnumerator LoadLevel(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        yield return StartCoroutine(LoadScene(scene.buildIndex));
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
