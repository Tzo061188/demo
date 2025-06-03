using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MovementData
{
    public IdleData idleData = new IdleData();
    public WalkData walkData = new WalkData();
    public RunData runData = new RunData();
}


[Serializable] 
public class IdleData{
    [Header("AnimatorID中的Int值")]public int movementValue;
    [Header("角色速度")]public float playerSpeed;
}
[Serializable]
public class WalkData{
    [Header("AnimatorID中的Int值")]public int movementValue;
    [Header("角色速度")]public float playerSpeed;
}
[Serializable]
public class RunData{
    [Header("AnimatorID中的Int值")]public int movementValue;
    [Header("角色速度")]public float playerSpeed;
}