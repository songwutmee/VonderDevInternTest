using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamageable
{
    public static PlayerStatus Instance { get; private set; }

    [Header("Status Values")]
    public float maxHP = 100f;
    public float currentHP = 100f;
    public float maxAP = 100f;
    public float currentAP = 100f;

    private bool isDead = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        // Reset health on a new day
        GameEvents.OnDayPassed += ResetStatus;
    }

    private void OnDisable()
    {
        GameEvents.OnDayPassed -= ResetStatus;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHP = Mathf.Max(0, currentHP - amount);
        GetComponent<SpriteFlash>()?.Flash();
        PlayerController.Instance.PlayHurtAnimation();
        
        if (currentHP <= 0) Die();
        
        UpdateUI();
    }

    public bool UseAP(float amount)
    {
        if (currentAP >= amount)
        {
            currentAP -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    private void Die()
    {
        isDead = true;
        PlayerController.Instance.PlayDeathAnimation();
        StartCoroutine(DeathResetRoutine());
    }

    private IEnumerator DeathResetRoutine()
    {
        yield return new WaitForSeconds(2f);
        ResetStatus();
        isDead = false;
    }

    public void ResetStatus()
    {
        currentHP = maxHP;
        currentAP = maxAP;
        UpdateUI();
        PlayerController.Instance.Revive();
    }

    public void UpdateUI()
    {
        GameEvents.TriggerPlayerStatusChanged(currentHP, maxHP, currentAP, maxAP);
    }
}