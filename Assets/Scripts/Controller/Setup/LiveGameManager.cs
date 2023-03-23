using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LiveGameManager
{
    private const int pointsToWin = 5;

    private readonly List<BikeController> allBikes;
    private readonly Action endGameCallback;
    private readonly BikeManager bikeManager;
    private readonly Updater updater;

    private int countdown_seconds = 0;
    private List<BikeController> livingBikes = new();

    public LiveGameManager(BikeManager bikeManager, List<Player> players, Action endGameCallback, Updater updater)
    {
        this.endGameCallback = endGameCallback;
        this.bikeManager = bikeManager;
        this.updater = updater;

        allBikes = bikeManager.CreateBikesForPlayers(players);

        foreach (var b in allBikes)
        {
            b.OnDeath += () => OnBikeDeath(b);
            b.SetPoints(0);
        }

        StartRound();
    }

    private void StartRound()
    {
        countdown_seconds = 3;
        livingBikes = allBikes.ToList();
        bikeManager.GetBikes().ForEach(b => b.Reset());
        bikeManager.GetBikes().ForEach(b => b.SetCenterText(countdown_seconds.ToString()));
        updater.DelayAction(1f, CountDown);
    }

    private void CountDown()
    {
        countdown_seconds -= 1;
        bikeManager.GetBikes().ForEach(b => b.SetCenterText(countdown_seconds.ToString()));

        if (countdown_seconds > 0)
        {
            updater.DelayAction(1f, CountDown);
        } 
        else
        {
            FinishCountdown();
        }
    }

    private void FinishCountdown()
    {
        foreach (var bike in bikeManager.GetBikes())
        {
            bike.SetCenterText("");
            bike.StartMoving();
            bike.SetTrailEnabled(true);
        }
    }

    private void OnBikeDeath(BikeController bike)
    {
        livingBikes.Remove(bike);
        if (livingBikes.Count <= 1)
        {
            EndRound();
        }
    }

    private void EndRound()
    {
        if (livingBikes.Count > 0)
        {
            var winner = livingBikes.First();
            winner.PauseBikeAtEndOfRound();
            winner.SetPoints(winner.Points + 1);
            winner.SetCenterText("WIN");

            if (winner.Points >= pointsToWin)
            {
                allBikes.ForEach(b => b.SetCenterText("DEFEAT"));
                winner.SetCenterText("VICTORY");
                updater.DelayAction(5f, EndGame);
            }
            else
            {
                winner.SetCenterText("WIN");
                updater.DelayAction(3f, StartRound);
            }
        } 
        else
        {
            updater.DelayAction(3f, StartRound);
        }
    }

    private void EndGame()
    {
        bikeManager.GetBikes().ForEach(b => b.Reset());
        bikeManager.RemoveAllBikes();
        endGameCallback.Invoke();
    }

    
}
