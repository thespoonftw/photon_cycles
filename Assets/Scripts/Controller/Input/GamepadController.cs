using System;
using UnityEngine;

public class GamepadController : IInputController
{
    private readonly string xAxisString;
    private readonly string tAxisString;
    private readonly string yAxisString;
    private readonly KeyCode proceedCode1 = 0;
    private readonly KeyCode proceedCode2 = 0;
    private readonly KeyCode returnCode1 = 0;
    private readonly KeyCode returnCode2 = 0;
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

        var keyCodeBase = controllerNumber * 20 + 330;

        if (isLeftJoystick)
        {
            proceedCode1 = (KeyCode)(keyCodeBase);
            proceedCode2 = (KeyCode)(keyCodeBase + 7);
            returnCode1 = (KeyCode)(keyCodeBase + 6);
            returnCode2 = (KeyCode)(keyCodeBase + 1);
            actionCode = (KeyCode)(keyCodeBase + 4);
        } else
        {
            actionCode = (KeyCode)(keyCodeBase + 5);
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

    public bool GetProceedDown()
    {
        if (proceedCode1 == 0 && proceedCode2 == 0)
            return false;

        return Input.GetKeyDown(proceedCode1) || Input.GetKeyDown(proceedCode2);
    }

    public bool GetReturnDown()
    {
        if (returnCode1 == 0 && returnCode2 == 0)
            return false;

        return Input.GetKeyDown(returnCode1) || Input.GetKeyDown(returnCode2);
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
