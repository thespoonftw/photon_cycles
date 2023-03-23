using System;
using UnityEngine;

public class BikeController
{
    public int Points { get; private set; }

    private readonly Player player;
    private readonly TrailController trailBuilder;
    private readonly MovementController bikeMover;
    private readonly BoostController boostController;
    private readonly Transform spawn;
    private readonly BikeView bikeView;
    private readonly Updater updater;
    private readonly Resourcer resourcer;

    private bool isMoving = false;
    private bool isTrailEnabled = false;
    private Camera camera;
    private BikeCanvas bikeCanvas;
    private int points;

    public event Action OnDeath;

    public BikeController(Player player, Transform spawn, Resourcer resourcer, Updater updater)
    {
        this.spawn = spawn;
        this.player = player;
        this.updater = updater;
        this.resourcer = resourcer;

        var go = UnityEngine.Object.Instantiate(resourcer.bikePrefab, spawn.position, spawn.rotation);
        bikeView = go.GetComponent<BikeView>();
        bikeView.SetColour(resourcer.bikeGlowMaterial, player.Color);

        var cameraParent = bikeView.GetCameraTransform();
        var cameraGo = new GameObject("camera");
        cameraGo.transform.SetParent(cameraParent, false);
        camera = cameraGo.AddComponent<Camera>();
        camera.fieldOfView = 70;
        var canvasGo = GameObject.Instantiate(resourcer.bikeCanvasPrefab);
        bikeCanvas = canvasGo.GetComponent<BikeCanvas>();
        var canvas = canvasGo.GetComponent<Canvas>();
        canvas.worldCamera = camera;
        canvas.planeDistance = 1;

        trailBuilder = new TrailController(player, bikeView.transform, resourcer.trailMaterial);
        bikeMover = new MovementController(player.Input, bikeView.transform);
        boostController = new BoostController(player.Input, bikeCanvas);

        updater.OnUpdate += Update;
    }

    public void SetCenterText(string value) => bikeCanvas.SetCenterText(value);

    public void SetTrailEnabled(bool isEnabled) => isTrailEnabled = isEnabled;

    public void StartMoving() => isMoving = true;

    public void PauseBikeAtEndOfRound() => isMoving = false;

    public void MoveCamera(Rect cameraRect) => camera.rect = cameraRect;

    public void SetPoints(int value)
    {
        Points = value;
        bikeCanvas.SetPoints(Points.ToString());
    }

    public void Reset()
    {
        bikeView.transform.position = spawn.position;
        bikeView.transform.rotation = spawn.rotation;
        bikeView.SetVisible(true);
        boostController.Reset();
        trailBuilder?.ClearTrail();
        SetCenterText("");
    }

    public void Remove()
    {
        updater.OnUpdate -= Update;
        GameObject.Destroy(bikeView.gameObject);
        GameObject.Destroy(bikeCanvas.gameObject);
    }

    private void Update()
    {
        if (!isMoving) 
            return;

        if (bikeMover.IsColliding())
        {
            Kill();
        }

        var isBoosting = boostController.IsBoosting();
        bikeMover.UpdatePosition(isBoosting);

        if (isTrailEnabled)
        {
            trailBuilder.AddTrail();
        }        
    }

    private void Kill()
    {
        isMoving = false;
        bikeView.SetVisible(false);
        OnDeath?.Invoke();
    }
}
