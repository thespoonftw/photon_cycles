
using UnityEngine;

public class Player   
{
    public Joystick Joystick { get; private set; }

    public BikeController Bike { get; private set; }

    public Camera Camera { get; private set; }

    public Color Color { get; private set; }

    public Player(Joystick joystick, Color color)
    {
        this.Joystick = joystick;
        this.Color = color;
    }

    public void SetBike(BikeController bike)
    {
        Bike = bike;
    }

    public void SetCamera(Camera camera)
    {
        Camera = camera;
    }

    public void RemoveBike()
    {
        Bike = null;
        Camera = null;
    }
}
