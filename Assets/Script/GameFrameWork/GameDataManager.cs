using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : SingletonBase_Mono<GameDataManager>
{
    //NPC 的所有任务
    public Dictionary<string,List<TaskData>> NpcTaskList_Dic = new Dictionary<string, List<TaskData>>();

}
