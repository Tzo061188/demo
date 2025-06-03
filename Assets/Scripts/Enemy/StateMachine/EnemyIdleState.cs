
using UnityEngine;

public class EnemyIdleState : IState
{
    public EnemyStateMachine enemyStateMachine;
    public EnemyReuseData enemyReuseData;

    //Timer timer;
    public EnemyIdleState(EnemyStateMachine enemyStateMachine)
    {
        this.enemyStateMachine = enemyStateMachine;

        enemyReuseData = enemyStateMachine.enemyReuseData;
    }

    public virtual void Enter(){
        //AnimatorID 数据 输入
        enemyStateMachine.animator.SetFloat(AnimatorID.MoveX,0);
        enemyStateMachine.animator.SetFloat(AnimatorID.MoveY,0);

        //PoolManager.Instance.TakeObj("Prefabs/","Timer").GetComponent<Timer>().CreateTimer(4f,ToPratolState,false,out timer);
    }
    public virtual void Update(){
        EnemyCheckPlayer();

    }
    public virtual void Exit(){
        //取消计时器
        //timer.isCancel = true;

    }

    public void ToPratolState(){
        enemyStateMachine.ChangedState(enemyStateMachine.enemyPartolState);
    }

    public void EnemyCheckPlayer(){
        if(enemyStateMachine.enemyAi.currentTarget != null){
            enemyStateMachine.ChangedState(enemyStateMachine.EnemyAttackAiState);
        }
    }


}