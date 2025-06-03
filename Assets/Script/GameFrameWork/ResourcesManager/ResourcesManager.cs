
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ResInfoBase { }
public class ResInfo<T> :  ResInfoBase {
    public T asset;
    //是否需要移除
    public bool IsDele;
    public UnityAction<T> action;
    public Coroutine coroutine;
}
public class ResourcesManager : SingletonBase<ResourcesManager>
{
    private Dictionary<string,ResInfoBase> resRecordDic = new Dictionary<string, ResInfoBase>();

    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <typeparam name="T">加载资源类型</typeparam>
    /// <param name="path">资源路径</param>
    /// <returns></returns>
    public T loadRes<T>(string path) where T: UnityEngine.Object
    {
        ResInfo<T> resinfo = null;
        //不存在
        if (!resRecordDic.ContainsKey(path))
        {

            resinfo = new ResInfo<T>();

            resinfo.asset = Resources.Load<T>(path);

            resRecordDic.Add(path, resinfo);

            return resinfo.asset;

        }
        else {
            resinfo = resRecordDic[path] as ResInfo<T>;
            //存在 资源还在异步加载中 停止异步加载 同步加载资源返回
            if (resinfo.asset == null)
            {
                MonoManager.Instance.StopCoroutine(resinfo.coroutine);

                resinfo.asset = Resources.Load<T>(path);

                resinfo.action?.Invoke(resinfo.asset);

                resinfo.coroutine = null;
                resinfo.action = null;

                return resinfo.asset;
            }
            //存在 返回
            else { 
                return resinfo.asset;
            }
        }
        
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <typeparam name="T">加载资源类型</typeparam>
    /// <param name="path">资源路径</param>
    /// <returns></returns>
    public T loadRes<T>(string path, T type) where T : Object 
    {   
        return Resources.Load<T>(path);         
    }



    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T">加载资源类型</typeparam>
    /// <param name="path">资源路径</param>
    /// <param name="action">回调函数</param>
    public void AsyncLoadRes<T>(string path, UnityAction<T> callBack) where T : Object
    {
        ResInfo<T> resInfo = null;
        //不存在
        if (!resRecordDic.ContainsKey(path))
        {

            resInfo = new ResInfo<T>();

            resInfo.action += callBack;

            resRecordDic.Add(path, resInfo);

            resInfo.coroutine = MonoManager.Instance.StartCoroutine(AsyncLoad<T>(path));


        }
        //存在
        else {

            resInfo = resRecordDic[path] as ResInfo<T>;
                //资源还没加载完成
            if (resInfo.asset == null)
                resInfo.action += callBack;
            else
                //资源加载完成了
                callBack?.Invoke(resInfo.asset);

        }
        
    }
    private IEnumerator AsyncLoad<T>(string path) where T:Object 
    {
        ResourceRequest request =  Resources.LoadAsync<T>(path);
        yield return request;



        ResInfo<T> resInfo =  resRecordDic[path] as ResInfo<T>;

        resInfo.asset = request.asset as T;
        //资源待移除
        if (resInfo.IsDele) {
            UnloadRes<T>(path);
            yield break;
        }

        resInfo.action?.Invoke(resInfo.asset);
        //执行完成清了
        resInfo.action = null;
        resInfo.coroutine = null;
    }


    /// <summary>
    /// 卸载指定的资源 只能卸载不需要实例化的对象 如脚本 音频
    /// </summary>
    /// <param name="obj"></param>
    public void UnloadRes<T>(string path) where T : Object
    {

        if (resRecordDic.ContainsKey(path))
        {
            ResInfo<T> resInfo = resRecordDic[path] as ResInfo<T>;

            if (resInfo.asset != null)
            {

                resRecordDic.Remove(path);
                Resources.UnloadAsset(resInfo.asset);
            }
            else {
                
                //待加载完成后移除
                resInfo.IsDele = true;
            }

        }
       

    }


    /// <summary>
    /// 卸载所有没用的资源
    /// </summary>
    /// <param name="callback"></param>
    public void UnloadAllonUseRes(UnityAction callback) {
        resRecordDic.Clear();
        MonoManager.Instance.StartCoroutine(UnloadAll(callback));
    }
    public IEnumerator UnloadAll(UnityAction callback) {
        AsyncOperation operation =  Resources.UnloadUnusedAssets();
        yield return operation;
        callback();
    }
}
