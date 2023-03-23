using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BikeCanvas : MonoBehaviour
{
    [SerializeField] Slider boostSlider;
    [SerializeField] TextMeshProUGUI pointsTextMesh;
    [SerializeField] TextMeshProUGUI centerTextMesh;

    public void SetBoost(float boostPercentage)
    {
        boostSlider.value = boostPercentage;
    }

    public void SetPoints(string value)
    {
        pointsTextMesh.text = value;
    }

    public void SetCenterText(string value)
    {
        centerTextMesh.text = value;
    }
}
