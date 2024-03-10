using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Character Level")]
    public int playerLevel = 1;

    [Header("Stat Levels")]
    public int healthLevel = 10;
    public int staminaLevel = 10;

    [Header("Stats")]
    public int maxHealth;
    public int currentHealth;
    public float maxStamina;
    public float currentStamina;

    
    public int attackDamage = 10;
    public int currentSouls = 1000;

    public float staminaRegenRate = 20f; // Tốc độ hồi phục Stamina mỗi giây
    private float staminaRegenTimer = 2f; // Thời gian delay sau mỗi hành động animation

    private Coroutine regenCoroutine;

    public HealthBar healthBar;
    public StaminaBar staminaBar;
    
    private Animator ani;
    private bool isDodging;
    private bool isRunning;
    private bool isAttacking;

    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetCurrentStamina(currentStamina);

        // Bắt đầu Coroutine để hồi phục Stamina mượt mà
        regenCoroutine = StartCoroutine(RegenerateStamina());

        GameObject tempChar = GameObject.FindGameObjectWithTag("Player");
        ani = tempChar.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        isDodging = ani.GetBool("Dodge");
        isAttacking = ani.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else isRunning = false;
    }

    public int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth == 0)
        {
            currentHealth = 0;
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentStamina -= damage * Time.deltaTime;
        }
        else
        {
            currentStamina = Mathf.Min(currentStamina - damage, maxStamina);
        }
        staminaBar.SetCurrentStamina(currentStamina);
        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }
    }

    IEnumerator RegenerateStamina()
    {
        while (true)
        {
            if(isDodging || isAttacking || isRunning)
            {
                staminaRegenTimer = 0;
            }
            // Hồi phục Stamina mỗi giây
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenTimer > 2f)
                {
                    currentStamina += staminaRegenRate * Time.deltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                    staminaBar.SetCurrentStamina(currentStamina);
                }
            }
            yield return null;
        }
    }

    public void AddSouls(int amount)
    {
        currentSouls += amount;
    }

    public void DeductSouls(int amount)
    {
        currentSouls -= amount;
        if (currentSouls < 0)
        {
            currentSouls = 0;
        }
    }

    public void IncreaseAttackDamage(int amount)
    {
        attackDamage += amount;
    }

    public void DecreaseAttackDamage(int amount)
    {
        attackDamage -= amount;
        if (attackDamage < 0)
        {
            attackDamage = 0;
        }
    }
}