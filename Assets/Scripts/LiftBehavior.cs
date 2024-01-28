using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiftBehavior : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {
        var highestBuildIndex = 0;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex > highestBuildIndex)
                highestBuildIndex = scene.buildIndex;
        }
        yield return SceneLoader.Instance.LoadLevel(highestBuildIndex + 1);
    }
}
