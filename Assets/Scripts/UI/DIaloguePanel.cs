using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialoguePanel : DIaloguePanelBase
{
	protected override void Start(){
		base.Start();
	}

    public override void ShowPanel()
    {
        this.gameObject.SetActive(true);

		//初始化任务数据描述

		if(GameDataManager.Instance.NpcTaskList_Dic.ContainsKey(GameManager.Instance.NPC.name)){
			List<TaskData> tastDatas = GameDataManager.Instance.NpcTaskList_Dic[GameManager.Instance.NPC.name];
			foreach (TaskData task in tastDatas)
			{
				//遇到没完成的任务
				if(task.isDone == 0){
					UpdatePanelInfo(task);
				}
			}

		}
    }
    public override void HidePanel()
    {
        this.gameObject.SetActive(false);
    }


    protected override void OnButton_CancelClick()
    {
        this.gameObject.SetActive(false);
    }
    protected override void OnButton_CloseClick()
    {
        this.gameObject.SetActive(false);
    }

	private void UpdatePanelInfo(TaskData task){
		//如果任务接受了的话 不在显示接受和取消按钮  显示OK按钮
		Button_Accept.gameObject.SetActive(false);
		Button_Cancel.gameObject.SetActive(false);
		Button_OK.gameObject.SetActive(false);

		TaskName.text = task.taskName;
		Task.text = task.taskDescribe;
		Reward.text = task.rewardDescribe;
	}

}