using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D),typeof(Animator),typeof(PhysicsCheck))]
public class Enemy : MonoBehaviour
{
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public Animator animator;
    [HideInInspector]public PhysicsCheck check;
    [Header("基本参数")]
    public Vector3 spawnPoint;//出生点
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public float hurtForce;
    public Vector3 faceDir;
    public Transform attacker;
    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask AttackLayer;
    [Header("计时器")]
    //撞墙等待时间
    public float waitTime;
    public float waitTimeCounter;
    public bool isWait;
    //丢失跟踪后状态持续时间
    public float lostTime;
    public float lostTimeCounter;
    [Header("状态")] 
    public bool isHurt;
    public bool isDead;
    //状态机
    protected BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;
    protected BaseState skillState;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        check = GetComponent<PhysicsCheck>();
        
        spawnPoint = transform.position;
        currentSpeed = normalSpeed;
        //waitTimeCounter = waitTime;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        //Debug.Log("update begin");
        currentState.LogicUpdate();
        TimeCounter();
    }

    private void FixedUpdate()
    {
        if(!isHurt&&!isDead&&!isWait)
            Move();
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }

    protected virtual void Move()
    {
        rb.velocity=new Vector2(currentSpeed*faceDir.x*Time.deltaTime,rb.velocity.y);
    }

    /// <summary>
    /// 计时器
    /// </summary>
    private void TimeCounter()
    {
        //撞墙等待时间计数器
        if (isWait)
        {
            waitTimeCounter-= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                isWait = false;//等待结束，开始巡逻
                waitTimeCounter = waitTime;//重置
                transform.localScale=new Vector3(faceDir.x, 1, 1);
            }
        }
        //丢失攻击目标计数器
        if (!FoundPlayer()&&lostTimeCounter>0)
        {
            lostTimeCounter -= Time.deltaTime;
            
        }
        
    }

    
    //寻找攻击目标
    public virtual bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance,
            AttackLayer);
    }

    //切换状态机
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill => skillState,
            _ => null
        };
        currentState.OnExit();
        currentState = newState;
        
        currentState.OnEnter(this);
    }
    
    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }
    #region 事件执行方法
    public void OnTakeDamage(Transform attackerTrans)
    {
        attacker = attackerTrans;
        //转身
        if (attackerTrans.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackerTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        //受击被击退
        isHurt = true;
        animator.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.velocity=new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
    }

    IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce( dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        isHurt = false;
    }

    public void OnDie()
    {
        transform.GetChild(0).gameObject.layer=2;//将攻击层置空
        rb.velocity = Vector2.zero;
        animator.SetBool("dead",true);
        isDead = true;
    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    

    #endregion

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset+new Vector3(checkDistance*-transform.localScale.x,0),0.2f);
    }
}
