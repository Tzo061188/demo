using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DIaloguePanelBase : PanelBase
{
	//自动生成UI控件变量
	protected Button Button_Accept;
	protected Button Button_Close;
	protected Button Button_Cancel;
	protected Button Button_OK;

	protected Text Task;
	protected Text TaskName;		
	protected Text Reward;
	
	protected virtual void Start(){
		//自动查找绑定UI
		Button_Accept = this.transform.Find("BackGround/Button_Accept").GetComponent<UnityEngine.UI.Button>();
		Button_Close = this.transform.Find("BackGround/Button_Close").GetComponent<UnityEngine.UI.Button>();
		Button_Cancel = this.transform.Find("BackGround/Button_Cancel").GetComponent<UnityEngine.UI.Button>();
		Button_OK = this.transform.Find("BackGround/Button_OK").GetComponent<UnityEngine.UI.Button>();

		Task = this.transform.Find("BackGround/Task").GetComponent<UnityEngine.UI.Text>();
		TaskName = this.transform.Find("BackGround/TaskName").GetComponent<UnityEngine.UI.Text>();
		Reward = this.transform.Find("BackGround/Reward").GetComponent<UnityEngine.UI.Text>();
		
		//自动添加事件
		Button_Accept.onClick.AddListener(OnButton_AcceptClick);
		Button_Close.onClick.AddListener(OnButton_CloseClick);
		Button_Cancel.onClick.AddListener(OnButton_CancelClick);
		Button_OK.onClick.AddListener(OnButton_OKClick);	
	}


    //自动生成事件函数
    protected virtual void OnButton_OKClick(){}
    protected virtual void OnButton_AcceptClick(){}
	protected virtual void OnButton_CloseClick(){}
	protected virtual void OnButton_CancelClick(){}

    public override void ShowPanel()
    {
        throw new NotImplementedException();
    }

    public override void HidePanel()
    {
        throw new NotImplementedException();
    }

}