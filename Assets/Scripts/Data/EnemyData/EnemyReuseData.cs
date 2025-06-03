using System;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class EnemyReuseData
{
    public string Name;
    public int enemyID;

    [Header("大于这个距离脱战")]public int detachDistance; //脱战距离
    [Header("大于这个距离追击敌人")]public int chaseMaxDistance;//超过这个距离就向前追击
    [Header("僵持间距")]public int standoffDistacne;//和敌人之间保持的最小间距
    [Header("与所攻击的敌人保持的最小间距")]public int keepMinDistacne;//和敌人之间保持的最小间距

    [Header("移动速度")]
    public float speed;
    public float chaseSpeed;

    [Header("是否有巡逻路径")]
    public List<Vector3> path;
    
    private int pathIndex;


    public int PathIndex
    {
        get{
            return pathIndex;
        }
        set{
           if(value > path.Count-1){
                pathIndex = 0;
           }else{
                pathIndex = value;
           }
        }
    }
            
}