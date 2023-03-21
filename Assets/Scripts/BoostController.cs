using System;
using System.Collections.Generic;
using UnityEngine;

public class BoostController
{
    private float remainingBoost = 0;
    private float drainRate = 0.5f;
    private float regenRate = 0.1f;
    private Player player;

    public BoostController(Player player)
    {
        this.player = player;
    }

    public void Reset()
    {
        remainingBoost = 0;
        player.Canvas.SetBoost(remainingBoost);
    }

    public bool IsBoosting()
    {
        var amountToRegen = regenRate * Time.deltaTime;
        remainingBoost = Math.Min(1, remainingBoost + amountToRegen);
        player.Canvas.SetBoost(remainingBoost);

        if (!player.Input.IsButton1Held())
            return false;

        var amountToDrain = drainRate * Time.deltaTime;
        
        if (amountToDrain > remainingBoost) 
            return false;

        remainingBoost -= amountToDrain;
        player.Canvas.SetBoost(remainingBoost);
        return true;

    }
}
