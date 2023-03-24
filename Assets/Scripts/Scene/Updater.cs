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

    private IEnumerator DelayAsync(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
