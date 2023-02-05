using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WhipParameters
{
    // Maximum whip's length
    public float MaxLenght;

    // Current whip's length
    public float CurrentLength;

    // Time in seconds it takes to reach its max length
    public float TimeToMax;

    // Time in seconds it takes to retract
    public float TimeToMin;

    // Time it lasts fully extended
    public float TimeAtMax;

    // Time it has been fully extended
    public float TimeActive;

    // Max amount of cooldown
    public float MaxCooldown;

    // Current cooldown
    public float CurrentCooldown;

    // Size of the tip's hit box
    public float HitBoxRadius;
}
