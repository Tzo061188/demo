using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MouseState{
    Ground,
    Dialogue,
    Attack,
}

public class MouseOnClick : MonoBehaviour
{
    public GameObject moveEffect;
    public Vector3 currentMovePos;
    public MouseState mouseState;

    private Ray mouseRay;

    void Update()
    {
        
        MouseIcon();  
    }

    //实时检测鼠标射线 变化图标 并且选着状态
    private void MouseIcon()
    {
        mouseRay =  Camera.main.ScreenPointToRay(Input.mousePosition);
        MathfTool.RayCast(mouseRay,(hitInfo)=>{

            switch (hitInfo.collider.gameObject.tag)
            {   
                case "Ground":
                    //切换鼠标图标
                    //进行移动点值获取 和 生成特效
                    if(Input.GetMouseButtonDown(1))
                        RayCastEffectGenerate(); 
                break;
                case "Npc":
                    //切换图标
                    //打开对话面板
                    if(Input.GetMouseButtonDown(1))
                    {
                        GameManager.Instance.NPC = hitInfo.collider.gameObject;
                        
                        if(UIManager.Instance.GetPanel<DialoguePanel>() != null){
                            UIManager.Instance.GetPanel<DialoguePanel>().ShowPanel();                        
                        }else
                            UIManager.Instance.ShowPanel<DialoguePanel>("Prefabs/UI/DIaloguePanel");
                    }
                break;
                case "Enemy":
                    //切换图标
                    //执行攻击
                    // if(Input.GetMouseButtonDown(1))
                    //     RayCastEffectGenerate(); 
                break;

                
            }
           

        },1000,1<<6|1<<7|1<<8);
    }

    //鼠标点击 生成移动特效 和 记录点击位置
    private void RayCastEffectGenerate(){

        //如果点击到UI
        if( EventSystem.current.IsPointerOverGameObject()){

            PointerEventData eventData =new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData,raycastResults);
            foreach(RaycastResult result in raycastResults)
            {
                print(result.gameObject.name);
            }
            return;
        }

        
        MathfTool.RayCast(mouseRay,(hitInfo)=>{
            Vector3 pos = new Vector3(hitInfo.point.x,hitInfo.point.y+0.3f,hitInfo.point.z);
            Instantiate(moveEffect,pos,Quaternion.identity);
            currentMovePos = hitInfo.point;

        },1000,LayerMask.GetMask("Ground"));
    }
}
