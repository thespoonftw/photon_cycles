
using UnityEngine;

public class Player   
{
    public int Id { get; private set; }

    public IInputController Input { get; private set; }

    public Color Color { get; private set; }


    public Player(int id, IInputController input, Color color)
    {
        Id = id;
        Input = input;
        Color = color;
    }
}
