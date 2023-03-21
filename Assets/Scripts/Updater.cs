using System;
using System.Collections;
using UnityEngine;

public class Updater : MonoBehaviour
{
    public event Action OnUpdate;

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    public void DelayAction(Action action, float delay)
    {
        StartCoroutine(DelayAsync(action, delay));
    }

    private IEnumerator DelayAsync(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
