using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NullState : PlayerAttackState
{
    public NullState(PlayerAttackStateMachine playerAttackStateMachine) : base(playerAttackStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        RoleInputSystem.Instance.roleInput.Role.Attack.started += OnAttack;

        //重置数据
        playerAttack.ResetData(); 
    }
  

    //轻攻击 
    private void OnAttack(InputAction.CallbackContext context)
    {
        if(GameManager.Instance.isPlayerCanMove != true)
            return;

        if (player.currentAttackEnemy != null)
        {  
            
            player.transform.forward = player.currentAttackEnemy.transform.position - player.transform.position;
        }
        playerAttack.OnLightAttack();
    }
    //重攻击
    private void OnHeavyAttackUpdate(InputAction.CallbackContext context)
    {
        playerAttack.OnHeavyAttack();
    }
    
    public override void Exit()
    {
        base.Exit();
        RoleInputSystem.Instance.roleInput.Role.Attack.started -= OnAttack;
    
    }
}
