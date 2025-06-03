using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackModuleData
{   
    [Header("轻攻击数据")]
    public AttackContentData lightAttack;

    [Header("重攻击数据")]
    public AttackContentData heavyAttack;

    [Header("大招攻击数据")]
    public AttackContentData finalSkillAttack;

    [Header("小技能攻击数据")]
    public AttackContentData normalSkillAttack;
    

    [Header("防御技能数据")]
    public AttackContentData parrySkillAttack;
}
