using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable,CreateAssetMenu(fileName ="AttackContentData",menuName ="ScriptableObjectData/Data/AttackContentData")]
public class AttackContentData:ScriptableObject
{
    

    [Header("攻击CD")] public float attackCD;
    [Header("是否可以执行攻击")]public bool IsCanExcute; 
    
    public List<AttackData> attackContents = new List<AttackData>();
    


    public int GetCount(){
        return attackContents.Count;
    }

    public string GetCurrentAttackName(int index){
        if(attackContents.Count == 0) return null;
        if(index < 0 || index > attackContents.Count-1) return null;
        return attackContents[index].attackName ;
    }
    public string GetCurrentHitName(int index){
        if(attackContents.Count == 0) return null;
        if(index < 0 || index > attackContents.Count-1) return null;
        return attackContents[index].hitName ;
    }
    public string GetCurrentParryName(int index){
        if(attackContents.Count == 0) return null;
        if(index < 0 || index > attackContents.Count-1) return null;
        return attackContents[index].parryName ;
    }
    public int GetCurrentAttackDamage(int index){
        if(attackContents.Count == 0) return 0;
        if(index < 0 || index > attackContents.Count-1) return 0;
        return attackContents[index].attackDamage ;
    }

    public float GetCurrentAttackDistance(int index){
        if(attackContents.Count == 0) return 0;
        if(index < 0 || index > attackContents.Count-1) return 0;
        return attackContents[index].attackDistance ;
    }

     //回复CD
    public void ResetCD(){
        if(IsCanExcute == false)  
            PoolManager.Instance.TakeObj("Prefabs/","Timer").GetComponent<Timer>().CreateTimer(attackCD,()=>{
                IsCanExcute = true;
            },false);
    }
}
