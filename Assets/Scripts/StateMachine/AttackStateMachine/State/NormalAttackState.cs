using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NormalAttackState : PlayerAttackState
{
    public NormalAttackState(PlayerAttackStateMachine playerAttackStateMachine) : base(playerAttackStateMachine)
    {
    }

    public override void Enter()
    {

        base.Enter();
        RoleInputSystem.Instance.roleInput.Role.Attack.started += OnAttackUpdate;
       // RoleInputSystem.Instance.roleInput.Role.HeavyAttack.started += OnHeavyAttackUpdate;
    }

    private void OnHeavyAttackUpdate(InputAction.CallbackContext context)
    {
        playerAttack.OnHeavyAttack();
    }

    private void OnAttackUpdate(InputAction.CallbackContext context)
    {
      
        if(GameManager.Instance.isPlayerCanMove != true)
            return;
        if (player.currentAttackEnemy != null)
        {  
            
            player.transform.forward = player.currentAttackEnemy.transform.position - player.transform.position;
        }
    
        playerAttack.OnLightAttack();
    }

    public override void Exit()
    {
        base.Exit();
        RoleInputSystem.Instance.roleInput.Role.Attack.started -= OnAttackUpdate;
       // RoleInputSystem.Instance.roleInput.Role.HeavyAttack.started += OnHeavyAttackUpdate;
       
    }
    //当攻击动画放完后回到空状态
    public override void AnimationExitEvent()
    {
        TimerManager.Instance.GetOneTimer(0.5f,ToNullState);

    }

    private void ToNullState()
    {
        if(!animator.CheckAnimation_TagIs(0,"Attack")){
            playerAttackStateMachine.ChangedState(playerAttackStateMachine.nullState);
            return;
        }
    }
}
