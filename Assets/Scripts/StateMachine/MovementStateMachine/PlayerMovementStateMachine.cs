using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine : StateMachine
{
    public Player player;
    public PlayerHealthSystem playerHealthSystem;
    public PlayerSO playerSO;
    public Animator animator;
    public IdleState idleState;
    public WalkState walkState;
    public RunState runState;
    public SprintState sprintState;
    public PlayerNullMovementState playerNullMovementState;

    public PlayerAttributeData playerAttributeData;

    public PlayerMovementStateMachine(Player player,Animator animator)
    {
        this.player = player;
        this.animator = animator;
        
        playerHealthSystem = player.GetComponent<PlayerHealthSystem>();

        playerSO = player.playerSO;
        playerAttributeData = new PlayerAttributeData();
        
        idleState = new IdleState(this);
        walkState = new WalkState(this);
        runState = new RunState(this);
        sprintState = new SprintState(this);
        playerNullMovementState = new PlayerNullMovementState(this);
    }

}
