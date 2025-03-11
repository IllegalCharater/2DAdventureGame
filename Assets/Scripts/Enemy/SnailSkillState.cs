using UnityEngine;

public class SnailSkillState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.animator.SetBool("walk",false);
        currentEnemy.animator.SetBool("hide",true);
        currentEnemy.animator.SetTrigger("skill");

        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        
        currentEnemy.GetComponentInChildren<Character>().isInvulnerable = true;
        currentEnemy.GetComponentInChildren<Character>().invulnerableCounter =
            currentEnemy.GetComponentInChildren<Character>().invulnerableDuration;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        currentEnemy.GetComponentInChildren<Character>().invulnerableCounter =
            currentEnemy.GetComponentInChildren<Character>().invulnerableDuration;
    }

    public override void PhysicsUpdate()
    {
        
    }
    

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("hide",false);
        
        currentEnemy.GetComponentInChildren<Character>().isInvulnerable = false;
        currentEnemy.GetComponentInChildren<Character>().invulnerableCounter = 0;
    }
}
