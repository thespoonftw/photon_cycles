using System.Collections.Generic;

public interface IInputController
{
    public int GetHorizontal();

    public int GetHorizontalDown();

    public int GetVertical();

    public int GetVerticalDown();

    public bool GetBoost();

    public bool GetAction();

    public bool GetStartDown();

    public static List<IInputController> allControllers = new()
    {
        new GamepadController(1, true),
        new GamepadController(1, false),
        new GamepadController(2, true),
        new GamepadController(2, false),
        new GamepadController(3, true),
        new GamepadController(3, false),
        new GamepadController(4, true),
        new GamepadController(4, false),
        new KeyboardController()
    };
}
