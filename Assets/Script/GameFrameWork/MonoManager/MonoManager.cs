using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 通过这个类 让没继承MonoBehaviour 的类可以执行update等方法帧更新 和通过这个类开启协程
/// 通过这个类 进行update的全局管理
/// </summary>
public class MonoManager : SingletonBase_MonoAuto<MonoManager>
{
    public event UnityAction update;
    public event UnityAction fixedUpdate;
    public event UnityAction lateUpdate;

    //添加方法到MonoManager的Update方法中执行
    public void AddUpdate(UnityAction action) 
    {
        update += action;
    }

    //添加方法到MonoManager的FixedUpdate方法中执行
    public void AddFixedUpdate(UnityAction action)
    {
        fixedUpdate += action;
    }

    //添加方法到MonoManager的LateUpdate方法中执行
    public void AddLateUpdate(UnityAction action)
    {
        lateUpdate += action;
    }

    //移除方法从MonoManager的Update方法
    public void RemoveUpdate(UnityAction action) 
    {  
        update -= action; 
    }

    //移除方法从MonoManager的FixedUpdate方法
    public void RemoveFixedUpdate(UnityAction action) 
    { 
        fixedUpdate -= action;
    }

    //移除方法从MonoManager的LateUpdate方法
    public void RemoveLateUpdate(UnityAction action) 
    { 
        lateUpdate -= action;
    }
    private void Update()
    {
        update?.Invoke();
    }
    private void FixedUpdate()
    {
        fixedUpdate?.Invoke();
    }
    private void LateUpdate()
    {
        lateUpdate?.Invoke();
    }
}
