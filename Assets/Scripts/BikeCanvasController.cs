using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BikeCanvasController : MonoBehaviour
{
    [SerializeField] Slider boostSlider;
    [SerializeField] TextMeshProUGUI pointsTextMesh;

    public void SetBoost(float boostPercentage)
    {
        boostSlider.value = boostPercentage;
    }

    public void SetPoints(string value)
    {
        pointsTextMesh.text = value;
    }
}
