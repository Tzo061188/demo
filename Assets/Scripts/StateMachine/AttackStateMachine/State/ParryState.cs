using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryState : PlayerAttackState
{
    public ParryState(PlayerAttackStateMachine playerAttackStateMachine) : base(playerAttackStateMachine)
    {
    }

    public override void Enter(){
        base.Enter();
        
        animator.SetBool(AnimatorID.Parry,true);
    }

    public override void Exit()
    {
        base.Exit();
        animator.SetBool(AnimatorID.Parry,false);
    }

}
