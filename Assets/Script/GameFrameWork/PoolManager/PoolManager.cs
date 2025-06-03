using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池管理器
/// </summary>
public class PoolManager : SingletonBase<PoolManager>
{
    

    private Dictionary<string , PoolData> poolDic = new Dictionary<string , PoolData>();

    //根对象
    private GameObject RootObj;

    //是否开启关系设置
    public static bool IsOpenRelation = true;

   /// <summary>
   /// 从池子中 拿出
   /// </summary>
   /// <param name="path">路径</param>
   /// <param name="objName">物体名字</param>
   /// <returns></returns>
    public GameObject TakeObj(string path,string objName) {

        GameObject obj = null;
        //存在 且 有
        if (poolDic.ContainsKey(objName) && poolDic[objName].Count > 0)
        {
            obj = poolDic[objName].TakeObj();
        }
        //不存在 或 没有
        else {
            //可以改成异步
            obj = GameObject.Instantiate(ResourcesManager.Instance.loadRes<GameObject>(path + objName));
        }

        obj.name = objName; 
        

        return obj;
    }
    /// <summary>
    /// 放回池子中
    /// </summary>
    /// <param name="obj">物体</param>
    public void PutObj(GameObject obj) {

        if (RootObj == null && IsOpenRelation) {
            RootObj = new GameObject("Pool");
            GameObject.DontDestroyOnLoad(RootObj);
        }

        
        if (!poolDic.ContainsKey(obj.name)) {

            poolDic.Add(obj.name, new PoolData(RootObj,obj.name));
        }
        
        poolDic[obj.name].PutObj(obj);

    }
    //跳转场景时释放引用
    public void Release() { 

        poolDic.Clear();
        RootObj = null;
    }
}


//池子数据对象
public class PoolData {

    private Queue<GameObject> data = new Queue<GameObject>();

    //长度
    public int Count => data.Count;

    //每个池子的对象
    public GameObject poolObj;

    //通过构造方法连接根对象  建立子父级关系
    public PoolData(GameObject RootObj,string PoolName) {

        if (RootObj != null && PoolManager.IsOpenRelation) { 

            poolObj = new GameObject(PoolName);
            poolObj.transform.SetParent(RootObj.transform);
        }

    }
    //拿
    public GameObject TakeObj()
    {
        GameObject obj =  data.Dequeue();

        obj.SetActive(true);

        obj.transform.SetParent(null);

        return obj;
    }
    //放
    public void PutObj(GameObject obj)
    {

        obj.SetActive(false);

        if(PoolManager.IsOpenRelation)
        obj.transform.SetParent(poolObj.transform);

        data.Enqueue(obj);
  
    }
}