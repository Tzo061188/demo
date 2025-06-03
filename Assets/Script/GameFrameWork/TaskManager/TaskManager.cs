using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : SingletonBase_MonoAuto<TaskManager>
{
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    public List<TaskData> allTask = new List<TaskData>();

    public TaskData GetTask(int taskID){
        foreach (TaskData task in allTask)
        {
            if(task.taskID == taskID)
                return task;
        }
        return null;
    }

}
