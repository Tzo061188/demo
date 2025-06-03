using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class RoleAttackBase
{
    public Animator animator;
    public Transform roleTransform;
    public SoundData playerSoundData;
    public PlayerAttributeData playerAttributeData;

    //所有的攻击形式 包括普通攻击，技能
    public AttackModuleData allAttackMode;

    
    public RoleAttackBase(Animator animator,Transform roleTransform,AttackModuleData allAttackModule){
        this.animator = animator;
        this.roleTransform = roleTransform;
        playerSoundData = roleTransform.GetComponent<Player>().playerSoundDataSO;
        this.allAttackMode = allAttackModule;
        playerAttributeData = new PlayerAttributeData(); 
    }

    #region 是否能够进行攻击、技能的使用
        
        public bool IsCanNormalAttack(){
            if(!playerAttributeData.isAttackInput) return false; //不可以输入 
            if(animator.CheckAnimation_TagIs(0,"Hit")) return false;
            if(animator.CheckAnimation_TagIs(0,"Spring")) return false;
            if(animator.CheckAnimation_TagIs(0,"Skill")) return false;
            return true;
        }

        public bool IsCanSkillAttack(){
            if(!playerAttributeData.isAttackInput) return false;
            if(animator.CheckAnimation_TagIs(0,"Hit")) return false;
            if(animator.CheckAnimation_TagIs(0,"Spring")) return false;
            if(animator.CheckAnimation_TagIs(0,"Attack")) return false;
            return true;
        }
    #endregion 


    //更新当前攻击模式下一次攻击index的更新
    public void UpdateAttackIndex(){
        playerAttributeData.currentAttackIndex ++;

        if(playerAttributeData.currentAttackIndex > playerAttributeData.currentAttackMode.GetCount()-1){
            
            playerAttributeData.currentAttackIndex = 0;

        }
    }
    //重置 数据
    public void ResetData(){
        playerAttributeData.currentAttackIndex = 0;
        playerAttributeData.isCanAttack = true;
        playerAttributeData.isCanMoveInterrupt = false;
        playerAttributeData.isAttackInput = true;
        
    }


    #region 攻击模组切换
        
        //使用轻攻击
        public void UseLightAttack(){
            if(allAttackMode.lightAttack == null) return;

            if(playerAttributeData.currentAttackMode != allAttackMode.lightAttack || playerAttributeData.currentAttackMode == null){
                playerAttributeData.currentAttackMode = allAttackMode.lightAttack;
                playerAttributeData.currentAttackIndex = 0;
            }
            //执行
            if(playerAttributeData.currentAttackMode != null)
                playerAttributeData.isAttackExecute = true;
        }

        
        //重攻击
        public void UseHeavyAttack(){
            if(allAttackMode.heavyAttack == null) return;

            if(playerAttributeData.currentAttackMode != allAttackMode.heavyAttack || playerAttributeData.currentAttackMode == null){
                playerAttributeData.currentAttackMode = allAttackMode.heavyAttack;
                playerAttributeData.currentAttackIndex = 0;
            }
            ExecuteAttack();
        }

        public void UseFinalSkillAttack(){
            if(allAttackMode.finalSkillAttack == null) return;

            if(playerAttributeData.currentAttackMode != allAttackMode.finalSkillAttack || playerAttributeData.currentAttackMode == null){
                playerAttributeData.currentAttackMode = allAttackMode.finalSkillAttack;
                playerAttributeData.currentAttackIndex = 0;
            }
            ExecuteAttack();
        }

        public void UseNormalSkillAttack(){
            if(allAttackMode.normalSkillAttack == null) return;
           
            if(playerAttributeData.currentAttackMode != allAttackMode.normalSkillAttack || playerAttributeData.currentAttackMode == null){
                playerAttributeData.currentAttackMode = allAttackMode.normalSkillAttack;
                playerAttributeData.currentAttackIndex = 0;
            }
            ExecuteAttack();
        }

    #endregion
    //更新攻击动画
    public void UpdateAttackAnimation(){
        //通过bool值做开关
        if(!playerAttributeData.isAttackExecute) return; //是否可以执行攻击指令
        if(!playerAttributeData.isCanAttack) return; //是否能够进行攻击了 、预输入开关 

        string animationName = playerAttributeData.currentAttackMode.GetCurrentAttackName(playerAttributeData.currentAttackIndex);
        
        // 动画过渡过去 
        animator.CrossFadeInFixedTime(animationName,0.1f,0);
   
        //index++
        UpdateAttackIndex();

        playerAttributeData.isAttackExecute = false; //执行完成
        playerAttributeData.isCanAttack = false; //不可攻击
    }
    private void ExecuteAttack()
    {
        if(playerAttributeData.currentAttackMode != null){

            playerAttributeData.isAttackExecute = true;
            playerAttributeData.isAttackInput = false;
        }
    }

    #region 动画事件
        public void CanNextAttack(){
            playerAttributeData.isCanAttack = true; //可以进行下一次攻击
        }

        public void AttackInput(){
            playerAttributeData.isAttackInput = true;
        }
    #endregion 
   

  
}
