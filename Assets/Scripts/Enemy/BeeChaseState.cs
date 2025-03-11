using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState
{
    private Attack attack;
    //追击坐标和方向
    private Vector3 target;
    private Vector3 moveDir;
    private bool isAttack;
    private float attackRateCounter;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        attack = currentEnemy.transform.GetChild(1).GetComponent<Attack>();
        //Debug.Log("attack range:"+attack.attackRange);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.animator.SetBool("chase",true);
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            //Debug.Log("change patrol");
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.5f, 0);
        //Debug.Log("target:"+target+" layer:"+currentEnemy.attacker.name);
        //判断攻击距离
        if (Mathf.Abs(target.x - currentEnemy.transform.position.x) <= attack.attackRange &&
            Mathf.Abs(target.y - currentEnemy.transform.position.y) <= attack.attackRange)
        {
            //Debug.Log("attack");
            //攻击
            isAttack = true;
            if(!currentEnemy.isHurt)
                currentEnemy.rb.velocity = Vector3.zero;
            //计时器
            attackRateCounter -= Time.deltaTime;
            if (attackRateCounter <= 0)
            {
                attackRateCounter =attack.attackRate;
                currentEnemy.animator.SetTrigger("attack");
            }
        }
        else
        {
            //超出攻击范围
            isAttack = false;
            
        }
        //确定移动方向
        moveDir=(target-currentEnemy.transform.position).normalized;
        if (moveDir.x > 0)
        {
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (moveDir.x < 0)
        {
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {
        //蜜蜂移动逻辑
        //Debug.Log(currentEnemy.rb.velocity);
        if (!currentEnemy.isHurt && !currentEnemy.isDead&&!isAttack)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed*Time.deltaTime;
        }
        
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("chase",false);
    }
}
