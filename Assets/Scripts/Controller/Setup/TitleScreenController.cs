using System;
using UnityEngine;

public class TitleScreenController
{
    private readonly Updater updater;
    private readonly Action<IInputController> callback;
    private readonly GameObject canvasGo;

    public TitleScreenController(Resourcer resourcer, Updater updater, Action<IInputController> callback)
    {
        this.updater = updater;
        this.callback = callback;
        canvasGo = GameObject.Instantiate(resourcer.titleScreenCanvasPrefab);
        updater.OnUpdate += Update;
    }

    private void Update()
    {
        foreach (var c in IInputController.allControllers)
        {
            if (c.GetProceedDown())
            {
                Complete(c);
                return;
            }
        }
    }

    private void Complete(IInputController controller)
    {
        updater.OnUpdate -= Update;
        GameObject.Destroy(canvasGo);
        callback.Invoke(controller);
    }
}
