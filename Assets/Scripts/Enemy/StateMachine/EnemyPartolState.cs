using UnityEngine;

public class EnemyPartolState : IState
{
    private EnemyStateMachine enemyStateMachine;
    private EnemyReuseData enemyReuseData;

    Vector3 currentTargetPos;
    Vector3 dir;
    public EnemyPartolState(EnemyStateMachine enemyStateMachine)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.enemyReuseData = enemyStateMachine.enemyReuseData;
    }

    public virtual void Enter(){
        //AnimatorID 数据 输入
        if(enemyReuseData.path == null || enemyReuseData.path.Count<=0 ){
            enemyStateMachine.ChangedState(enemyStateMachine.enemyIdleState);
        }

        currentTargetPos = enemyReuseData.path[enemyReuseData.PathIndex];

        dir = currentTargetPos-enemyStateMachine.enemyTransform.position;


       
    }
    public virtual void Update(){
        Partol();
        EnemyCheckPlayer();
    }
    public virtual void Exit(){


    }

    private void Partol(){
        //检测到没到
        if(Vector3.Distance(enemyStateMachine.enemyTransform.position,currentTargetPos)>0.5f){

            enemyStateMachine.enemyMovement.transform.forward = dir;

            enemyStateMachine.enemyMovement.Move(dir,enemyReuseData.speed);

            enemyStateMachine.animator.SetFloat(AnimatorID.MoveY,1f);
        }
            
        else{

            enemyReuseData.PathIndex += 1;

            enemyStateMachine.animator.SetFloat(AnimatorID.MoveY,0f);
            
            enemyStateMachine.ChangedState(enemyStateMachine.enemyIdleState);
        }   
    }
    
    public void EnemyCheckPlayer(){
        if(enemyStateMachine.enemyAi.currentTarget != null){
            enemyStateMachine.ChangedState(enemyStateMachine.EnemyAttackAiState);
        }
    }

}