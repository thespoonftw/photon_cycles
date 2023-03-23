
using UnityEngine;

public class Player   
{
    public IInputController Input { get; private set; }

    public Color Color { get; private set; }

    public Player(IInputController input, Color color)
    {
        this.Input = input;
        this.Color = color;
    }
}
