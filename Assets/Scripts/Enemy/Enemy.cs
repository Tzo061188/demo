using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : RoleBase
{

    public EnemyStateMachine enemyStateMachine;

    public string currentState;
    public int enemyID;
    public EnemySO enemySO;
    private EnemyReuseData enemyReuseData;

    public Vector3 checkEnemyOffest;
    public LayerMask checkLayerMask;
    public float checkEnemyRadius;
    protected override void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        //初始化
        if(enemyReuseData == null){

            enemyReuseData =  enemySO.EnemyReuseData;      
        }
            

        enemyStateMachine.ChangedState(enemyStateMachine.enemyIdleState);
    }

    protected override void Update()
    {
        base.Update();
        //执行当前状态的方法
        enemyStateMachine.Update();
        enemyStateMachine.HandInput();


        if(animator.CheckAnimation_TagIs(0,"Attack"))
            Move(transform.forward,animator.GetFloat(AnimatorID.MoveSpeed));
    }

    private void OnEnable() {
        EventCenter.Instance.AddEventListener<AttackData>(AllEventName.TiggerAttack,TiggerAttack);
        enemyStateMachine.currentState.OnValueChange += GetState;
    }
    private void OnDisable() {
        EventCenter.Instance.RemoveEventListener<AttackData>(AllEventName.TiggerAttack,TiggerAttack);
        enemyStateMachine.currentState.OnValueChange -= GetState;
    }

    public void GetState(IState state){
        currentState = state.GetType().Name;
    }

    //响应玩家攻击
    private void TiggerAttack(AttackData data){
        print("收到伤害:"+data.attackDamage);
        animator.CrossFadeInFixedTime(data.hitName,0.1f);
        Move(transform.forward * -1 , enemyReuseData.chaseSpeed);

    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position+ checkEnemyOffest,checkEnemyRadius);
    }

    //轻攻击的检测
    public void CheckEnemy(){
        
        Collider[] colliders = Physics.OverlapSphere(transform.position+ checkEnemyOffest,checkEnemyRadius,checkLayerMask);
        if(colliders.Length > 0){
            for (int i = 0; i < colliders.Length; i++)
            {
        
                //事件传递伤害
                
            }
        }
    }




  
}
