using System.Collections.Generic;

public interface IInputController
{
    public int GetHorizontal();

    public int GetHorizontalDown();

    public int GetVertical();

    public int GetVerticalDown();

    public bool GetBoost();

    public bool GetAction();

    public bool GetProceedDown();

    public bool GetReturnDown();

    public string GetName();
}
