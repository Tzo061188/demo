using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovementState : IState
{
    protected PlayerMovementStateMachine playerMovementStateMachine;
    protected Animator animator;
    protected Transform playerTransform;


    //控制Animator的Movement输入值的大小
    protected float inputMultiple;
    
    public PlayerAttributeData playerAttributeData;
    

    //对变量进行赋值
    protected PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine){
        this.playerMovementStateMachine = playerMovementStateMachine;
        animator = playerMovementStateMachine.animator;
        playerTransform = playerMovementStateMachine.player.transform;
        playerAttributeData = playerMovementStateMachine.playerAttributeData;
    }




    //进行状态切换
    public virtual void AnimationChangeEvent(IState state)
    {
        playerMovementStateMachine.ChangedState(state);
    }
    //状态进行退出
    public virtual void AnimationExitEvent()
    {
        
    }

    

    public virtual void Enter()
    {

        RoleInputSystem.Instance.roleInput.Role.Sprint.started += ToSprintState;

    }

    public virtual void Exit()
    {
        //移除输入事件
        RoleInputSystem.Instance.roleInput.Role.Sprint.started -= ToSprintState;

    }

    public virtual void HandInput()
    {
        animator.SetFloat(AnimatorID.MovementID,inputMultiple,0.1f,Time.deltaTime);
    }

    public virtual void Update()
    {
        if(animator.CheckAnimation_TagIs(0,"Attack") || animator.CheckAnimation_TagIs(0,"Parry")
        || animator.CheckAnimation_TagIs(0,"Skill") || animator.CheckAnimation_TagIs(0,"Parry_OK")) return;
            PlayerRotation();

    }
    //处理角色移动时的旋转
    private float currentVelocity = 0;
    public void PlayerRotation(){

        if(playerMovementStateMachine.player.IsLockLook) return ;

        if(RoleInputSystem.Instance.Move != Vector2.zero ){
            
            float angle =  Mathf.Atan2(RoleInputSystem.Instance.Move.x,RoleInputSystem.Instance.Move.y)*Mathf.Rad2Deg + 
            playerMovementStateMachine.player.CameraTrans.eulerAngles.y;
            playerTransform.eulerAngles =  Vector3.up *
            Mathf.SmoothDampAngle(playerTransform.eulerAngles.y,angle,ref currentVelocity,0.1f);
        
        }
    }
    //所有移动状态都可以切换冲刺状态
    private void ToSprintState(CallbackContext context){

        if(GameManager.Instance.isPlayerCanMove == false)
            return;

        if(playerAttributeData.isCanSprint  && animator.CheckAnimation_TagIs(0,"Sprint") == false){

            //TODO：闪避打断  需要细化
            if(!animator.CheckAnimation_TagIs(0,"Skill") && !animator.CheckAnimation_TagIs(0,"Attack"))
                animator.CrossFadeInFixedTime("Sprint_Front",0.1f);
            
        }
    }

}
