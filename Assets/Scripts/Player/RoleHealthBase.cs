using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleHealthBase : MonoBehaviour
{
    public int currentHp;
    public int maxHp;

    public float currentExecuteCount; 
    public float maxExecuteCount; //处决计数量
    [Range(0,1)]public float speed_ExecuteCountReduce; //超过一定时间  处决值的减少速度
    protected Animator animator;
    
    protected virtual void Awake() {
        animator = GetComponentInChildren<Animator>();
        currentHp = maxHp;
        currentExecuteCount = 0;
    }

    public bool IsDie(){
        if(currentHp <= 0) return true;
        return false;
    }
    
    public bool IsCanExecute(){
        if(currentExecuteCount >= maxExecuteCount) return true;
        return false;
    }
}
