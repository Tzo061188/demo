using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : RoleBase
{

    [SerializeField,Header("当前状态")]
    private string currentMovementState;
    [SerializeField] private string currentAttackState;

    public bool IsLockLook; //锁定目标

    [Header("当前的攻击对象")]
    public GameObject currentAttackEnemy;

    public PlayerMovementStateMachine playerMovementStateMachine;
    public PlayerAttackStateMachine playerAttackStateMachine;
    
    public Transform CameraTrans;
    private Vector3 playerMovementDirection;


    [SerializeField,Header("相机与看向目标")] 
    public CustomCamrea customCamrea;
    public Transform target;

    //角色移动速度输入
    [HideInInspector]public float speed = 5.5f;
    [HideInInspector]public float sprint;


    [Header("敌人检测")]
    public Vector3 Skill01Box; //普通技能的攻击检测
    public Transform SkillCenterPoint;//普通技能的攻击检测

    public Vector3 FinishSkill01Box; //大招技能的攻击检测
    public Transform FinishSkillCenterPoint;//大招技能的攻击检测

    public Transform attackCheckCenterPoint;
    public float checkEnemyRadius;
    public float checkEnemyAreaRadius;
    public LayerMask EnemyLayer;
    

    [Header("角色SO配置数据")]
    public PlayerSO playerSO;

    [Header("角色声音的SO配置数据")]
    public SoundData playerSoundDataSO;

    [Header("当前的攻击数据")]
    public AttackData currentAttackData;

    public List<Transform> EffectPos;
    private List<ParticleSystem> Effects = new List<ParticleSystem>();

    protected override void Awake()
    {

        base.Awake();
        InitPlayer();
    }

    public  void InitPlayer()
    {
        CameraTrans = Camera.main.transform;
        if (customCamrea == null)
            this.customCamrea = GameManager.Instance.customCamrea;

        customCamrea.InitTarget(target);


        animator = gameObject.transform.Find("安比").GetComponent<Animator>();
        //初始化状态机
        playerAttackStateMachine = new PlayerAttackStateMachine(this, animator);
        playerMovementStateMachine = new PlayerMovementStateMachine(this, animator);

        
    }

    private void Start() {
        playerMovementStateMachine.ChangedState(playerMovementStateMachine.idleState);
        playerAttackStateMachine.ChangedState(playerAttackStateMachine.nullState);
    }

    private void OnEnable() {
        //添加了 当状态改变 执行打印状态名称 以及显现在inspect上
        playerMovementStateMachine.currentState.OnValueChange +=ChangeMovementState;
        playerAttackStateMachine.currentState.OnValueChange +=ChangeAttackState;
        
    }


    private void OnDisable() {
        playerMovementStateMachine.currentState.OnValueChange -=ChangeMovementState;
        playerAttackStateMachine.currentState.OnValueChange -=ChangeAttackState; 
    }

    private void OnDestroy()
    {
        playerAttackStateMachine.normalAttackState.Exit();
        playerAttackStateMachine.nullState.Exit();
        playerAttackStateMachine.parryState.Exit();
        playerAttackStateMachine.finishSkillState.Exit();
        playerAttackStateMachine.normalskillState.Exit();

        playerMovementStateMachine.idleState.Exit();
        playerMovementStateMachine.runState.Exit();
        playerMovementStateMachine.walkState.Exit();
        playerMovementStateMachine.sprintState.Exit();
        playerMovementStateMachine.playerNullMovementState.Exit();

        RoleInputSystem.Instance.Disable();
        
    }
    protected override void Update()
    {

        if(GameManager.Instance.isPlayerCanMove == false)
            return;

        base.Update();
        playerMovementStateMachine.Update();
        playerMovementStateMachine.HandInput();
        playerAttackStateMachine.Update();

        //检测区域内敌人
        CheckEnemyInArea();
        CheckCurrentEnemyExitArea();

        PlayerAnimationMove();
        //特效返回对象池
        if(Effects.Count > 0 ){
            for(int i = Effects.Count-1 ; i >= 0  ; i--){
                if(Effects[i].isStopped){
                    PoolManager.Instance.PutObj(Effects[i].gameObject);
                    Effects.Remove(Effects[i]);
                }
            }
        }

    }

    private void PlayerAnimationMove()
    {
        if(GameManager.Instance.isPlayerCanMove == false)
            return;

        //处理各种状态 的移动
        if (animator.CheckAnimation_TagIs(0, "Sprint"))
        {
            if (!CheckFrontObstacle()) //没有障碍时
                Move(transform.forward, animator.GetFloat(AnimatorID.MoveSpeed));
            return;
        }
        //移动
        if (animator.CheckAnimation_TagIs(0, "Movement") )
        {
            if(IsLockLook == false){
                //无锁定    
                Move(transform.forward, speed);

            }else{
                //锁定  
                LockLookMovement();
                Move(playerMovementDirection, speed);
            }
            return;
        }
        
        if(animator.CheckAnimation_TagIs(0,"Attack")){
             //攻击时向前移动  使用在动画中添加的曲线值 进行移动 
            if (!CheckFrontObstacle()) //没有障碍时
                Move(transform.forward, animator.GetFloat(AnimatorID.MoveSpeed));
            return;
        }


        Move(transform.forward,animator.GetFloat(AnimatorID.MoveSpeed));
    }

    //锁定
    public void LockLookMovement(){
        animator.SetFloat(AnimatorID.MoveX,RoleInputSystem.Instance.Move.x);
        animator.SetFloat(AnimatorID.MoveY,RoleInputSystem.Instance.Move.y);
        animator.SetFloat(AnimatorID.LookOn,1);
        if(IsLockLook) playerMovementDirection = transform.forward * RoleInputSystem.Instance.Move.y 
                                                    +transform.right * RoleInputSystem.Instance.Move.x;

    }


    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheckCenterPoint.position,checkEnemyRadius);
        Gizmos.color = Color.blue;
         Gizmos.DrawWireSphere(transform.position,checkEnemyAreaRadius);
        Gizmos.color = Color.green;
        Vector3 point = new Vector3(groundCheckPoint.position.x,
                                    groundCheckPoint.position.y + pointOffset,
                                    groundCheckPoint.position.z);
         Gizmos.DrawWireSphere(point,checkRadius);


    }

    public void AnimationChangeEvent(Game_Enum.AnimationStateName animationStateName){
        switch (animationStateName)
        {

            case Game_Enum.AnimationStateName.Attack:
                playerMovementStateMachine.AnimationChangeEvent(playerMovementStateMachine.playerNullMovementState);
                playerAttackStateMachine.AnimationChangeEvent(playerAttackStateMachine.normalAttackState);
                break;
            case Game_Enum.AnimationStateName.Sprint:
                playerAttackStateMachine.AnimationChangeEvent(playerAttackStateMachine.nullState);
                playerMovementStateMachine.AnimationChangeEvent(playerMovementStateMachine.sprintState);
                break;
            

        }
    }

    public void AnimationExitEvent(){
        playerAttackStateMachine.AnimationExitEvent();
        playerMovementStateMachine.AnimationExitEvent();
    }


    private void ChangeAttackState(IState state)
    {
        currentAttackState = state.GetType().Name;

    }

    private void ChangeMovementState(IState state)
    {
        currentMovementState = state.GetType().Name;

    }


     #region 动画事件
        public void ClearCurrentAttackData(){}
        public void EnablePreInput(){
            playerAttackStateMachine.playerAttack.EnablePreInput();
        }

        public void CancelAttackColdTime(){
            playerAttackStateMachine.playerAttack.CancelAttackColdTime();
        }
        public void CancelSprintColdTime(){
            playerMovementStateMachine.sprintState.CancelSprintColdTime();
        }
        //攻击检测触发
        public void ATK(){
            CheckEnemy();
        }
        public void NormalSkill_02_ATK(){
            
            Collider[] colliders =  Physics.OverlapBox(SkillCenterPoint.position,Skill01Box
                                                        ,this.transform.rotation,EnemyLayer);
    
            if(colliders.Length > 0){
                for (int i = 0; i < colliders.Length; i++)
                {

                    if(i == 0 && currentAttackEnemy == null){
                        currentAttackEnemy = colliders[i].gameObject;
                    }
                    //事件传递伤害
                    currentAttackData = playerSO.attackModuleData.normalSkillAttack.attackContents[0];

                    currentAttackData.attacker = this.gameObject;

                    EventCenter.Instance.EventTrigger<AttackData>(AllEventName.TiggerAttack,currentAttackData);
                }
            }
        }
        public void FinishSkill_ATK(){

            Collider[] colliders =  Physics.OverlapBox(FinishSkillCenterPoint.position,FinishSkill01Box
                                                        ,transform.rotation,EnemyLayer);
            if(colliders.Length > 0){
                for (int i = 0; i < colliders.Length; i++)
                {
                    if(i == 0 && currentAttackEnemy == null){
                        currentAttackEnemy = colliders[i].gameObject;
                    }
                    //事件传递伤害
                    currentAttackData = playerSO.attackModuleData.finalSkillAttack.attackContents[0];

                    currentAttackData.attacker = this.gameObject;

                    EventCenter.Instance.EventTrigger<AttackData>(AllEventName.TiggerAttack,currentAttackData);
                }
            }
        }
        // 轻攻击特效
        public void PlayEffect(int index){
            GameObject effect = null;
            if(index != 4)
                effect =  PoolManager.Instance.TakeObj("Effect/","Sword Slash 11");
            else
                effect =  PoolManager.Instance.TakeObj("Effect/","Prick 3");
            effect.transform.position = EffectPos[index].position;
            effect.transform.rotation = EffectPos[index].rotation;
            effect.transform.localScale = EffectPos[index].localScale;
            Effects.Add( effect.GetComponent<ParticleSystem>());

        }
        // 技能特效
        public void PlaySkillEffect(int index){

            if(index == 6){
                GameObject effect =  PoolManager.Instance.TakeObj("Effect/","Sword Slash 8");
                effect.transform.position = EffectPos[index].position;
                effect.transform.rotation = EffectPos[index].rotation;
                effect.transform.localScale = EffectPos[index].localScale;
                Effects.Add( effect.GetComponent<ParticleSystem>());

            }else if(index == 7){

                GameObject effect =  PoolManager.Instance.TakeObj("Effect/","Sword Slash 6");
                effect.transform.position = EffectPos[index].position;
                effect.transform.rotation = EffectPos[index].rotation;
                effect.transform.localScale = EffectPos[index].localScale;
                Effects.Add( effect.GetComponent<ParticleSystem>());
            }
            


        }
        //以下关于声音的
        public void FootSound(){
            MusicManager.Instance.PlaySound(playerSoundDataSO.GetSound(Game_Enum.PlayerSoundType.Foot).RandamClip());
        }
        public void AttackSound(){
            MusicManager.Instance.PlaySound(playerSoundDataSO.GetSound(Game_Enum.PlayerSoundType.Attack).RandamClip());
        }
        public void PlayerAttackVoiceSound(){
            MusicManager.Instance.PlaySound(playerSoundDataSO.GetSound(Game_Enum.PlayerSoundType.PlayerAttackVoice).RandamClip());
        }
        public void PlayerSprintSound(){
            MusicManager.Instance.PlaySound(playerSoundDataSO.GetSound(Game_Enum.PlayerSoundType.Sprint).RandamClip());
        }

        public void HitSound(){
            MusicManager.Instance.PlaySound(playerSoundDataSO.GetSound(Game_Enum.PlayerSoundType.Hit).RandamClip());
        }

        public void ParrySound(){
            MusicManager.Instance.PlaySound(playerSoundDataSO.GetSound(Game_Enum.PlayerSoundType.Parry).RandamClip());
        }
    #endregion 

    #region 攻击检测
        protected void CheckEnemyInArea(){
            if(currentAttackEnemy == null)
                return;

            Collider[] colliders = Physics.OverlapSphere(transform.position,checkEnemyAreaRadius,EnemyLayer);
            if(colliders.Length > 0){ 
                currentAttackEnemy = colliders[0].gameObject;

            }
        }
        protected void CheckCurrentEnemyExitArea(){
            if(currentAttackEnemy!=null){
                if(Vector3.Distance(currentAttackEnemy.transform.position,transform.position)>checkEnemyAreaRadius){

                    currentAttackEnemy = null;
                    GameManager.Instance.currentAttackEnemy = currentAttackEnemy;

                }
            }
        }
        public void CheckEnemy(){
            
            Collider[] colliders = Physics.OverlapSphere(attackCheckCenterPoint.position,checkEnemyRadius,EnemyLayer);
            if(colliders.Length > 0){
                for (int i = 0; i < colliders.Length; i++)
                {
                    if(i == 0 && currentAttackEnemy == null){
                        currentAttackEnemy = colliders[i].gameObject;
                        GameManager.Instance.currentAttackEnemy = currentAttackEnemy;
                    }
                    //事件传递伤害
                    currentAttackData = playerAttackStateMachine.playerAttack.playerAttributeData.currentAttackMode.attackContents
                                        [playerAttackStateMachine.playerAttack.playerAttributeData.currentAttackIndex];

                    currentAttackData.attacker = this.gameObject;

                    EventCenter.Instance.EventTrigger<AttackData>(AllEventName.TiggerAttack,currentAttackData);
                }
            }
        }

     #endregion

    // public void PlayerRotation(){
    //     if(RoleInputSystem.Instance.Move != Vector2.zero){
    //     print("X"+RoleInputSystem.Instance.Move.x +"-------   Y"+RoleInputSystem.Instance.Move.y);
    //     print("角度值"+Mathf.Atan2(RoleInputSystem.Instance.Move.y,RoleInputSystem.Instance.Move.x)*Mathf.Rad2Deg);
    //     print("________________");
    //     print("角度值"+Mathf.Atan2(RoleInputSystem.Instance.Move.x,RoleInputSystem.Instance.Move.y)*Mathf.Rad2Deg);


    //     float angle =  Mathf.Atan2(RoleInputSystem.Instance.Move.x,RoleInputSystem.Instance.Move.y)*Mathf.Rad2Deg + CameraTrans.eulerAngles.y;
    //     Quaternion rotate = Quaternion.Euler(0,angle,0);
    //     transform.rotation = Quaternion.Lerp(transform.rotation,rotate,0.1f);
    //     }
    // }
}
