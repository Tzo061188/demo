using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class WalkState : PlayerMovementState
{
    GameTimer gameTimer;
    int SprintCount;
    public WalkState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        inputMultiple = playerMovementStateMachine.playerSO.movementData.walkData.movementValue;
        playerMovementStateMachine.player.speed = playerMovementStateMachine.playerSO.movementData.walkData.playerSpeed;
        

        RoleInputSystem.Instance.roleInput.Role.Move.canceled +=ToIdleState;
        RoleInputSystem.Instance.roleInput.Role.Run.performed += ToRunState;

    }
    public override void Exit()
    {
        base.Exit();
        RoleInputSystem.Instance.roleInput.Role.Move.canceled -=ToIdleState;
        RoleInputSystem.Instance.roleInput.Role.Run.performed -= ToRunState;

    }

    public override void Update()
    {
        base.Update();
    }
    private void ToIdleState(CallbackContext context){
        playerMovementStateMachine.ChangedState(playerMovementStateMachine.idleState);
    }
    private void ToRunState(CallbackContext context){
        playerMovementStateMachine.ChangedState(playerMovementStateMachine.runState);
    }


}
