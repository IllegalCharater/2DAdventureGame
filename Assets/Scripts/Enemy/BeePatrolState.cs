using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrolState : BaseState
{
    //移动坐标
    private Vector3 target;
    //移动方向
    private Vector3 moveDir;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        target = currentEnemy.GetNewPoint();
        //Debug.Log("enter:"+target);
    }

    public override void LogicUpdate()
    {
        //Debug.Log("update"+target);
        //Debug.Log(currentEnemy.transform.position);
        //发现攻击目标切换状态
        if (currentEnemy.FoundPlayer())
        {
            //Debug.Log("change chase");
            currentEnemy.SwitchState(NPCState.Chase);
        }
        
        //进入指定坐标后开始等待
        if (Mathf.Abs(target.x - currentEnemy.transform.position.x) < 0.1f &&
            Mathf.Abs(target.y - currentEnemy.transform.position.y) < 0.1f)
        {
            currentEnemy.isWait = true;
            target = currentEnemy.GetNewPoint();
            //Debug.Log("waiting");
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
        if (!currentEnemy.isWait && !currentEnemy.isHurt && !currentEnemy.isDead)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed*Time.deltaTime;
        }
        else
        {
            currentEnemy.rb.velocity = Vector3.zero;
        }
    }

    public override void OnExit()
    {
        
    }
}
