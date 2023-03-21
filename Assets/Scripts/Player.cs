
using UnityEngine;

public class Player   
{
    public IInputController Input { get; private set; }

    public BikeController Bike { get; private set; }

    public Camera Camera { get; private set; }

    public Color Color { get; private set; }

    public GameObject CanvasGo { get; private set; }

    public BikeCanvasController Canvas { get; private set; }

    public int Points { get; private set; }

    public Player(IInputController input, Color color)
    {
        this.Input = input;
        this.Color = color;
    }

    public void SetBike(BikeController bike)
    {
        Bike = bike;
    }

    public void SetCamera(Camera camera, GameObject canvasGo)
    {
        Camera = camera;
        CanvasGo = canvasGo;
        Canvas = CanvasGo.GetComponent<BikeCanvasController>();
    }

    public void AddPoint()
    {
        Points++;
        Canvas.SetPoints(Points.ToString());
    }

    public void RemoveBike()
    {
        Bike = null;
        Camera = null;
    }
}
