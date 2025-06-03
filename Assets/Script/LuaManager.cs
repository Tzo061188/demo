using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class LuaManager : SingletonBase<LuaManager>
{
    private LuaEnv env; 
    private byte[] Bytes;
    public LuaTable _G{
        get{
            if(env != null) return env.Global;
            return null; 
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(){
        if(env == null)
            env = new LuaEnv();
        env.AddLoader(CostomLoaderInAB);
        env.AddLoader(CostomLoader);
        
    }

    #region  资源重定向函数
    /// <summary>
    /// 从Asset/Lua/下加载lua脚本
    /// </summary>
    /// <param name="fileName">脚本文件名</param>
    private byte[] CostomLoader(ref string fileName){
        string path = Application.dataPath + "/Lua/"+fileName+".lua";
        if(File.Exists(path)){
            return File.ReadAllBytes(path);
        }else{
            Debug.Log("自定义加载文件未找到: "+path);
        }
        return null;
    }


    /// <summary>
    /// 从Asset/Lua/下加载lua脚本
    /// </summary>
    /// <param name="fileName">脚本文件名</param>
    private byte[] CostomLoaderInAB(ref string fileName){
        
     

        return Bytes;
    }


    #endregion
    


    /// <summary>
    /// 执行lua语法
    /// </summary>
    public void DoString(string str){
        if(env == null){
            Debug.Log("lua解析器未初始化");   
            return;
        } 

        env.DoString(str);
    }

    /// <summary>
    /// 执行lua文件
    /// </summary>
    public void DoLuaFile(string fileName){
        if(env == null){
            Debug.Log("lua解析器未初始化");   
            return;
        } 
        string str = string.Format("require('{0}')",fileName);

        env.DoString(str);
    }

    /// <summary>
    /// 垃圾回收
    /// </summary>
    public void Tick(){
        if(env == null){
            Debug.Log("lua解析器未初始化");   
            return;
        }
        env.Tick();
    }


    /// <summary>
    /// 释放销毁
    /// </summary>
    public void Dispose(){
        if(env == null){
            Debug.Log("lua解析器未初始化");   
            return;
        }
        env.Dispose();
        env = null;  
    }

}
