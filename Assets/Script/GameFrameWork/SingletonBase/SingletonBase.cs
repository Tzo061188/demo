using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不继承MonoBehaviour 的单例基类
/// </summary>
/// <typeparam name="T">类</typeparam>
public abstract class SingletonBase<T> where T : class, new () 
{
    private static T instance;

    protected static readonly object lockObj = new object();
    public static T Instance
    {
        get {

            if (instance == null) {
                lock (lockObj) { 
                    if(instance == null)
                        instance = new T();
                }            
            }
            return instance;
        }
    }
    public static T GetInstance(){
        if(instance == null)
            instance = new T();
        return instance;                
    }
}
