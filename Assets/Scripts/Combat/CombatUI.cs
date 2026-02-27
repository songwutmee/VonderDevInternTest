using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatUI : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI apText;

    private void OnEnable()
    {
        GameEvents.OnPlayerHealthChanged += UpdateHP;
        GameEvents.OnPlayerAPChanged += UpdateAP;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerHealthChanged -= UpdateHP;
        GameEvents.OnPlayerAPChanged -= UpdateAP;
    }

    private void Start()
    {
        // Initial update to clear "New Text" placeholders
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.UpdateUI();
        }
    }

    private void UpdateHP(float current, float max)
    {
        hpText.text = $"HP: {Mathf.CeilToInt(current)} / {max}";
    }

    private void UpdateAP(float current, float max)
    {
        apText.text = $"AP: {Mathf.CeilToInt(current)} / {max}";
    }
}