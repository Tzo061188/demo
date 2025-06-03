using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private Player player;

    private void Start() {
        player = GetComponentInParent<Player>();
    }

    #region 动画事件
        public void ClearCurrentAttackData(){}
        public void EnablePreInput(){
            player.EnablePreInput();
        }

        public void CancelAttackColdTime(){
            player.CancelAttackColdTime();
        }
        public void CancelSprintColdTime(){
            player.CancelAttackColdTime();
        }
        //攻击检测触发
        public void ATK(){
            player.CheckEnemy();
        }
        public void NormalSkill_02_ATK(){
            
            player.NormalSkill_02_ATK();
        }
        public void FinishSkill_ATK(){

            player.FinishSkill_ATK();
        }
        // 轻攻击特效
        public void PlayEffect(int index){
           player.PlayEffect(index);
        }
        // 技能特效
        public void PlaySkillEffect(int index){
            player.PlaySkillEffect(index);
        }
        //以下关于声音的
        public void FootSound(){
            player.FootSound();
        }
        public void AttackSound(){
            player.AttackSound();
        }
        public void PlayerAttackVoiceSound(){
            player.PlayerAttackVoiceSound();
        }
        public void PlayerSprintSound(){
            player.PlayerSprintSound();
        }

        public void HitSound(){
            player.HitSound();
        }

        public void ParrySound(){
            player.ParrySound();
        }
    #endregion 


}
