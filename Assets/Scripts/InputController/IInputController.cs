using System.Collections.Generic;

public interface IInputController
{
    public int GetXAxis();

    public bool IsButton1Held();

    public bool IsButton2Held();

    public static List<IInputController> GetAllControllers()
    {
        return new()
        {
            new GamepadController(1, true),
            new GamepadController(1, false),
            new GamepadController(2, true),
            new GamepadController(2, false),
            new GamepadController(3, true),
            new GamepadController(3, false),
            new GamepadController(4, true),
            new GamepadController(4, false),
            new WASDController(),
            new ArrowsController()
        };
    }
}
