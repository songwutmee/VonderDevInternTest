using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameEvents
{
    // System 1: Time
    public static Action<TimePeriod, int, DayOfWeek> OnTimeChanged;
    
    // System 2: Inventory
    public static Action OnInventoryUpdated;
    
    // System 3: Combat (Planned)
    public static Action OnPlayerAttack; 

    public static void TriggerTimeChanged(TimePeriod period, int day, DayOfWeek dayofWeek)
    {
        OnTimeChanged?.Invoke(period, day, dayofWeek);
    }

    public static void TriggerInventoryUpdated()
    {
        OnInventoryUpdated?.Invoke();
    }
}