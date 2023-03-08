using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<TMP_Dropdown> dropDowns;
    [SerializeField] List<GameObject> cameras;
    [SerializeField] GameObject bikePrefab;    
    [SerializeField] Transform spawnLocation;

    private GameObject[] bikes = new GameObject[8];

    private List<string> options = new List<string>()
    {
        "Closed",
        "Controller 1 - Left Joystick",
        "Controller 2 - Left Joystick",
        "Controller 3 - Left Joystick",
        "Controller 4 - Left Joystick",
        "Controller 1 - Right Joystick",
        "Controller 2 - Right Joystick",
        "Controller 3 - Right Joystick",
        "Controller 4 - Right Joystick",
        "Keyboard - Arrows",
        "Keyboard - WASD"
    };

    private List<string> axes = new List<string>()
    {
        string.Empty,
        "J2X",
        "J4X",
        "J6X",
        "J8X",
        "J1X",
        "J3X",
        "J5X",
        "J7X",
        "Arrows",
        "WASD"
    };

    private void Start()
    {
        for (int i=0; i< dropDowns.Count; i++)
        {
            var d = dropDowns[i];
            d.ClearOptions();
            d.AddOptions(options);
            int index = i;
            d.onValueChanged.AddListener(delegate {
                DropDown(index, d.value);
            });
        }
    }

    private void DropDown(int playerIndex, int dropdownIndex)
    {
        var existingBike = bikes[playerIndex];
        if (existingBike != null)
        {
            cameras[playerIndex].transform.parent = transform;
            cameras[playerIndex].transform.localPosition = Vector3.zero;
            cameras[playerIndex].transform.localRotation = Quaternion.identity;
            Destroy(existingBike);
        }

        if (dropdownIndex != 0)
        {
            var newBike = Instantiate(bikePrefab, spawnLocation.position, spawnLocation.rotation);
            bikes[playerIndex] = newBike;
            var bikeController = newBike.GetComponent<BikeController>();
            cameras[playerIndex].transform.parent = bikeController.GetCameraTransform();
            bikeController.SetAxis(axes[dropdownIndex]);
        }
    }
}
