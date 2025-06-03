using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackStateMachine : StateMachine
{   
    public Player player;
    public Animator animator;
    public PlayerAttack playerAttack;
    
    public NormalAttackState normalAttackState;
    public NullState nullState;
    public ParryState parryState;
    public SkillState normalskillState;
    public FinishSkillState finishSkillState;

    public PlayerAttackStateMachine(Player player,Animator animator)
    {
        this.player = player;
        this.animator = animator;
        this.playerAttack = new PlayerAttack(animator,player.transform,player.playerSO.attackModuleData);
        this.normalAttackState = new NormalAttackState(this);
        this.nullState = new NullState(this);
        this.parryState = new ParryState(this);
        this.normalskillState = new SkillState(this);
        this.finishSkillState =  new FinishSkillState(this);
    }
}
