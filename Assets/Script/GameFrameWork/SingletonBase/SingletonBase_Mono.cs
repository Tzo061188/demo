using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



/// <summary>
/// 继承了MonoBehaviour 的 单例基类
/// </summary>
/// <typeparam name="T">类名</typeparam>
[DisallowMultipleComponent]//一个游戏物体只允许挂载一个该组件
public class SingletonBase_Mono<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;
    public static T Instance { 

        get {       
            return instance; 
        }    
    }
    protected virtual void Awake()
    {
        if (instance != null) {
            Debug.LogWarning("这个组件" + typeof(T).ToString() + "挂载了多次");
            Destroy(this);
            return;
        }
        instance = this as T;
    }
    //----需要手动挂载在游戏物体上
    //----是否移除 看情况自己添加
}
