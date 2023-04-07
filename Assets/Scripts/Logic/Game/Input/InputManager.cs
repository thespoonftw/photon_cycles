using System.Collections.Generic;
using System.Linq;

public class InputManager
{
    private List<IInputController> allControllers = new()
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

    public IInputController LeadInput { get; set; }

    public List<IInputController> GetAllControllers()
    {
        return allControllers.ToList();
    }

    public int GetControllerIndex(IInputController controller)
    {
        return allControllers.IndexOf(controller);
    }

    public IInputController GetController(int controllerIndex)
    {
        return allControllers[controllerIndex];
    }
}
