using System;
using System.Collections.Generic;
using UnityEngine;

public class BikeView : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] List<GameObject> partsToColour;
    [SerializeField] GameObject body;

    public void SetColour(Material bikeGlowMaterial, Color colour)
    {
        var mat = new Material(bikeGlowMaterial);
        mat.color = colour;
        partsToColour.ForEach(p => p.GetComponent<MeshRenderer>().material = mat);
    }

    public void SetVisible(bool isVisible)
    {
        body.SetActive(isVisible);
    }

    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }
}
