using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    protected Animator animator;
    protected CharacterController characterController;
    protected EnemyMovement enemyMovement;
    public EnemyStateMachine enemyStateMachine;

    [Header("检测玩家的相关属性")]
    public float radius;
    public LayerMask checkLayerMask_Player;
    public LayerMask checkLayerMask_Obstacle;
    Collider[] colliders;

    [Header("攻击触发区域")]
    public Transform attackCheckCenterPoint;
    public float attackRadius;
    public  GameObject currentTarget; //攻击目标

    [Header("敌人的数据")]
    public EnemySO enemySO;    
    public SoundData soundData;

    //攻击数据
    public List<AttackContentData> attackComboDatas;
    public AttackContentData currentAttackCombo;
    public AttackData currentAttackData;
    public bool IsCanAttack = true; //攻击的间隔
    private int index = 0;  //在一个combo中的动画索引
    private bool startAttack = false; //是否开始攻击  保证他出了攻击范围也会把连招打完  而不是中断
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
        characterController = GetComponentInParent<CharacterController>();
        enemyStateMachine = new EnemyStateMachine(enemyMovement,this,animator,transform,enemySO.EnemyReuseData);

        //初始化状态
        enemyStateMachine.ChangedState(enemyStateMachine.enemyIdleState);
 
        InitAttackComboData();
    }

    private void Update() {

        if(animator.CheckAnimation_TagIs(0,"Attack")){

            if(!enemyMovement.CheckFrontObstacle())
                enemyMovement.Move(transform.forward,animator.GetFloat(AnimatorID.MoveSpeed));
        }

        EnemyCheckPlayer(); // 一直检测敌人

        //处理敌人的当前状态更新
        enemyStateMachine.Update();
        
       
            
    }



    //检测敌人是否出现
    public void EnemyCheckPlayer(){
        colliders =  Physics.OverlapSphere(transform.position,radius,checkLayerMask_Player);
        if(colliders.Length > 0 ){
            currentTarget = colliders[0].gameObject;   
           
        }
    }
    //获取目标距离
    public float GetTargetDistacne() => Vector3.Distance(currentTarget.transform.position,transform.position);
    //获取目标方向
    public Vector3 GetTargetDirection() => (currentTarget.transform.position - transform.position).normalized;

    //初始化技能攻击CD
    public void InitAttackComboData(){
        if(attackComboDatas.Count>0){
            foreach(AttackContentData item in attackComboDatas){
                if(item.IsCanExcute == false)
                    item.IsCanExcute = true;
            }
        }
    }
    //全部技能进入等待CD
    public void AllInCD(){
        if(attackComboDatas.Count>0){
            foreach(AttackContentData item in attackComboDatas){
                item.IsCanExcute = false;            
            }
        }
    }
    //获取CD好的技能攻击
    public AttackContentData GetCanUseAttackComboData(){
        if(attackComboDatas.Count>0){
            foreach(AttackContentData item in attackComboDatas){
                if(item.IsCanExcute){
                    IsCanAttack = true;  //初始化可以攻击  防止 攻击动画打断 不能进入
                    index = 0;  //重置索引
                    startAttack = false; //进入攻击范围是否开始攻击
                    return item;
                }

            }
        }
        return null;
    }
    //执行攻击播放动画
    
    public void ExecuteAttack(){

        
        //前提条件
        if(startAttack == false &&  GetTargetDistacne()>currentAttackCombo.attackContents[0].attackDistance){

            animator.SetFloat(AnimatorID.Run,1);
            animator.SetFloat(AnimatorID.MoveY,1);
            animator.SetFloat(AnimatorID.MoveX,0);
            enemyMovement.Move(GetTargetDirection(),enemyStateMachine.enemyReuseData.chaseSpeed);

            if(GetTargetDistacne() <= currentAttackCombo.attackContents[0].attackDistance){
                startAttack = true;
                animator.SetFloat(AnimatorID.Run,0);
                animator.SetFloat(AnimatorID.MoveY,0);
                animator.SetFloat(AnimatorID.MoveX,0);
            }
        }
        else  //  一开始就满足攻击距离
        {
            startAttack = true;
        }

        if(startAttack  && currentAttackCombo.IsCanExcute) {

           
            if(index >= currentAttackCombo.attackContents.Count){ //超过这个索引时 证明 连招完毕

                currentAttackCombo.IsCanExcute = false;
                currentAttackCombo.ResetCD();
                currentAttackCombo = null;
                return;
            }


            if(IsCanAttack){
                currentAttackData = currentAttackCombo.attackContents[index];
                currentAttackData.attacker = this.gameObject;
                animator.CrossFadeInFixedTime(currentAttackData.attackName,0.4f);
                IsCanAttack = false;
                index ++;

            }
  
        } 
       
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackCheckCenterPoint.position,attackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackCheckCenterPoint.position,radius);
    }

    #region 动画事件相关的方法

        //触发攻击动画的事件     攻击·动画事件
        public void ATK(){

            if(currentAttackData != null)
                currentAttackData.attacker = this.gameObject;

            Collider[] colliders = Physics.OverlapSphere(attackCheckCenterPoint.position,attackRadius,checkLayerMask_Player);
            if(colliders.Length > 0){     
                //事件传递伤害
                EventCenter.Instance.EventTrigger<AttackData>(AllEventName.EnemyTiggerAttack,currentAttackData);                
            }
        }
        public void StopCurrentAttackData(){

            if(currentAttackCombo != null){

                currentAttackCombo.IsCanExcute = false;
                currentAttackCombo.ResetCD();
                currentAttackCombo = null;
            }
            currentAttackData = null;
        }
        public void CancelAttackColdTime(){
            IsCanAttack = true;
        }
        public void EnablePreInput(){}

        public void FootSound(){
            MusicManager.Instance.PlaySound(soundData.GetSound(Game_Enum.PlayerSoundType.Foot).RandamClip());
        }
        public void AttackSound(){
            MusicManager.Instance.PlaySound(soundData.GetSound(Game_Enum.PlayerSoundType.Attack).RandamClip());
        }
        public void HitSound(){
            MusicManager.Instance.PlaySound(soundData.GetSound(Game_Enum.PlayerSoundType.Hit).RandamClip());
        }
        public void ParrySound(){
            MusicManager.Instance.PlaySound(soundData.GetSound(Game_Enum.PlayerSoundType.Parry).RandamClip());
        }
        public void PlayEffect(){
            
        }
    #endregion
}
