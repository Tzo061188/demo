using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNullMovementState : PlayerMovementState
{
    public PlayerNullMovementState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerMovementStateMachine.player.speed = 3f;
       
        
    }

    public override void Update()
    {
        //攻击时不旋转
        if(!animator.CheckAnimation_TagIs(0,"Attack"))
            base.Update();
    }

    public override void AnimationExitEvent()
    {
       CheckState();
    }

    private void CheckState()
    {
        if(animator.CheckAnimation_TagIs(0,"Attack")||animator.CheckAnimation_TagIs(0,"Skill"))
           return;
        if(RoleInputSystem.Instance.Move != Vector2.zero) {
            playerMovementStateMachine.ChangedState(playerMovementStateMachine.runState);
        }
        else{
            playerMovementStateMachine.ChangedState(playerMovementStateMachine.idleState);
        }
    }
}
