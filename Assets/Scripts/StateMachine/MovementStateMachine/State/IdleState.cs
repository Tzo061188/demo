using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class IdleState : PlayerMovementState
{
    private GameTimer timer;
    public IdleState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //idle 状态 AnimatorID
        inputMultiple = 0;
        animator.SetFloat(AnimatorID.MovementID,inputMultiple);
        //idle 速度 speed
        playerMovementStateMachine.player.speed = playerMovementStateMachine.playerSO.movementData.idleData.playerSpeed;

        
        RoleInputSystem.Instance.roleInput.Role.Move.started += ToWalkState;

    }
    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        base.Exit();
        RoleInputSystem.Instance.roleInput.Role.Move.started -= ToWalkState;
        
    }

    private void ToWalkState(CallbackContext context){

        playerMovementStateMachine.ChangedState(playerMovementStateMachine.walkState);

        
    }
}
