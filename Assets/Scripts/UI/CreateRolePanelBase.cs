using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CreateRolePanelBase : PanelBase
{
	//自动生成UI控件变量
	protected Button Button_Left;
	protected Button Button_Right;
	protected Button Button_OK;

	protected InputField InputField_Name;

	
	protected virtual void Start(){
		//自动查找绑定UI
		Button_Left = this.transform.Find("Button_Left").GetComponent<UnityEngine.UI.Button>();
		Button_Right = this.transform.Find("Button_Right").GetComponent<UnityEngine.UI.Button>();
		Button_OK = this.transform.Find("Button_OK").GetComponent<UnityEngine.UI.Button>();

		InputField_Name = this.transform.Find("InputField_Name").GetComponent<UnityEngine.UI.InputField>();

		
		//自动添加事件
		Button_Left.onClick.AddListener(OnButton_LeftClick);
		Button_Right.onClick.AddListener(OnButton_RightClick);
		Button_OK.onClick.AddListener(OnButton_OKClick);
		InputField_Name.onValueChanged.AddListener(OnInputField_NameValueChanged);
			
	}

   
  

    //自动生成事件函数
    protected virtual void OnButton_LeftClick(){}
	protected virtual void OnButton_RightClick(){}
	protected virtual void OnButton_OKClick(){}
	protected virtual void OnInputField_NameValueChanged(string text){}

    public override void ShowPanel(){}

    public override void HidePanel(){}


}