using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameEvents
{
    // System 1: Time
    public static Action<TimePeriod, int, DayOfWeek> OnTimeChanged;
    public static Action OnDayPassed; 

    // System 2/4: Inventory and Crafting 
    public static Action OnInventoryUpdated;

    // System 3: Combat
    public static Action<float, float> OnPlayerHealthChanged; // Current, Max
    public static Action<float, float> OnPlayerAPChanged;     // Current, Max
    public static Action OnScreenShake;


    public static void TriggerTimeChanged(TimePeriod period, int day, DayOfWeek dayofWeek)
    {
        OnTimeChanged?.Invoke(period, day, dayofWeek);
    }

    public static void TriggerInventoryUpdated()
    {
        OnInventoryUpdated?.Invoke();
    }

    public static void TriggerPlayerStatusChanged(float hp, float maxHp, float ap, float maxAp)
    {
        OnPlayerHealthChanged?.Invoke(hp, maxHp);
        OnPlayerAPChanged?.Invoke(ap, maxAp);
    }
}









