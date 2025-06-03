using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEditor;

public class CreateRolePanel : CreateRolePanelBase
{
    public int currentRoleIndex = 0;
    public Vector3 RolePos;
    
    public List<GameObject> Models;
    public List<GameObject> characters;
    public bool FirstLoading = false;
    private void Awake() {
        
    }
	protected override void Start(){
		base.Start();
        ShowPanel();

        //以下代码刚开始写在了showpanel中 运行后 characters这个list对象 有长度 但 存储的实例化对象
        // 报 missing 丢失 
        //执行顺序上 showPanel 先执行 Start后执行
           characters = new List<GameObject>();
            
            for (int i = 0; i < Models.Count; i++)
            {
                bool isHideOrShow = i > 0 ? false : true;
                
                characters.Add(Instantiate(Models[i],RolePos,Quaternion.AngleAxis(180,Vector3.up)));
                characters[i].SetActive(isHideOrShow);
            }
	}

	public override void ShowPanel(){

         
           
        this.gameObject.SetActive(true);
       

        
	}
    public override void HidePanel()
    {
        this.gameObject.SetActive(false);
    }



}