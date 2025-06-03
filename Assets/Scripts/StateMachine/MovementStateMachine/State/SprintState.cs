using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class SprintState : PlayerMovementState
{
    public SprintState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        playerAttributeData.isCanSprint = false;

        TimerManager.Instance.GetOneTimer(0.4f,CancelSprintColdTime);
    }

    public void CancelSprintColdTime(){
        playerAttributeData.isCanSprint = true;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void AnimationExitEvent()
    {
        base.AnimationExitEvent();
        
        if(RoleInputSystem.Instance.Move != Vector2.zero){
            playerMovementStateMachine.ChangedState(playerMovementStateMachine.runState);
            return;
        }
        playerMovementStateMachine.ChangedState(playerMovementStateMachine.idleState);
    }
}
