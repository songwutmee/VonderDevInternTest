using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Settings")]
    public float secondsPerPeriod = 60f;
    public bool useRealtimeProgression = true;

    [Header("Status")]
    public int totalDays = 0;
    public TimePeriod currentPeriod = TimePeriod.Morning;

    private float timer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        NotifyTimeChange();
    }

    private void Update()
    {
        if (useRealtimeProgression)
        {
            timer += Time.deltaTime;
            if (timer >= secondsPerPeriod)
            {
                AdvanceTime();
            }
        }
    }

    //  Morning -> Afternoon -> Evening -> Next Day
    public void AdvanceTime()
    {
        timer = 0;
        
        if (currentPeriod == TimePeriod.Morning)
        {
            currentPeriod = TimePeriod.Afternoon;
        }
        else if (currentPeriod == TimePeriod.Afternoon)
        {
            currentPeriod = TimePeriod.Evening;
        }
        else if (currentPeriod == TimePeriod.Evening)
        {
            // Reset to Morning and increase day count
            currentPeriod = TimePeriod.Morning;
            totalDays++;
        }

        NotifyTimeChange();
    }

    private void NotifyTimeChange()
    {
        DayOfWeek dayOfWeek = (DayOfWeek)(totalDays % 7);
        GameEvents.TriggerTimeChanged(currentPeriod, totalDays, dayOfWeek);
    }
}