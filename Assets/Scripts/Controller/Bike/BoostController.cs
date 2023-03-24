using System;
using System.Collections.Generic;
using UnityEngine;

public class BoostController
{
    private float remainingBoost = 0;
    private float drainRate = 0.5f;
    private float regenRate = 0.1f;
    private readonly IInputController input;
    private readonly BikeCanvas canvas;

    public BoostController(IInputController input, BikeCanvas canvas)
    {
        this.input = input;
        this.canvas = canvas;
    }

    public void Reset()
    {
        remainingBoost = 0;
        canvas.SetBoost(remainingBoost);
    }

    public bool IsBoosting()
    {
        var amountToRegen = regenRate * Time.deltaTime;
        remainingBoost = Math.Min(1, remainingBoost + amountToRegen);
        canvas.SetBoost(remainingBoost);

        if (!input.GetBoost())
            return false;

        var amountToDrain = drainRate * Time.deltaTime;
        
        if (amountToDrain > remainingBoost) 
            return false;

        remainingBoost -= amountToDrain;
        canvas.SetBoost(remainingBoost);
        return true;

    }
}
