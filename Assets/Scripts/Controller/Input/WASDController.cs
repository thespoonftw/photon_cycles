using UnityEngine;

public class WASDController : IInputController
{
    public bool IsButton1Held()
    {
        return Input.GetKey(KeyCode.W);
    }

    public bool IsButton2Held()
    {
        return Input.GetKey(KeyCode.S);
    }

    public int GetXAxis()
    {
        if (Input.GetKey(KeyCode.A))
            return -1;
        else if (Input.GetKey(KeyCode.D))
            return 1;

        return 0;
    }
}
