using System.Collections;
using UnityEngine;
using TMPro;

public class TimeSlowAbility : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float slowTimeScale = 0.2f;
    [SerializeField] float duration = 5f;
    [SerializeField] float cooldown = 15f;

    [Header("UI")]
    [SerializeField] TMP_Text abilityText;

    private bool isActive = false;
    private bool onCooldown = false;
    private float durationTimer;
    private float cooldownTimer;

    // Store player's original speeds
    MovementStateManager movement;

    void Start()
    {
        movement = GetComponent<MovementStateManager>();
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isActive && !onCooldown)
            ActivateAbility();

        if (isActive)
        {
            durationTimer -= Time.unscaledDeltaTime; // unscaled so player timer works while time is slow
            if (durationTimer <= 0f)
                DeactivateAbility();
        }

        if (onCooldown)
        {
            cooldownTimer -= Time.unscaledDeltaTime;
            if (cooldownTimer <= 0f)
                onCooldown = false;
        }

        UpdateUI();
    }

    void ActivateAbility()
    {
        isActive = true;
        durationTimer = duration;
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Make player movement unaffected by using unscaled speeds
        movement.walkSpeed *= (1f / slowTimeScale);
        movement.walkBackSpeed *= (1f / slowTimeScale);
        movement.runSpeed *= (1f / slowTimeScale);
        movement.runBackSpeed *= (1f / slowTimeScale);
        movement.crouchSpeed *= (1f / slowTimeScale);
        movement.crouchBackSpeed *= (1f / slowTimeScale);
    }

    void DeactivateAbility()
    {
        isActive = false;
        onCooldown = true;
        cooldownTimer = cooldown;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Restore player speeds
        movement.walkSpeed *= slowTimeScale;
        movement.walkBackSpeed *= slowTimeScale;
        movement.runSpeed *= slowTimeScale;
        movement.runBackSpeed *= slowTimeScale;
        movement.crouchSpeed *= slowTimeScale;
        movement.crouchBackSpeed *= slowTimeScale;
    }

    void UpdateUI()
    {
        if (abilityText == null) return;

        if (isActive)
            abilityText.text = "SLOW: " + durationTimer.ToString("F1") + "s";
        else if (onCooldown)
            abilityText.text = "Cooldown: " + cooldownTimer.ToString("F1") + "s";
        else
            abilityText.text = "[F] Time Slow";
    }
}