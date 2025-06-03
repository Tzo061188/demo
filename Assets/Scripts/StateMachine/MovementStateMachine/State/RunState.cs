using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RunState : PlayerMovementState
{
    public RunState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        inputMultiple = playerMovementStateMachine.playerSO.movementData.runData.movementValue;

        playerMovementStateMachine.player.speed = playerMovementStateMachine.playerSO.movementData.runData.playerSpeed;

        animator.SetFloat(AnimatorID.Run,1);      

        RoleInputSystem.Instance.roleInput.Role.Move.canceled += ToIdleState;
        
        RoleInputSystem.Instance.roleInput.Role.Run.canceled += ToWalkState;
    }


    public override void Exit()
    {
        base.Exit();

        animator.SetFloat(AnimatorID.Run,0);   

        RoleInputSystem.Instance.roleInput.Role.Move.canceled -=ToIdleState;
        
        RoleInputSystem.Instance.roleInput.Role.Run.canceled -= ToWalkState;
    }
    public override void Update()
    {
        base.Update();
    }


    private void ToWalkState(InputAction.CallbackContext context)
    {
        playerMovementStateMachine.ChangedState(playerMovementStateMachine.walkState);
    }

    
    private void ToIdleState(InputAction.CallbackContext context)
    {
        playerMovementStateMachine.ChangedState(playerMovementStateMachine.idleState);
    }
}
