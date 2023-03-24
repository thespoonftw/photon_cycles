using System;
using UnityEngine;

public class GamepadController : IInputController
{
    private readonly string xAxisString;
    private readonly string tAxisString;
    private readonly string yAxisString;
    private readonly KeyCode startCode1 = 0;
    private readonly KeyCode startCode2 = 0;
    private readonly KeyCode actionCode = 0;

    private int lastFrameCount;
    private int lastXValue;
    private int lastYValue;
    private int currentXValue;
    private int currentYValue;

    public GamepadController(int controllerNumber, bool isLeftJoystick)
    {
        var side = isLeftJoystick ? "L" : "R";
        xAxisString = $"C{controllerNumber}-{side}-X";
        yAxisString = $"C{controllerNumber}-{side}-Y";
        tAxisString = $"C{controllerNumber}-{side}-T";

        if (isLeftJoystick)
        {
            startCode1 = (KeyCode)(controllerNumber * 20 + 330);
            startCode2 = (KeyCode)(controllerNumber * 20 + 337);
            actionCode = (KeyCode)(controllerNumber * 20 + 4);
        } else
        {
            actionCode = (KeyCode)(controllerNumber * 20 + 5);
        }
            
    }

    public bool GetBoost()
    {
        return Input.GetAxis(tAxisString) > 0.5f;
    }

    public bool GetAction()
    {
        return Input.GetKey(actionCode);
    }

    public int GetHorizontal()
    {
        var value = Input.GetAxis(xAxisString);

        if (value < -0.5f)
            return -1;
        else if (value > 0.5f)
            return 1;

        return 0;
    }

    public int GetVertical()
    {
        var value = Input.GetAxis(yAxisString);

        if (value < -0.5f)
            return -1;
        else if (value > 0.5f)
            return 1;

        return 0;
    }

    public bool GetStartDown()
    {
        if (startCode1 == 0 && startCode2 == 0)
            return false;

        return Input.GetKeyDown(startCode1) || Input.GetKeyDown(startCode2);
    }

    public int GetHorizontalDown()
    {
        if (lastFrameCount + 2 == Time.frameCount)
        {
            lastFrameCount++;
            lastXValue = currentXValue;
        }
        currentXValue = GetHorizontal();
        if (lastFrameCount + 1 == Time.frameCount)
        {
            return currentXValue != lastXValue ? currentXValue : 0;
        }
        lastFrameCount = Time.frameCount;
        return currentXValue;
    }

    public int GetVerticalDown()
    {
        if (lastFrameCount + 2 == Time.frameCount)
        {
            lastFrameCount++;
            lastYValue = currentYValue;
        }
        currentYValue = GetVertical();
        if (lastFrameCount + 1 == Time.frameCount)
        {
            return currentYValue != lastYValue ? currentYValue : 0;
        }
        lastFrameCount = Time.frameCount;
        return currentYValue;
    }
}
