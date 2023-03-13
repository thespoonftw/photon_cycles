using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiveGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;

    private List<Player> allPlayers = new();
    private List<Player> livingPlayers = new();
    private BikeManager bikeManager;

    private int countdown_seconds = 0;
        

    public void Init(BikeManager bikeManager, List<Player> players)
    {
        this.bikeManager = bikeManager;
        allPlayers = players;

        bikeManager.CreateBikesForPlayers(players);

        foreach (var p in players)
        {
            p.Bike.OnDeath += () => OnPlayerDeath(p);
        }

        StartRound();
    }

    private void StartRound()
    {
        countdown_seconds = 3;
        livingPlayers = allPlayers.ToList();
        allPlayers.ForEach(p => p.Bike.SetVisible(true));
        allPlayers.ForEach(p => p.Bike.ReturnToSpawn());
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
        livingPlayers.ForEach(p => p.Bike.SetMoving(true));
    }

    private void OnPlayerDeath(Player player)
    {
        Debug.Log("A player died");
        livingPlayers.Remove(player);
        if (livingPlayers.Count <= 1)
        {
            EndRound();
        }
    }

    private void EndRound()
    {
        livingPlayers.ForEach(p => p.Bike.SetMoving(false));
        StartCoroutine(EndRoundAsync());
    }

    private IEnumerator EndRoundAsync()
    {
        yield return new WaitForSeconds(3f);
        StartRound();
    } 

    
}
