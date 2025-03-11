using UnityEngine;

public class SnailPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.animator.SetBool("walk",true);
        
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Skill);
        }
        //撞墙后返回
        if (!currentEnemy.check.isGround||(currentEnemy.check.touchLeftWall && currentEnemy.faceDir.x < 0 ||
                                           currentEnemy.check.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.isWait = true;//撞墙之后开始等待
            currentEnemy.animator.SetBool("walk",false);
            currentEnemy.rb.velocity=new Vector2(0,currentEnemy.rb.velocity.y);
        }
        else
        {
            currentEnemy.animator.SetBool("walk",true);
            
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("walk",false);
    }
}
