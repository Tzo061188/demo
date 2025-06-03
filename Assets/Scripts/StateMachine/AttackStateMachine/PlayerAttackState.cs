using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : IState
{
    public Player player;
    public Animator animator;
    public PlayerAttack playerAttack;
    protected bool IsCanExecute;
    public PlayerAttackStateMachine playerAttackStateMachine;

    public PlayerAttackState(PlayerAttackStateMachine playerAttackStateMachine)
    {
        player = playerAttackStateMachine.player;
        this.playerAttackStateMachine = playerAttackStateMachine;
        playerAttack = playerAttackStateMachine.playerAttack;
        animator = playerAttackStateMachine.animator;
    }

    public virtual void Enter(){

        RoleInputSystem.Instance.roleInput.Role.HeavyAttack.performed += OnParry;
        RoleInputSystem.Instance.roleInput.Role.HeavyAttack.canceled += OnExitParry;

       
        RoleInputSystem.Instance.roleInput.Role.NormalSkill.started += OnNormalSkill;
        RoleInputSystem.Instance.roleInput.Role.FinishSkill.started += OnFinishSkill;


        EventCenter.Instance.AddEventListener(AllEventName.TiggerExecution ,TiggerExecution);
    }




    public virtual void Update() {
        playerAttackStateMachine.playerAttack.UpdateAttackAnimation();
    }
   
    public virtual void Exit(){

               
        RoleInputSystem.Instance.roleInput.Role.HeavyAttack.performed -= OnParry;
        RoleInputSystem.Instance.roleInput.Role.HeavyAttack.canceled -= OnExitParry;

        RoleInputSystem.Instance.roleInput.Role.NormalSkill.started -= OnNormalSkill;
        RoleInputSystem.Instance.roleInput.Role.FinishSkill.started -= OnFinishSkill;
        
        EventCenter.Instance.RemoveEventListener(AllEventName.TiggerExecution ,TiggerExecution);
    }

      // 防御
    private void OnParry(InputAction.CallbackContext context){
        if(GameManager.Instance.isPlayerCanMove != true)
            return;
        if(animator.CheckAnimation_TagIs(0,"Skill") || animator.CheckAnimation_TagIs(0,"Attack")){
            return;
        }
        playerAttackStateMachine.ChangedState(playerAttackStateMachine.parryState);
    }
 
    // 取消防御
    private void OnExitParry(InputAction.CallbackContext context){
        if(GameManager.Instance.isPlayerCanMove != true)
            return;
        if(animator.CheckAnimation_TagIs(0,"Skill") || animator.CheckAnimation_TagIs(0,"Attack")){
            return;
        }
        playerAttackStateMachine.ChangedState(playerAttackStateMachine.nullState);
    }

    private void OnNormalSkill(InputAction.CallbackContext context){
        if(GameManager.Instance.isPlayerCanMove != true)
            return;
        if(animator.CheckAnimation_TagIs(0,"Skill")){
            return;
        }
        playerAttackStateMachine.ChangedState(playerAttackStateMachine.normalskillState);
    }

    private void OnFinishSkill(InputAction.CallbackContext context){
        if(GameManager.Instance.isPlayerCanMove != true)
            return;
        if(animator.CheckAnimation_TagIs(0,"Skill")){
            return;
        }
        playerAttackStateMachine.ChangedState(playerAttackStateMachine.finishSkillState);

    }

    //触发处决
    public void TiggerExecution(){
        IsCanExecute = true;
    }

    public virtual void AnimationChangeEvent(IState state){
        playerAttackStateMachine.ChangedState(state);
    }

    public virtual void AnimationExitEvent(){
        
    }
}
