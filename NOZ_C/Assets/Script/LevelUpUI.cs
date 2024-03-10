using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public PlayerStats playerStats;

    private HealthBar healthBar;
    private StaminaBar staminaBar;

    public Button levelUpButton;

    [Header("Player Level")]
    public int currentPlayerLevel;
    public int projectedPlayerLevel;
    public Text currentPlayerLevelText;
    public Text projectedPlayerLevelText;
    
    [Header("Souls")]
    public Text currentSoulsText;
    public Text soulsRequiredToLevelUpText;
    private int soulsRequiredToLevelUp;
    private int baseLevelUpCost = 50;

    [Header("Health")]
    public Text healthText;

    [Header("Stamina")]
    public Text staminaText;

    [Header("Attack Damage")]
    public Text attackDamageText;

    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        currentPlayerLevel = playerStats.playerLevel;
        currentPlayerLevelText.text = currentPlayerLevel.ToString();

        projectedPlayerLevel = playerStats.playerLevel + 1;
        projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

        // Update health and stamina text based on current player level
        healthText.text = playerStats.SetMaxHealthFromHealthLevel().ToString();
        staminaText.text = playerStats.SetMaxStaminaFromStaminaLevel().ToString();
        attackDamageText.text = playerStats.attackDamage.ToString();

        soulsRequiredToLevelUp = 0;
        soulsRequiredToLevelUp += Mathf.RoundToInt(projectedPlayerLevel * baseLevelUpCost);

        currentSoulsText.text = playerStats.currentSouls.ToString();
        soulsRequiredToLevelUpText.text = soulsRequiredToLevelUp.ToString();

        if (playerStats.currentSouls < soulsRequiredToLevelUp)
        {
            levelUpButton.interactable = false;
        }
        else
        {
            levelUpButton.interactable = true;
        }
    }

    public void ConfirmLevelUp()
    {
        playerStats.playerLevel = projectedPlayerLevel;

        playerStats.currentSouls -= soulsRequiredToLevelUp;

        playerStats.healthLevel++;
        playerStats.staminaLevel++;

        // Update max health and max stamina based on the incremented levels
        playerStats.maxHealth = playerStats.SetMaxHealthFromHealthLevel();
        playerStats.currentHealth = playerStats.maxHealth;
        playerStats.maxStamina = playerStats.SetMaxStaminaFromStaminaLevel();
        playerStats.attackDamage += 5;

        //Cập nhật slider.value
        healthBar.SetMaxHealth(playerStats.maxHealth);
        healthBar.SetCurrentHealth(playerStats.maxHealth);

        staminaBar.SetMaxStamina(playerStats.maxStamina);
        staminaBar.SetCurrentStamina(playerStats.maxStamina);

        UpdateUI();
    }
}
