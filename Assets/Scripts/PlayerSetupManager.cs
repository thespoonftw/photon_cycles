using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSetupManager
{
    private List<Color> availableColors = new List<Color>()
    {
        Color.cyan,
        new Color(1, 0.5f, 0),
        Color.magenta,
        Color.green,
        Color.yellow,
        Color.red,
        Color.blue,
        new Color(0.5f, 0, 1)
    };

    private List<Player> activePlayers = new();
    private List<IInputController> inactiveControllers = IInputController.allControllers;
    private Dictionary<Player, float> holdDownTime = new();
    private BikeManager bikeManager;
    private Updater updater;

    private const float HOLD_DOWN_TIME_SEC = 2f;

    private Action<List<Player>> startGameCallback;

    public PlayerSetupManager(BikeManager bikeManager, Action<List<Player>> startGameCallback, Updater updater, List<Player> initialPlayers)
    {
        this.bikeManager = bikeManager;
        this.startGameCallback = startGameCallback;
        this.updater = updater;
        initialPlayers.ForEach(p => AddPlayer(p.Input));
        updater.OnUpdate += Update;
    }

    private void FinishSetup()
    {
        updater.OnUpdate -= Update;
        bikeManager.RemoveAllPlayers();
        startGameCallback.Invoke(activePlayers);
    }

    private void Update()
    {
        // add controllers that have movement
        inactiveControllers.Where(j => j.GetXAxis() != 0).ToList().ForEach(j => AddPlayer(j));

        // measure hold down time
        activePlayers.ForEach(j => MeasureHoldDownTime(j));

        // remove controllers with sufficient hold down time
        holdDownTime.Where(pair => pair.Value > HOLD_DOWN_TIME_SEC).Select(pair => pair.Key).ToList().ForEach(j => RemovePlayer(j));

        if (Input.GetKey(KeyCode.Return)) FinishSetup();
        
    }

    private void AddPlayer(IInputController input)
    {
        var newColor = availableColors[0];
        availableColors.Remove(newColor);
        var player = new Player(input, newColor);
        activePlayers.Add(player);
        inactiveControllers.Remove(input);
        holdDownTime.Add(player, 0);
        bikeManager.CreateBikeForPlayer(player);
        player.Bike.StartMovingBikeForPlayerSelect();
    }

    private void RemovePlayer(Player player)
    {       
        availableColors.Add(player.Color);
        activePlayers.Remove(player);
        inactiveControllers.Add(player.Input);
        holdDownTime.Remove(player);
        bikeManager.RemoveBikeForPlayer(player);
    }

    private void MeasureHoldDownTime(Player player)
    {
        if (player.Input.IsButton2Held())
        {
            holdDownTime[player] += Time.deltaTime;            
        }
        else
        {
            holdDownTime[player] = 0;
        }
    }    
}
