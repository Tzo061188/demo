using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EnemyAttackBase
{
    public Animator animator;
    public Transform roleTransform;
    public SoundData SoundData;

    public bool isCanAttack; //可以进行下次攻击
    public AttackContentData currentAttackMode;//当前的一个攻击形式   
    public bool isAttackExecute; //进行攻击执行    
    public AttackModuleData allAttackMode;//所有的攻击形式 包括普通攻击，技能
    public int index;
    
    public EnemyAttackBase(Animator animator,Transform roleTransform,AttackModuleData allAttackModule){
        this.animator = animator;
        this.roleTransform = roleTransform;

        this.allAttackMode = allAttackModule; 
    }

    #region 是否能够进行攻击、技能的使用
        
        public bool IsCanNormalAttack(){
            
            if(animator.CheckAnimation_TagIs(0,"Hit")) return false;
            if(animator.CheckAnimation_TagIs(0,"Spring")) return false;
            if(animator.CheckAnimation_TagIs(0,"NormalSkill")) return false;
            if(animator.CheckAnimation_TagIs(0,"FinalSkill")) return false;
            return true;
        }

        public bool IsCanSkillAttack(){
            
            if(animator.CheckAnimation_TagIs(0,"Hit")) return false;
            if(animator.CheckAnimation_TagIs(0,"Spring")) return false;
            if(animator.CheckAnimation_TagIs(0,"Attack")) return false;
            return true;
        }
    #endregion 


 


    #region 攻击模组切换
        
        //使用轻攻击
        public void UseLightAttack(){
            if(allAttackMode.lightAttack == null) return;

            if(currentAttackMode != allAttackMode.lightAttack || currentAttackMode == null){
                currentAttackMode = allAttackMode.lightAttack;
            }
            //执行
            ExecuteAttack();
        }

        
        //重攻击
        public void UseHeavyAttack(){
            if(allAttackMode.heavyAttack == null) return;

            if(currentAttackMode != allAttackMode.heavyAttack || currentAttackMode == null){
                currentAttackMode = allAttackMode.heavyAttack;
                
            }
            ExecuteAttack();
        }

        public void UseFinalSkillAttack(){
            if(allAttackMode.finalSkillAttack == null) return;

            if(currentAttackMode != allAttackMode.finalSkillAttack || currentAttackMode == null){
                currentAttackMode = allAttackMode.finalSkillAttack;
                
            }
            ExecuteAttack();
        }

        public void UseNormalSkillAttack(){
            if(allAttackMode.normalSkillAttack == null) return;
           
            if(currentAttackMode != allAttackMode.normalSkillAttack || currentAttackMode == null){
                currentAttackMode = allAttackMode.normalSkillAttack;
                
            }
            ExecuteAttack();
        }

    #endregion
    //更新攻击动画
    public void UpdateAttackAnimation(){
        //通过bool值做开关
        if(!isAttackExecute) return; //是否可以执行攻击指令
        if(!isCanAttack) return; //是否能够进行攻击了 、预输入开关 

        //随机下标 
        index = UnityEngine.Random.Range(0,currentAttackMode.GetCount());

        string animationName = currentAttackMode.GetCurrentAttackName(index);
        
        // 动画过渡过去 
        animator.CrossFadeInFixedTime(animationName,0.1f,0);
        // animatorId 过度过去
        //这里用animatorId 配置name没意义  不通用
        //animator.SetTrigger(AnimatorID.Attack);
        

        isAttackExecute = false; //执行完成
        isCanAttack = false; //不可攻击
    }
    private void ExecuteAttack()
    {
        if(currentAttackMode != null){

            isAttackExecute = true;
            
        }
    }

    #region 动画事件
        public void CanNextAttack(){
            isCanAttack = true; //可以进行下一次攻击
        }


    #endregion 
   

  
}
