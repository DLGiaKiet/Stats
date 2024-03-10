using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region AttackVariables
    public List<AttackSO> combo;
    private float lastpress = 0; // Cooldown của mỗi Attack --> Không spam quá nhanh
    private float lastcomboEnd; // đảm bảo rằng combo kết thúc trong 1 khoảng time
    private int ComboCounter; // thực hiện được bao nhiêu đòn.
    private Animator anim;
    private Animator anime;
    public static bool isAttacking = false;
    private PlayerStats playerStats;
    private int attackStaminaCost = 20;
    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject tempchar = GameObject.FindGameObjectWithTag("Player");
        anime = tempchar.GetComponentInChildren<Animator>();
        playerStats = tempchar.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !anime.GetCurrentAnimatorStateInfo(0).IsName("Dodge") && playerStats.currentStamina > attackStaminaCost)
        {
            Attack();
            
        }
        ExitAttack();
    }

    private void Attack()
    {
        if(Time.time - lastcomboEnd >= 0.5f && ComboCounter < combo.Count) // between combo
        {
            CancelInvoke("EndCombo"); // Dừng combo trước để bắt đầu combo mới
            if(Time.time - lastpress >= 0.6f) // between attack
            {
                anim.runtimeAnimatorController = combo[ComboCounter].aniOV;
                anim.Play("Attack", 0, 0.15f);
                isAttacking = true;
                ComboCounter++;
                lastpress = Time.time;
                playerStats.TakeStaminaDamage(attackStaminaCost);
                // Kiểm tra cho chắc liệu có past qua giới hạn của combo không.
                if (ComboCounter >= combo.Count)
                {
                    ComboCounter = 0;
                }
            }
        }
    }

    private void ExitAttack()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1); // after 1 sec
        }    
    }   
    
    private void EndCombo()
    {
        ComboCounter = 0;
        isAttacking = false;
        lastcomboEnd = Time.time;
    }   
}
