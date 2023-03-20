using System;
using System.Collections.Generic;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] List<GameObject> partsToColour;
    [SerializeField] GameObject body;

    private TrailBuilder trailBuilder;
    private BikeMover bikeMover;

    private bool isMoving = false;
    private Transform spawn;

    public event Action OnDeath;

    public void Init(Player player, Transform spawn)
    {
        this.spawn = spawn;

        var mat = new Material(partsToColour[0].GetComponent<MeshRenderer>().material);
        mat.color = player.Color;
        partsToColour.ForEach(p => p.GetComponent<MeshRenderer>().material = mat);
        trailBuilder = GetComponent<TrailBuilder>();
        trailBuilder.Init(player);
        bikeMover = GetComponent<BikeMover>();
        bikeMover.Init(player.Input);
    }

    public Transform GetCameraTransform() => cameraTransform;

    public void ResetBikeStartOfRound()
    {
        transform.position = spawn.position;
        transform.rotation = spawn.rotation;
        SetVisible(true);
        trailBuilder.ClearTrail();
    }

    public void StartMovingBikeStartOfRound()
    {
        trailBuilder.SetEnabled(true);
        isMoving = true;
    }

    public void StartMovingBikeForPlayerSelect()
    {
        isMoving = true;
    }

    public void PauseBikeAtEndOfRound()
    {
        isMoving = false;
    }

    private void Update()
    {
        if (isMoving)
        {
            if (bikeMover.IsColliding())
            {
                Kill();
            }

            bikeMover.UpdatePosition();
        }

        trailBuilder.AddTrail();
    }

    private void Kill()
    {
        isMoving = false;
        SetVisible(false);
        OnDeath?.Invoke();
    }

    private void SetVisible(bool isVisible)
    {
        body.SetActive(isVisible);
    }
}
