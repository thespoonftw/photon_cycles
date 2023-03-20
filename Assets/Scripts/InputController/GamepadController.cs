using System;
using UnityEngine;

public class GamepadController : IInputController
{
    private readonly string xAxisString;
    private readonly string tAxisString;

    public GamepadController(int controllerNumber, bool isLeftJoystick)
    {
        var side = isLeftJoystick ? "Left" : "Right";
        var controllerName = $"Controller{controllerNumber}-{side}";
        xAxisString = $"{controllerName}-X";
        tAxisString = $"{controllerName}-T";
    }

    public bool IsButton1Held()
    {
        return Input.GetAxis(tAxisString) > 0.5f;
    }

    public bool IsButton2Held()
    {
        return false;
    }

    public int GetXAxis()
    {
        var value = Input.GetAxis(xAxisString);

        if (value < -0.5f)
            return -1;
        else if (value > 0.5f)
            return 1;

        return 0;
    }
}
