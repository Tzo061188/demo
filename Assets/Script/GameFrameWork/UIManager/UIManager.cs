using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIManager : SingletonBase_Mono<UIManager>
{
    private Dictionary<string,PanelBase> panelsDic = new Dictionary<string,PanelBase>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }
    public void ShowPanel<T>(string path) where T : PanelBase 
    {
        string key =  typeof(T).Name;
        //�ҵ�
        if (panelsDic.ContainsKey(key))
        {
            panelsDic[key].ShowPanel();
            
        }
        //û�ҵ�
        else {
            //ռ��λ�� ��ֹ�ظ�����
            panelsDic.Add(key, null);
            ResourcesManager.Instance.AsyncLoadRes<T>(path, (panel) =>
            {
                /// <summary>
                /// 原来的加载： 存储和调用的不是实力化出来的对象和方法
                /// 经过更改
                /// </summary>
                /// <returns></returns>
                GameObject obj =  Instantiate(panel.gameObject,this.transform);
                panelsDic[key]= obj.GetComponent<T>();
                obj.GetComponent<T>().ShowPanel();
                // Instantiate(panel.gameObject,this.transform);
                // panelsDic[key]= panel;
                // panel.ShowPanel();
            });
        }
    }
    public void HidePanel<T>() where T : PanelBase
    {
        string key = typeof(T).Name;
        //�ҵ�
        if (panelsDic.ContainsKey(key))
            panelsDic[key].HidePanel();

    
    }
    public T GetPanel<T>() where T : PanelBase
    {
        string key = typeof(T).Name;
        //�ҵ�
        if (panelsDic.ContainsKey(key) && panelsDic[key] != null)
            return panelsDic[key] as T;
        else
            return null;
       
    }
}
