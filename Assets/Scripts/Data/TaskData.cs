using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TaskData
{
    public TaskData(){}
    public TaskData(int taskID,int isDone,int isAccept,string taskName,string taskDescribe,string rewardDescribe,int Target){
        this.taskID = taskID;
        this.isAccept = isAccept;
        this.isDone = isDone;
        this.taskDescribe = taskDescribe;
        this.rewardDescribe = rewardDescribe;
        this.taskName = taskName;
        this.Target = Target;
    }

    public int taskID;
    //是否完成了
    public int isDone;
    //是否接受了
    public int isAccept;

    public int Target;  //任务目标

    //任务需要的东西和数量  需要杀死的敌人和数量
    public List<TaskNeed> taskNeeds = new List<TaskNeed>();  

    //奖励的道具和数量 奖励的金币
    public List<TaskReward> taskRewards = new List<TaskReward>();

    #region 描述的名字 任务描述 奖励描述
        
        public string taskName;
           
        public string taskDescribe;
        public string rewardDescribe;


        // public string rewardDescribe{
        //      get{
        //         string describe = "";
        //         foreach(TaskReward taskReward in taskRewards){
        //             if(taskReward.rewardItem != null)
        //                 describe += $"获得物品 {taskReward.rewardItem} x {taskReward.rewardItemNumber}个\n";
        //             if(taskReward.rewardGoldNumber != 0)
        //                 describe += $"获得金币 {taskReward.rewardGoldNumber}个\n";
        //         }
        //         return describe;
        //     }
        //     set{
        //         taskDescribe = value;
        //     }
        // }
        
    #endregion


        

}
//任务需要类
public class TaskNeed{

    //需要的物品
    public string needItemName;
    //需要的物品的数量
    public int needNumber ;
    //完成的物品数量
    public int finishNumber;
    //需要杀死的敌人
    public EnemyBase needKillEnemy;
    //需要杀死的敌人数量
    public int killNumber;
    //完成杀死的敌人数量
    public int finishKillNumber;
}

//任务奖励类
public class TaskReward{
    //奖励的物品
    public string rewardItem;
    //奖励的物品数量
    public int rewardItemNumber;
    //奖励的金币数量
    public int rewardGoldNumber;
}
