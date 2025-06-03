using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 继承了MonoBehaviour 的 单例基类
/// </summary>
/// <typeparam name="T">类名</typeparam>
public class SingletonBase_MonoAuto<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get {
            if (instance == null) {    
                GameObject gameObj = new GameObject(typeof(T).ToString());
                instance =  gameObj.AddComponent<T>();
            }
            return instance;
        }
    }
    // ----在调用时自动创建游戏物体 自行挂载
    // ----是否移除 看情况自己添加

}
