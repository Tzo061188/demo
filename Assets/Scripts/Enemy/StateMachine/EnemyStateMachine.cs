using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Animator animator;
    public Transform enemyTransform;

    // 这个怪物自己的数据
    public EnemyReuseData enemyReuseData;
    public EnemyAi enemyAi;
    public EnemyMovement enemyMovement;

    public EnemyNullState enemyNullState;
    public EnemyIdleState enemyIdleState;
    public EnemyPartolState enemyPartolState;
    public EnemyAttackAiState EnemyAttackAiState;
  

    public EnemyStateMachine(EnemyMovement enemyMovement,EnemyAi enemyAi,Animator animator,Transform enemyTransform,EnemyReuseData enemyReuseData)
    {
        this.enemyMovement = enemyMovement;
        this.enemyAi = enemyAi;
        this.animator = animator;
        this.enemyTransform = enemyTransform;
        this.enemyReuseData = enemyReuseData;

        enemyIdleState = new EnemyIdleState(this);
        enemyPartolState = new EnemyPartolState(this);
        enemyNullState = new EnemyNullState();
        EnemyAttackAiState = new EnemyAttackAiState(this);
    }

    
}