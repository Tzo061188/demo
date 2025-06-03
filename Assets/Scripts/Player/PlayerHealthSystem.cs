using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{

    public int currentHp;
    public int maxHp;
    public float tiredCount = 1;
    public float maxTiredCount = 5;
    public float reduceTiredCount_Speed;
    private bool isTakeDamage;
    private float time;
    private Animator animator;
    private Player player;

    void Awake()
    {
        animator = transform.Find("安比").GetComponent<Animator>();
        player = GetComponent<Player>();
        currentHp = maxHp;
    }


    void Update()
    {
        if(isTakeDamage){
            time += Time.deltaTime;
            
        }
        if(time > 2.5f ){
            isTakeDamage = false; 
            tiredCount -= reduceTiredCount_Speed * Time.deltaTime;
            tiredCount = Mathf.Clamp(tiredCount,0,maxTiredCount);
        }

    }
    private void OnEnable() {
        EventCenter.Instance.AddEventListener<AttackData>(AllEventName.EnemyTiggerAttack,TiggerAttack);
        
    }

    private void OnDisable() {
        EventCenter.Instance.RemoveEventListener<AttackData>(AllEventName.EnemyTiggerAttack,TiggerAttack);
       
    }

    //触发敌人的伤害
    public void TiggerAttack(AttackData data){

        if(animator.CheckAnimation_TagIs(0,"Skill") || animator.CheckAnimation_TagIs(0,"Sprint"))
            return;


        if(IsDie()) return;

        //被击打看人 

            Vector3 v1;
            if(data.attacker == null){
                if (player.currentAttackEnemy == null)
                    player.currentAttackEnemy = GameManager.Instance.currentAttackEnemy;
                v1 = GameManager.Instance.currentAttackEnemy.transform.position;

            }else{

                if (player.currentAttackEnemy == null)
                    player.currentAttackEnemy = data.attacker;
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
        time = 0;

        if(animator.CheckAnimation_TagIs(0,"Parry")){

            if(tiredCount >= maxTiredCount) {
                animator.CrossFadeInFixedTime("Parry_Tired",0.1f);
                tiredCount = 0;
                return;
            }
            
            animator.CrossFadeInFixedTime(data.parryName,0.1f);
            //累计疲劳值
            tiredCount += data.parryExecuteCount;
            tiredCount = Mathf.Clamp(tiredCount,0,maxTiredCount);



        }else{
            Hit(data);
        }

        if(IsDie()){
            animator.CrossFadeInFixedTime("Die",0.1f);
            //TODO：通知ui manager 
        }
    }

    private void Hit(AttackData data)
    {
        animator.CrossFadeInFixedTime(data.hitName, 0.1f);
        //累计疲劳值
        tiredCount += data.parryExecuteCount * 1.5f;
        tiredCount = Mathf.Clamp(tiredCount,0,maxTiredCount);

        print("受伤" + data.attackDamage);
        currentHp -= data.attackDamage;
    }

    public bool IsDie(){
        if(currentHp <= 0) return true;
        return false;
    }


    public float GetTiredScale(){
        return tiredCount/maxTiredCount;
    }

    public float GetHPScale(){

        return (float)currentHp/(float)maxHp;
    }

}
