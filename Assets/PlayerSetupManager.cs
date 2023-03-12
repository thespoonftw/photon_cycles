using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSetupManager : MonoBehaviour
{
    [SerializeField] CameraLoader cameraLoader;
    [SerializeField] GameObject bikePrefab;

    private List<Joystick> activeJoysticks = new List<Joystick>();
    private List<Joystick> inactiveJoysticks = JoystickHelper.GetControllerTypes().ToList();
    private List<BikeController> bikes = new List<BikeController>();
    private List<GameObject> cameras = new List<GameObject>();
    private Dictionary<Joystick, float> holdDownTime = new();
    private SpawnLocater spawnLocater;

    private int NumberOfPlayers => activeJoysticks.Count();

    public void Init(SpawnLocater spawnLocater)
    {
        this.spawnLocater = spawnLocater;
    }

    private const float HOLD_DOWN_TIME_SEC = 2f;

    private void Update()
    {
        // add controllers that have movement
        inactiveJoysticks.Where(j => j.GetXAxis() != 0).ToList().ForEach(j => AddPlayer(j));

        // measure hold down time
        activeJoysticks.ForEach(j => MeasureHoldDownTime(j));

        // remove controllers with sufficient hold down time
        holdDownTime.Where(pair => pair.Value > HOLD_DOWN_TIME_SEC).Select(pair => pair.Key).ToList().ForEach(j => RemovePlayer(j));

        
    }

    private void AddPlayer(Joystick joystick)
    {
        activeJoysticks.Add(joystick);
        inactiveJoysticks.Remove(joystick);
        holdDownTime.Add(joystick, 0);

        var spawn = spawnLocater.GetSpawnForPlayer(GetPlayerIndex(joystick));
        var go = Instantiate(bikePrefab, spawn.position, spawn.rotation);
        var bikeController = go.GetComponent<BikeController>();
        bikes.Add(bikeController);
        bikeController.SetJoystick(joystick);

        RefreshCameras();
    }

    private void RemovePlayer(Joystick joystick)
    {
        var playerIndex = GetPlayerIndex(joystick);
        var bike = bikes[playerIndex];
        bikes.Remove(bike);
        Destroy(bike.gameObject);

        activeJoysticks.Remove(joystick);
        inactiveJoysticks.Add(joystick);
        holdDownTime.Remove(joystick);       

        RefreshCameras();
    }

    private void RefreshCameras()
    {
        foreach (var c in cameras)
        {
            Destroy(c);
        }

        cameras = new List<GameObject>();
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            var cameraParent = bikes[i].GetCameraTransform();
            var cameraPrefab = cameraLoader.GetCameraPrefab(i, NumberOfPlayers);
            var cameraGo = Instantiate(cameraPrefab, cameraParent.position, cameraParent.rotation, cameraParent.transform);
            var camera = cameraGo.GetComponent<Camera>();
            camera.fieldOfView = 70;
            cameras.Add(cameraGo);
        }
    }

    private void MeasureHoldDownTime(Joystick joystick)
    {
        if (joystick.GetYAxis() == -1)
        {
            holdDownTime[joystick] += Time.deltaTime;            
        }
        else
        {
            holdDownTime[joystick] = 0;
        }
    }

    private int GetPlayerIndex(Joystick joystick) => activeJoysticks.FindIndex(j => j == joystick);
}
