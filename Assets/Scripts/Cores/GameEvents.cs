using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<TimePeriod, int, DayOfWeek> OnTimeChanged;
    
    public static Action OnPlayerAttack; 

    public static void TriggerTimeChanged(TimePeriod period, int day, DayOfWeek dayOfWeek)
    {
        OnTimeChanged?.Invoke(period, day, dayOfWeek);
    }
}