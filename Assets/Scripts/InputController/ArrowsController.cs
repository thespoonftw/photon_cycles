using UnityEngine;

public class ArrowsController : IInputController
{
    public bool IsButton1Held()
    {
        return Input.GetKey(KeyCode.UpArrow);
    }

    public bool IsButton2Held()
    {
        return Input.GetKey(KeyCode.DownArrow);
    }

    public int GetXAxis()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            return -1;
        else if (Input.GetKey(KeyCode.RightArrow))
            return 1;

        return 0;
    }
}
