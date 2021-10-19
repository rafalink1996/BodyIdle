using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        StartCoroutine(loadSceneAsync());
    }
    IEnumerator loadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            yield return null;
        }
    }
}
