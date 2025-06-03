using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StateInfo<T>{
    public T value = default(T);
    public Action<T> OnValueChange;

    public T Value{
        get{
            return value;
        }
        set{
            if(!value.Equals(this.value)){
                OnValueChange?.Invoke(value);
                this.value = value;
            }
        }
    }
}

public class StateMachine
{   
    //当前状态
    public StateInfo<IState> currentState = new StateInfo<IState>();

       /// <summary>
    /// 状态切换
    /// </summary>
    public void ChangedState(IState state){
        currentState.Value?.Exit();
        Debug.Log(state.GetType().Name);
        currentState.Value = state;
        currentState.Value.Enter();
    }
    /// <summary>
    /// 执行更新方法
    /// </summary>
    public void Update(){
        currentState.Value?.Update();
    }
    /// <summary>
    /// 执行输入方法
    /// </summary>
    public void HandInput(){
        currentState.Value?.HandInput();
    }
    /// <summary>
    /// 动画状态转换
    /// </summary>
    /// <param name="state"></param>
    public void AnimationChangeEvent(IState state){
        currentState.Value?.AnimationChangeEvent(state);
    }
    /// <summary>
    /// 动画退出
    /// </summary>
    public void AnimationExitEvent(){
        currentState.Value?.AnimationExitEvent();
    }


}
