using System.Collections.Generic;
using UnityEngine;

public enum Joystick
{
    J1,
    J2,
    J3,
    J4,
    J5,
    J6,
    J7,
    J8,
    Arrows,
    WASD
}

public static class JoystickHelper   
{
    public static Joystick[] GetControllerTypes()
    {
        return (Joystick[])Joystick.GetValues(typeof(Joystick));
    }

    public static int GetXAxis(this Joystick type) => RawToInt(type.GetXRaw());

    public static int GetYAxis(this Joystick type) => RawToInt(type.GetYRaw());

    private static float GetXRaw(this Joystick type) => Input.GetAxis(type.ToString() + "-X");

    private static float GetYRaw(this Joystick type) => Input.GetAxis(type.ToString() + "-Y");

    private static int RawToInt(float value)
    {
        if (value < -0.5f)
            return -1;
        else if (value > 0.5f)
            return 1;

        return 0;
    }

}
