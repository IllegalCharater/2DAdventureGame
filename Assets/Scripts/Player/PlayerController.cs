using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation; 
    private Character character;
    
    public Vector2 inputDirection;
    private int faceDir;
    
    private Vector2 originalOffset;
    private Vector2 originalSize;
    private float walkSpeed;
    private float runSpeed;
    private float crouchSpeed;
    private Coroutine slideCoroutine;//滑铲行为
    [Header("基本参数")]
    public float speed;
    public float jumpForce;
    public float wallJumpForce;
    public float hurtForce;
    public float slideSpeed;
    public float slideDistance;
    public float slideDisCounter;
    public float slidePowerCost;
    [Header("状态")] 
    public bool isSpeedup;
    public bool isCrouch;//下蹲
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool wallJump;
    public bool isSlide;
    [Header("物理材质")]
    public PhysicsMaterial2D normalMaterial;
    public PhysicsMaterial2D wallMaterial;
    [Header("监听事件")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterLoadSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO afterLoadDataEvent;
    public VoidEventSO backToMenuEvent;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        character = GetComponent<Character>();
        
        walkSpeed = speed;
        crouchSpeed = speed / 2;
        runSpeed = speed*2;
        originalOffset = capsuleCollider.offset;
        originalSize = capsuleCollider.size;
        
        inputControl = new PlayerInputControl();
        //跳跃
        inputControl.Gameplay.Jump.started += Jump; //started指按键被按下的一刻，+=为注册函数

        #region 加速
        inputControl.Gameplay.Speedup.performed += ctx =>
        {
            if (physicsCheck.isGround&&!isCrouch)
            {
                speed = runSpeed;
                isSpeedup = true;
            }
        };
        inputControl.Gameplay.Speedup.canceled += ctx =>
        {
            if (physicsCheck.isGround&&!isCrouch)
            {
                speed = walkSpeed;
                isSpeedup = false;
            }
        };
        #endregion
        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;
        //滑铲
        inputControl.Gameplay.Slide.started += Slide;
        
        inputControl.Enable();
    }
    
    private void OnEnable()
    {
        
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterLoadSceneEvent.OnEventRaised += OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        afterLoadDataEvent.OnEventRaised += OnAfterLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
    }

    #region 事件注册

    //重置死亡状态
    private void Restart()
    {
        isDead = false;
    }
    //加载数据时
    private void OnLoadDataEvent()
    {
        //Debug.Log("load");
        
    }
    //加载数据完成时
    private void OnAfterLoadDataEvent()
    {
        //Debug.Log("load finish");
        Restart();
    }
    //加载完成后关闭控制器
    private void OnAfterSceneLoadedEvent()
    {
        inputControl.Gameplay.Enable();
    }
    //加载场景时关闭控制器
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
        
    }
    //加载到主菜单
    private void OnBackToMenuEvent()
    {
        Restart();
    }
    
    #endregion
    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterLoadSceneEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        afterLoadDataEvent.OnEventRaised -= OnAfterLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        Flip();
        if(!isHurt&&!isAttack)
            Move();
    }

    public void Move()
    {
        //水平方向移动
        //if(!isCrouch)
        if(!wallJump)
            rb.velocity = new Vector2(inputDirection.x*speed*Time.deltaTime,rb.velocity.y);
        if(!isSpeedup)
            Crouch();
        
    }
    private void Crouch()
    {
        //下蹲
        isCrouch = inputDirection.y < -0.05f && physicsCheck.isGround;
        if (isCrouch)
        {
            //速度减少
            speed=crouchSpeed;
            //下蹲缩小碰撞体大小
            capsuleCollider.offset = new Vector2(-0.05f, 0.85f);
            capsuleCollider.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            //速度还原
            speed = walkSpeed;
            //还原下蹲前的碰撞体
            capsuleCollider.size = originalSize;
            capsuleCollider.offset = originalOffset;
        }
        
    }
    private void Flip()
    {
        //人物朝向
        faceDir=(int)transform.localScale.x;
        if (inputDirection.x > 0) faceDir = 1;
        else if (inputDirection.x < 0) faceDir = -1;
        //人物翻转
        transform.localScale = new Vector3(faceDir, 1, 1);
    }
    private void Jump(InputAction.CallbackContext obj)
    {
        //Debug.Log("Jump");
        if(physicsCheck.isGround)
        {
            //Debug.Log("normal jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isSlide = false;
            isSpeedup = false;
            //Debug.Log("break slide");
            if (slideCoroutine != null)
            {
                StopCoroutine(slideCoroutine);
                gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }
        else if(physicsCheck.onWall)
        {
            //Debug.Log("wall jump");
            rb.AddForce(new Vector2(-inputDirection.x, 4f) * wallJumpForce, ForceMode2D.Impulse);
            wallJump = true;
        }
        
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (!physicsCheck.onWall)
        {
            playerAnimation.PlayAttackAnimation();
            isAttack = true;
        }
        
    }


    private void Slide(InputAction.CallbackContext obj)
    {
        if (!isSlide&&physicsCheck.isGround&&!wallJump&&character.currentPower>=slidePowerCost)
        {
            isSlide=true;
            //var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x,transform.position.y);
            character.OnSlide(slidePowerCost);
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            slideDisCounter = slideDistance;
            slideCoroutine=StartCoroutine(TriggerSlide());
        }
        
    }

    IEnumerator TriggerSlide()
    {
        do
        {
            yield return null;
            if(!physicsCheck.isGround||physicsCheck.touchLeftWall&&faceDir<0 || physicsCheck.touchRightWall&&faceDir>0)
            {
                break;
            }
            rb.MovePosition(new Vector2(transform.position.x+transform.localScale.x*slideSpeed,transform.position.y));
            slideDisCounter -= slideSpeed;
        }while(slideDisCounter > 0.1f);
        
        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    
    // IEnumerator TriggerSlide(Vector3 targetPos)
    // {
    //     do
    //     {
    //         yield return null;
    //         if(!physicsCheck.isGround||physicsCheck.touchLeftWall&&faceDir<0 || physicsCheck.touchRightWall&&faceDir>0)
    //         {
    //             break;
    //         }
    //         rb.MovePosition(new Vector2(transform.position.x+transform.localScale.x*slideSpeed,transform.position.y));
    //         
    //     }while(Mathf.Abs(targetPos.x - transform.position.x) > 0.1f);
    //     
    //     isSlide = false;
    //     gameObject.layer = LayerMask.NameToLayer("Player");
    // }
    
    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir =  new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.AddForce(dir*hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        //Debug.Log("die!");
        isDead = true;
        inputControl.Gameplay.Disable();
        
    }
    
    #endregion

    private void CheckState()
    {
        //空中时切换光滑物理材质
        capsuleCollider.sharedMaterial = physicsCheck.isGround? normalMaterial: wallMaterial;
        //空中重置加速状态
        if(!physicsCheck.isGround)
            isSpeedup = false;
        //爬墙时缓慢下降
        if(physicsCheck.onWall&&!wallJump)
            rb.velocity=new Vector2(rb.velocity.x,rb.velocity.y/2f);
        else
        {
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y);
        }
        //蹬墙跳下落时重置状态
        if(wallJump&&rb.velocity.y<0f)
            wallJump = false;
    }
}
