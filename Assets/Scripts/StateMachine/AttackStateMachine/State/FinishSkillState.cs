using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSkillState : PlayerAttackState
{
    private int index = 0;
    private AttackContentData comboData;
    public FinishSkillState(PlayerAttackStateMachine playerAttackStateMachine) : base(playerAttackStateMachine)
    {
    }

    public override void Enter(){
        

        if (player.currentAttackEnemy != null)
        {  
            
            player.transform.forward = player.currentAttackEnemy.transform.position - player.transform.position;
        }
        comboData =  player.playerSO.attackModuleData.finalSkillAttack;
        animator.CrossFadeInFixedTime(comboData.attackContents[index].attackName,0.1f);
    }
    public override void Update()
    {
        base.Update();
        if(animator.CheckAnimation_TagIs(0,"Skill") == false){
            playerAttackStateMachine.ChangedState(playerAttackStateMachine.nullState);
        }
    }

    public override void Exit()
    {

        index = 0;
        comboData = null;
    }
    public override void AnimationExitEvent()
    {
        playerAttackStateMachine.ChangedState(playerAttackStateMachine.nullState);
        Debug.Log("退出技能");
    }

}
