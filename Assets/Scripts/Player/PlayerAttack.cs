using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : RoleAttackBase
{
    public PlayerAttack(Animator animator, Transform roleTransform,AttackModuleData allAttackModule) : base(animator, roleTransform,allAttackModule)
    {
    }

    public void OnLightAttack(){
        if(IsCanNormalAttack()){
            UseLightAttack();
        }
    }

    public void OnHeavyAttack(){
        if(IsCanNormalAttack()){
            UseHeavyAttack();
        }
    }

    public void EnablePreInput(){
        AttackInput();
    }
    public void CancelAttackColdTime(){
        CanNextAttack();
    }

}
