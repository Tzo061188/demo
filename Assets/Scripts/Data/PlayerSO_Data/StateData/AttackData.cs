using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable,CreateAssetMenu(menuName ="ScriptableObjectData/Data/AttackData",fileName = "AttackData")]
public class AttackData:ScriptableObject{

    [Header("伤害来源")]public GameObject attacker; //传递攻击者
    [Header("攻击动画的名称")]public string attackName;
    [Header("攻击伤害")]public int attackDamage;
    [Header("防御累计的处决值")]public int parryExecuteCount;
    [Header("攻击距离")]public float attackDistance;

    [Header("攻击类型")]public Game_Enum.AttackState attackStatel;

    [Header("攻击特效预制体")]public GameObject effectPrefab;
    [Header("攻击武器音效")]public AudioClip weaponAudio;
    [Header("角色声音")]public AudioClip roleAudio;

    [Header("攻击敌人播放的受伤动画名称")] public string hitName;
    [Header("攻击敌人播放的防御动画名称")] public string parryName;



}
