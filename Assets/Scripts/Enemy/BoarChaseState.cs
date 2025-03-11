
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //Debug.Log("chase");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.animator.SetBool("run",true);
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        if (!currentEnemy.check.isGround||(currentEnemy.check.touchLeftWall && currentEnemy.faceDir.x < 0 ||
                                           currentEnemy.check.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale=new Vector3(currentEnemy.faceDir.x,1,1);
            
        }
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.animator.SetBool("run",false);
    }
}
    
