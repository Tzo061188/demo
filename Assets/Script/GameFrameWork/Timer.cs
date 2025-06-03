using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float timer;
    public UnityAction action;
    public bool isFinish;
    public bool isCancel;
    
    void Update()
    {
        Execute();
        PutInPool();
    }

    public void Execute(){

        if(gameObject.activeSelf == false) return;

        if(timer>0 && !isFinish){

            timer -= Time.deltaTime;

            if(timer < 0 && isCancel == false){ //不取消执行
                action?.Invoke();
                isFinish = true;
            }  
        }
        
    }

    public void PutInPool(){
        if(isFinish || isCancel){
            timer = 0;
            action = null;
            PoolManager.Instance.PutObj(this.gameObject);
        }
    }

    public void CreateTimer(float timer,UnityAction action,bool isFinish){
        this.timer = timer;
        this.action = action;
        this.isFinish = isFinish;
        isCancel = false;
    } 

    public void CreateTimer(float timer,UnityAction action,bool isFinish,out Timer thistimer){
        this.timer = timer;
        this.action = action;
        this.isFinish = isFinish;
        isCancel = false;
        thistimer = this;
    }
}
