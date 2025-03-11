using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Snail : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new SnailPatrolState();
        skillState = new SnailSkillState();
    }

    protected override void Move()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PreMove")&&!animator.GetCurrentAnimatorStateInfo(0).IsName("Recover"))
            rb.velocity=new Vector2(currentSpeed*faceDir.x*Time.deltaTime,rb.velocity.y);
    }
}
