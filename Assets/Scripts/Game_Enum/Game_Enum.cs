using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Enum
{

    public enum AttackState{ Attack,Skill}
    public enum AnimationStateName{
        Null,
        Idle, 
        Run, 
        Walk,
        Attack,
        Sprint,
        Dash,
        DashBack,
        TurnBack,
        Skill
    
    }
    public enum Enemy_AnimationStateName{
        lightAttack,
        SphereSkill,
        BoxSkill,
    }
    public enum PlayerSoundType{
        Foot,FootBack,Attack,Sprint,PlayerAttackVoice,PutWeapon,PlayerMoveVoice,Parry,Hit
    }
}
