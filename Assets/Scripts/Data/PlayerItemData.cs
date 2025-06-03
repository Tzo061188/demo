using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataList
{
    public List<PlayerItemData> itemData_List = new List<PlayerItemData>();

    public List<TaskData> task_List = new List<TaskData>();

    public int Gold_number = 0;

    public float soundVolume = 0.5f;
    public float bgMusicVolume = 0.5f;
}

public class PlayerItemData{
    public int belongTo;  // -- 1 Bag  2 Fast  3 weapon 4 head  5 armor  6 decoration  7 gem
    public int id;
    public int pos;
    public int count;
    public PlayerItemData(){}
    public PlayerItemData(int belongTo,int id,int pos,int count){
        this.belongTo = belongTo;
        this.id = id;
        this.pos = pos;
        this.count = count;
    }
}