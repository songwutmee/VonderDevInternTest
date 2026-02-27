using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
    private SpriteRenderer skyRenderer;

    public Color morningColor = new Color(1f, 0.7f, 0.5f);
    public Color afternoonColor = new Color(0.5f, 0.8f, 1f);
    public Color eveningColor = new Color(0.1f, 0.1f, 0.3f);

    private void Awake()
    {
        skyRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        GameEvents.OnTimeChanged += UpdateSkyColor;
    }

    private void OnDisable()
    {
        GameEvents.OnTimeChanged -= UpdateSkyColor;
    }

    private void UpdateSkyColor(TimePeriod period, int day, DayOfWeek dayOfWeek)
    {
        // Switch based on state
        if (period == TimePeriod.Morning) skyRenderer.color = morningColor;
        else if (period == TimePeriod.Afternoon) skyRenderer.color = afternoonColor;
        else if (period == TimePeriod.Evening) skyRenderer.color = eveningColor;
    }
}