using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealthSystem : RoleHealthBase
{
    [Header("防御相关的属性")]
    public int currentParryCount; 
    public int maxParryCount; // 超过最大防御时 开始受伤计数

    public int currentParryAttackCount; //当前积累的防御反击数值
    public int maxParryAttackCount; //当前防御反击   超过  最大防御反击时执行


    public int hitCount;
    public int maxHitCount; //受伤计数 低于最大 计数时 可以受伤   -- 超过了  无敌  或者 技能
    public bool isTakeDamage;//是否受到伤害
    public float maxAttackIntervalTime; //超过最大攻击间隔时间  重置 当前防御次数
    private float currentCDTime;
    private EnemyAi enemyAi;

    // 敌人血量UI的变量
    private Transform canvas_Enemy;
    private Image Hp;
    private RectTransform tired;
    private Vector3 tired_scale =  new Vector3();
    protected override void Awake() {
        base.Awake();

        enemyAi = GetComponentInChildren<EnemyAi>();
        canvas_Enemy = transform.Find("Canvas_Enemy");
        Hp = transform.Find("Canvas_Enemy/HP").GetComponent<Image>();
        tired = transform.Find("Canvas_Enemy/Parry").GetComponent<RectTransform>();
    }
    private void Update() {

        if(isTakeDamage && currentCDTime > maxAttackIntervalTime){ //超过了攻击间隔时间  重置一些数据
            isTakeDamage = false;
            currentParryCount = 0 ;
        }
        if(!isTakeDamage && currentExecuteCount>0){ //超过了攻击间隔时间  缓慢减少处决值 
            currentExecuteCount -= speed_ExecuteCountReduce * Time.deltaTime;
        }
        if(isTakeDamage){
            currentCDTime += Time.deltaTime;
        }

        if(enemyAi.currentTarget != null)
            canvas_Enemy.forward = (enemyAi.currentTarget.transform.position - transform.position).normalized;

        currentHp = Mathf.Clamp(currentHp,0,maxHp);
        Hp.fillAmount = (float)currentHp/(float)maxHp;


        currentExecuteCount = Mathf.Clamp(currentExecuteCount,0,maxExecuteCount);
        tired_scale.Set((currentExecuteCount/maxExecuteCount),1f,1f);
        tired.localScale = tired_scale;


        

    }


    private void OnEnable() {
        EventCenter.Instance.AddEventListener<AttackData>(AllEventName.TiggerAttack,TiggerAttack);
    }
    private void OnDisable() {
        EventCenter.Instance.RemoveEventListener<AttackData>(AllEventName.TiggerAttack,TiggerAttack);
    }


    //触发玩家造成的攻击
    public void TiggerAttack(AttackData data){

        print("是否死亡"+IsDie());
        if(IsDie()) return;

        //打断重置当前的攻击数据 让他重新获取去
        enemyAi.StopCurrentAttackData();

        //transform.LookAt(new Vector3(data.attacker.transform.position.x,transform.position.y,data.attacker.transform.position.z));
         Vector3 v1;
            if(data.attacker == null){
                v1 = GameManager.Instance.player.transform.position;

            }else{
                
                v1 =  data.attacker.transform.position;

            }

            if( v1 != null ){
                v1.y = 0;
                Vector3 v2 = transform.position;
                v2.y = 0;

                v1 = (v1 - v2).normalized;
                transform.forward = v1;
            }

        isTakeDamage = true;
        currentCDTime = 0;

        if(data.attackStatel == Game_Enum.AttackState.Skill){


            animator.CrossFadeInFixedTime(data.hitName,0.1f,0);
            currentHp -= data.attackDamage;
            currentExecuteCount += data.attackDamage; //累计处决值

            CheckDie();
            return;
        }




        if(currentParryCount <= maxParryCount){ //进行格挡

            currentParryCount ++;
            currentExecuteCount += data.parryExecuteCount; //累计处决值
            
            animator.CrossFadeInFixedTime(data.parryName,0.1f);
            

            
        }else{ 

            if(hitCount < maxHitCount){//进行受伤
                animator.CrossFadeInFixedTime(data.hitName,0.1f);
                hitCount ++;

                currentHp -= data.attackDamage;
                currentExecuteCount += data.attackDamage; //累计处决值

                CheckDie();
            }else
            {
                ResetAllCount();
            }


        }

        
    }

    private void CheckDie(){
        if(IsDie()){

            animator.CrossFadeInFixedTime("Die",0.1f);
            enemyAi.enemyStateMachine.ChangedState(enemyAi.enemyStateMachine.enemyNullState);

            //TODO：怪物死亡事件触发  传入敌人的id
            EventCenter.Instance.EventTrigger(AllEventName.EnemyDie,0);
            if(SceneManager.GetActiveScene().name == "AttackScene")
                GameObject.FindGameObjectWithTag("Door").transform.Find("Teleport_12").gameObject.SetActive(true);
        }
    }

    private void ResetAllCount()
    {
        hitCount = 0;
        currentParryCount = 0;
    }
    private void ResetExecute()
    {
        currentExecuteCount = 0;
        Time.timeScale = 1;
    }
}
