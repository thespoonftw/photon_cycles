using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LiveGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;

    private List<Player> allPlayers = new();
    private List<Player> livingPlayers = new();

    private const int pointsToWin = 5;

    private int countdown_seconds = 0;

    private Action endGameCallback;
        

    public void Init(BikeManager bikeManager, List<Player> players, Action endGameCallback)
    {
        this.endGameCallback = endGameCallback;
        allPlayers = players;

        bikeManager.CreateBikesForPlayers(players);

        foreach (var p in players)
        {
            p.Bike.OnDeath += () => OnPlayerDeath(p);
            p.Canvas.SetPoints("0");
        }

        StartRound();
    }

    private void StartRound()
    {
        countdown_seconds = 3;
        livingPlayers = allPlayers.ToList();
        allPlayers.ForEach(p => p.Bike.ResetBikeStartOfRound());
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        countdownText.text = countdown_seconds.ToString();
        countdown_seconds -= 1;
        yield return new WaitForSeconds(1f);
        
        if (countdown_seconds > 0)
        {
            StartCoroutine(CountDown());
        } 
        else
        {
            FinishCountdown();
        }
    }

    private void FinishCountdown()
    {
        countdownText.text = "";
        livingPlayers.ForEach(p => p.Bike.StartMovingBikeStartOfRound());
    }

    private void OnPlayerDeath(Player player)
    {
        livingPlayers.Remove(player);
        if (livingPlayers.Count <= 1)
        {
            EndRound();
        }
    }

    private void EndRound()
    {
        if (livingPlayers.Count > 0)
        {
            var winner = livingPlayers.First();
            winner.Bike.PauseBikeAtEndOfRound();
            winner.AddPoint();
        }

        StartCoroutine(EndRoundAsync());
    }

    private IEnumerator EndRoundAsync()
    {
        yield return new WaitForSeconds(3f);

        if (allPlayers.Select(p => p.Points).Any(p => p >= pointsToWin))
        {
            EndGame();
        }
        else
        {
            StartRound();
        }        
    }

    private void EndGame()
    {
        allPlayers.ForEach(p => p.Bike.ResetBikeStartOfRound());
        endGameCallback.Invoke();
    }

    
}
