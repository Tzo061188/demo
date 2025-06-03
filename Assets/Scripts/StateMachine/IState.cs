using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 所有状态接口
public interface IState{
    public virtual void Enter(){}
    public virtual void Exit(){}
    public virtual void Update(){}
    //处理输入
    public virtual void HandInput(){}
    //处理动画跳转下一个状态
    public virtual void AnimationChangeEvent(IState state){}
    //动画退出事件
    public virtual void AnimationExitEvent(){}

}
