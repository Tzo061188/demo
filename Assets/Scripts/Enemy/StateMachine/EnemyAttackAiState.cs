using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyAttackAiState : IState
{
    public EnemyStateMachine enemyStateMachine;
    public EnemyReuseData enemyReuseData;

    public float distance;//敌人和玩家之间的距离
  

    int moveDir; //控制左右移动和站立
    float timer; //左右移动的改变时间

    bool isChase;

    public EnemyAttackAiState(EnemyStateMachine enemyStateMachine)
    {
        this.enemyStateMachine = enemyStateMachine;
        enemyReuseData = enemyStateMachine.enemyReuseData;
    }
    
    public virtual void Enter(){

        //初始化攻击数据
        enemyStateMachine.enemyAi.InitAttackComboData();

    }       
    public virtual void Update()
    {
        if(enemyStateMachine.animator.CheckAnimation_TagIs(0,"Attack")) return;
        if(enemyStateMachine.animator.CheckAnimation_TagIs(0,"Parry")) return;
        if(enemyStateMachine.animator.CheckAnimation_TagIs(0,"Hit")) return;

        GetRandomValue();//获取左右横移的随机值

        //敌人和玩家之间的距离 
        if( enemyStateMachine.enemyAi.currentTarget != null) 
            distance = enemyStateMachine.enemyAi.GetTargetDistacne();


        if(enemyStateMachine.enemyAi.currentAttackCombo == null  ){
            
            ChaseAndStandoff();
            enemyStateMachine.enemyAi.currentAttackCombo =  enemyStateMachine.enemyAi.GetCanUseAttackComboData();

        }
        else{
            
            enemyStateMachine.enemyAi.ExecuteAttack();

        }

        if(enemyStateMachine.enemyAi.currentTarget != null)
        enemyStateMachine.enemyTransform.LookAt(enemyStateMachine.enemyAi.currentTarget.transform.position);

        if(distance > enemyReuseData.detachDistance){ //脱战距离

            //脱战  当前目标为null  攻击数据为null  所有技能进入等待冷却
                enemyStateMachine.enemyAi.currentTarget = null;
                enemyStateMachine.enemyAi.currentAttackCombo = null;
              

            enemyStateMachine.ChangedState(enemyStateMachine.enemyIdleState);
            return;
        }

    }

    //追击和僵持
    public void ChaseAndStandoff(){

        

        if(isChase){
            enemyStateMachine.animator.SetFloat(AnimatorID.Run,1f,0.2f,Time.deltaTime);
            enemyStateMachine.animator.SetFloat(AnimatorID.MoveY,1f,0.2f,Time.deltaTime);
            enemyStateMachine.animator.SetFloat(AnimatorID.MoveX,0f,0.2f,Time.deltaTime);
            enemyStateMachine.enemyMovement.Move(enemyStateMachine.enemyTransform.forward,enemyReuseData.chaseSpeed);
            if(distance <= enemyReuseData.keepMinDistacne) //保持距离 要小于僵持距离  保证在僵持时动一下不出僵持距离
                isChase = false;
            return;
        }

        if (distance < enemyReuseData.standoffDistacne){  // 小于僵持距离 5进行僵持

            enemyStateMachine.animator.SetFloat(AnimatorID.Run,0f,0.2f,Time.deltaTime);
        
            //进行僵持           
            enemyStateMachine.animator.SetFloat(AnimatorID.MoveY,0f,0.2f,Time.deltaTime); 
            enemyStateMachine.animator.SetFloat(AnimatorID.MoveX,moveDir,0.2f,Time.deltaTime);
            enemyStateMachine.enemyMovement.Move(enemyStateMachine.enemyTransform.right * moveDir ,enemyReuseData.speed);
           
            
        }else if(distance > enemyReuseData.chaseMaxDistance){  // 当他超过了 僵持距离 并且超过了追击距离 开始追击
            isChase = true;
            //向前追击    
        }
   
    }



    //获取左右横移的随机值
    public void GetRandomValue(){
        if (timer < 4f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0; 
            moveDir =  Random.Range(-1,2);
        }
    }

    public virtual void AnimationChangeEvent(IState state){
        enemyStateMachine.ChangedState(state);
    }
}
