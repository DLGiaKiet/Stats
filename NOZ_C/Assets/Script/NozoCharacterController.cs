using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class NozoCharacterController : MonoBehaviour
{
    #region MovementVariable
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float Gravity;
    [SerializeField] private Transform Player_Charac;

    private CharacterController characterController;
    private Vector3 direction;
    private float moveSpeed;
    private Vector3 velocity;
    private float horizontal;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float GroundCheckDistance;
    [SerializeField] LayerMask GroundMask;

    private Animator ani;

    [SerializeField] private AnimationCurve dodgeCurve;

    private bool isDodging = false;
    private float dodgeTimer;

    private PlayerStats playerStats;

    private UIManager uiManager;
    private bool isSelectWindowShow = false;

    private int rollStaminaCost = 40;
    private int runStaminaCost = 5;

    #endregion
    void Start()
    {
        GameObject tempChar = GameObject.FindGameObjectWithTag("Player");
        characterController = tempChar.GetComponent<CharacterController>();
        ani = tempChar.GetComponentInChildren<Animator>();
        Keyframe dodge_lastFrame = dodgeCurve[dodgeCurve.length - 1];
        dodgeTimer = dodge_lastFrame.time;
        playerStats = tempChar.GetComponent<PlayerStats>();
        uiManager = FindObjectOfType<UIManager>();
    }
    void Update()
    {
        Gravity_Control();

        if (!isDodging && !ani.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Player_Rota();
            Dichuyen();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (direction.magnitude != 0 && playerStats.currentStamina > rollStaminaCost) 
            {
                StartCoroutine(Dodge());
                playerStats.TakeStaminaDamage(rollStaminaCost);
            };
            return;
        }

        // Mở cửa sổ lựa chọn khi nhấn phím Escape và cửa sổ chưa hiển thị
        if (Input.GetKeyDown(KeyCode.Escape) && !isSelectWindowShow)
        {
            uiManager.OpenSelectWindow();
            isSelectWindowShow = true; // Đánh dấu rằng cửa sổ đã được mở
        }
        // Đóng cửa sổ lựa chọn khi nhấn phím Escape và cửa sổ đã hiển thị
        else if (Input.GetKeyDown(KeyCode.Escape) && isSelectWindowShow)
        {
            uiManager.CloseSelectWindow();
            isSelectWindowShow = false; // Đánh dấu rằng cửa sổ đã được đóng
        }
    }

    private void Dichuyen()
    {

        horizontal = Input.GetAxisRaw("Horizontal");
        var goc_quay = characterController.transform.eulerAngles.y;
        if(goc_quay == 90f)
        {
            direction = new Vector3(horizontal, 0, 0);
        }    
        else if(goc_quay == 0)
        {
            direction = new Vector3(0, 0, horizontal);
        }

        if (!isGrounded)
        {
            ani.SetBool("IsMoving", true);
            if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                if(playerStats.currentStamina > runStaminaCost)
                {
                    Run();
                    playerStats.TakeStaminaDamage(runStaminaCost);
                }
                else
                {
                    Walk();
                }
            }  
            else if(direction == Vector3.zero)
            {
                ani.SetBool("IsMoving", false);
            }    
            direction *= moveSpeed;
        }
        characterController.Move(direction * Time.deltaTime);
        
    }
    private void Player_Rota()
    {
        Vector3 current_Rota = Player_Charac.transform.localEulerAngles;
        switch (horizontal)
        {
            case 1:
            case 2:
                Mv_forward(current_Rota);
                break;
            case -1:
            case -2:
                Mv_backward(current_Rota);
                break;
        }
    }   
    
    private void Gravity_Control()
    {
        isGrounded = Physics.CheckSphere(transform.position, GroundCheckDistance, GroundMask);
        if (!isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y -= Gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }    
    private void Mv_forward(Vector3 current_Rota)
    {
        if(characterController.transform.position.y != 0)
        {
            current_Rota.y = 0f;
            Player_Charac.transform.localEulerAngles = current_Rota;
        }    
    }   
    
    private void Mv_backward(Vector3 current_Rota)
    {
        if (characterController.transform.position.y != -180)
        {
            current_Rota.y = -180f;
            Player_Charac.transform.localEulerAngles = current_Rota;
        }
    }   
    
    private void Walk()
    {
        moveSpeed = WalkSpeed;
        ani.SetFloat("Ani_Transition", 0, 0.1f, Time.deltaTime);
    }    

    private void Run()
    {
        moveSpeed = RunSpeed;
        ani.SetFloat("Ani_Transition", 1f, 0.1f, Time.deltaTime);
    }   
    IEnumerator Dodge()
    {
        ani.SetTrigger("Dodge");
        ani.SetBool("IsMoving", false);
        isDodging = true;
        float timer = 0;
        while(timer <dodgeTimer)
        {
            float speed = dodgeCurve.Evaluate(timer);
            Vector3 dir = (direction * speed);
            characterController.Move(dir * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        isDodging = false;
    }
}
