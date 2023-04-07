using System;
using System.Collections.Generic;
using System.Linq;

public class ServerMatchController
{
    private const int pointsToWin = 2;

    private readonly Game game;
    private readonly List<BikeController> allBikes;
    private readonly Action endGameCallback;
    private readonly ServerBikeManager bikeManager;

    private int countdown_seconds = 0;
    private List<BikeController> livingBikes = new();

    public ServerMatchController(Game game, Server server, Action endGameCallback)
    {
        this.game = game;
        this.endGameCallback = endGameCallback;

        //bikeManager = new BikeManagerHost(resourcer, updater, false);
        //allBikes = bikeManager.CreateBikesForPlayers(players);

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
        //bikeManager.GetBikes().ForEach(b => b.Reset());
        //bikeManager.GetBikes().ForEach(b => b.SetCenterText(countdown_seconds.ToString()));
        game.Mono.DelayAction(1f, CountDown);
    }

    private void CountDown()
    {
        countdown_seconds -= 1;
        //bikeManager.GetBikes().ForEach(b => b.SetCenterText(countdown_seconds.ToString()));

        if (countdown_seconds > 0)
        {
            game.Mono.DelayAction(1f, CountDown);
        } 
        else
        {
            FinishCountdown();
        }
    }

    private void FinishCountdown()
    {
        /*
        foreach (var bike in bikeManager.GetBikes())
        {
            bike.SetCenterText("");
            bike.StartMoving();
            bike.SetTrailEnabled(true);
        }
        */
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
                game.Mono.DelayAction(5f, EndGame);
            }
            else
            {
                winner.SetCenterText("WIN");
                game.Mono.DelayAction(3f, StartRound);
            }
        } 
        else
        {
            game.Mono.DelayAction(3f, StartRound);
        }
    }

    private void EndGame()
    {
        //bikeManager.GetBikes().ForEach(b => b.Reset());
        //bikeManager.RemoveAllBikes();
        endGameCallback.Invoke();
    }

    
}
