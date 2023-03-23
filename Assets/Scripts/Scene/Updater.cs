using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Updater : MonoBehaviour
{
    public event Action OnUpdate;

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    public void DelayAction(float delay, Action action)
    {
        StartCoroutine(DelayAsync(delay, action));
    }

    public SpawnLocater FindSpawnLocater() 
    {
        return FindObjectOfType<SpawnLocater>();
    }

    public void LoadScene(int sceneIndex, Action postLoadAction)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex, postLoadAction));
    }

    public void UnloadScene(int sceneIndex, Action postUnloadAction)
    {
        StartCoroutine(UnloadSceneAsync(sceneIndex, postUnloadAction));
    }

    private IEnumerator DelayAsync(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    private IEnumerator LoadSceneAsync(int sceneIndex, Action postLoadAction)
    {
        yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
        postLoadAction.Invoke();
    }

    private IEnumerator UnloadSceneAsync(int sceneIndex, Action postUnloadAction)
    {
        yield return SceneManager.UnloadSceneAsync(sceneIndex);
        postUnloadAction.Invoke();
    }
}
