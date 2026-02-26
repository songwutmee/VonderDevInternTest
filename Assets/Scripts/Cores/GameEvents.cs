using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class GameEvents
{
    // System 1: Time Events
    public static Action<TimePeriod, int, DayOfWeek> OnTimeChanged;
    
    // System 2: Inventory Events
    public static Action OnInventoryUpdated;
    
    // System 3: Combat Events
    public static Action OnPlayerAttack; 
    public static Action<float> OnHealthChanged;

    public static void TriggerTimeChanged(TimePeriod period, int day, DayOfWeek dayofWeek)
    {
        if (OnTimeChanged != null) OnTimeChanged.Invoke(period, day, dayofWeek);
    }

    // Triggered whenever items are added, removed, or moved
    public static void TriggerInventoryUpdated()
    {
        if (OnInventoryUpdated != null) OnInventoryUpdated.Invoke();
    }
}