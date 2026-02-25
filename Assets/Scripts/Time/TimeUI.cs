using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeUI : MonoBehaviour
{
     [SerializeField] private TextMeshProUGUI timeText;

    private void OnEnable()
    {
        GameEvents.OnTimeChanged += UpdateUI;
    }

    private void OnDisable()
    {
        GameEvents.OnTimeChanged -= UpdateUI;
    }

    private void UpdateUI(TimePeriod period, int day, DayOfWeek dayOfWeek)
    {
        // DAY 1 - Monday (Morning)
        timeText.text = "DAY " + day + " - " + dayOfWeek + " (" + period + ")";
    }
}
