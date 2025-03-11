using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    [Header("巡逻范围")]
    public float patrolRadius;
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
    }

    protected override void Move()
    {
        //去掉基类中的移动方法
    }

    public override bool FoundPlayer()
    {
        //Debug.Log("find");
        var obj=Physics2D.OverlapCircle(transform.position, checkDistance, AttackLayer);
        if (!obj) return false;
        attacker = obj.transform;
        return true;

    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }

    public override Vector3 GetNewPoint()
    {
        var targetX=Random.Range(-patrolRadius, patrolRadius);
        var targetY=Random.Range(-patrolRadius, patrolRadius);
        
        return spawnPoint+ new Vector3(targetX,targetY);
    }
}
