using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Joystick   
{
    private static List<Joystick> joysticks = null;

    private static List<string> joystickStrings = new()
    {
        "J1",
        "J2",
        "J3",
        "J4",
        "J5",
        "J6",
        "J7",
        "J8",
        "Arrows",
        "WASD"
    };

    private string xAxisName;
    private string yAxisName;

    private Joystick(string joystickName)
    {
        xAxisName = joystickName + "-X";
        yAxisName = joystickName + "-Y";
    }

    public static List<Joystick> GetAllJoysticks()
    {
        joysticks ??= joystickStrings.Select(s => new Joystick(s)).ToList();
        return joysticks;
    }

    public int GetXAxis() => RawToInt(Input.GetAxis(xAxisName));

    public int GetYAxis() => RawToInt(Input.GetAxis(yAxisName));

    private int RawToInt(float value)
    {
        if (value < -0.5f)
            return -1;
        else if (value > 0.5f)
            return 1;

        return 0;
    }
}
