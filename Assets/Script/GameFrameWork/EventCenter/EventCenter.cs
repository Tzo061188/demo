using System;
using System.Collections;
using System.Collections.Generic;
using MyEventInfo;
using UnityEngine;
using UnityEngine.Events;

#region 封装传递事件信息的数据类 

namespace MyEventInfo{
    public abstract class EventInfoBase { }
    public class EventInfo<T> : EventInfoBase
    {
        public UnityAction<T> Actions;
        public EventInfo(UnityAction<T> action) {
            Actions += action;
        }
    }

    public class EventInfo : EventInfoBase
    {
        public UnityAction Actions;
        public EventInfo(UnityAction action)
        {
            Actions += action;
        }
    }
}

#endregion

//所有事件的名字
public enum AllEventName { 

    /// <summary>
    /// 空事件（例子） ---转递的参数类型：null
    /// </summary>
    NULL,
    /// <summary>
    /// 加分事件 ---无参
    /// </summary>
    AddScore,
    /// <summary>
    /// 加血 -- 转递的参数类型：int
    /// </summary>
    AddHP,

    /// <summary>
    /// 触发攻击 -- 转递的参数类型：AttackData  玩家
    /// </summary>
    TiggerAttack,

    /// <summary>
    /// 触发攻击 -- 转递的参数类型：AttackData 敌人的
    /// </summary>
    EnemyTiggerAttack,

    /// <summary>
    /// 触发处决 -- 转递的参数类型：AttackData 敌人的
    /// </summary>
    TiggerExecution,

    /// <summary>
    /// 添加任务 -- 
    /// </summary>
    AddTask,

    /// <summary>
    /// 滴人死亡
    /// </summary>
    EnemyDie,

}

/// <summary>
/// 事件中心
/// </summary>
public class EventCenter : SingletonBase<EventCenter> { 

    private Dictionary<AllEventName, EventInfoBase> eventDic = new Dictionary<AllEventName, EventInfoBase>();

    /// <summary>
    /// 触发事件 带参数
    /// </summary>
    /// <param name="eventName">想要触发的事件名字</param>
    public void EventTrigger<T>(AllEventName eventName , T info) {

        if (eventDic.ContainsKey(eventName)) 
            (eventDic[eventName] as EventInfo<T>).Actions ?.Invoke(info);
    }

    /// <summary>
    /// 触发事件 不带参数
    /// </summary>
    /// <param name="eventName">想要触发的事件名字</param>
    public void EventTrigger(AllEventName eventName)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo).Actions ?.Invoke();
    }

    /// <summary>
    /// 添加想要监听的事件 带参数
    /// </summary>
    /// <param name="eventName">添加想要监听的事件名字</param>
    /// <param name="action">想要执行的事件方法</param>
    public void AddEventListener<T>(AllEventName eventName , UnityAction<T> action) {

        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo<T>).Actions += action;
        else  
            eventDic.Add(eventName, new EventInfo<T>(action));
    }

    #region 提供给lua的方法
        /// <summary>
        /// 添加想要监听的事件 带参数
        /// </summary>
        /// <param name="eventName">添加想要监听的事件名字</param>
        /// <param name="action">想要执行的事件方法</param>
        public void AddEventListener_InLua(AllEventName eventName , UnityAction<int> action){
            
            
            if (eventDic.ContainsKey(eventName)){

                (eventDic[eventName] as EventInfo<int>).Actions += action;

            }
            else{
                eventDic.Add(eventName, new EventInfo<int>(action));

            }  
        }

        /// <summary>
        /// 触发事件 带参数
        /// </summary>
        /// <param name="eventName">想要触发的事件名字</param>
        public void EventTrigger_InLua(AllEventName eventName , int index)
        {
            if (eventDic.ContainsKey(eventName)) {

                (eventDic[eventName] as EventInfo<int>).Actions ?.Invoke(index);

            }
        }
    #endregion



    /// <summary>
    /// 添加想要监听的事件 不带参数
    /// </summary>
    /// <param name="eventName">添加想要监听的事件名字</param>
    /// <param name="action">想要执行的事件方法</param>
    public void AddEventListener(AllEventName eventName, UnityAction action)
    {

        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo).Actions += action;
        else
            eventDic.Add(eventName, new EventInfo(action));
    }

    /// <summary>
    /// 移除监听事件 带参数
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="action"></param>
    public void RemoveEventListener<T>(AllEventName eventName,UnityAction<T> action) {
        if(eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo<T>).Actions -= action;
    }

    /// <summary>
    /// 移除监听事件 不带参数
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(AllEventName eventName, UnityAction action)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo).Actions -= action;
    }

    //清除所有监听事件
    public void Clear() {
        eventDic.Clear();
    }

    //清除指定的监听事件
    public void Clear(AllEventName eventName) { 
        eventDic.Remove(eventName);
    }
}
