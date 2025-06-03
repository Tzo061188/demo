using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RoleMove : MonoBehaviour
{
    
    private Animator animator;
    private CharacterController characterController;

    [SerializeField]
    private CameraView cameraView;

    //X轴横向移动量
    private float moveX;
    //Y轴纵向移动量
    private float moveZ;
    //移动方向
    private Vector3 direction ;
    //鼠标X旋转量
    private Quaternion rotateValue;
    //移动速度
    public float speed = 0;
    //摄像机看向和跟随的目标
    public Transform lookTarget;

    public AnimatorClipInfo[] animatorClipInfo;
    private bool isAttack;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponentInChildren<CharacterController>();
        animatorClipInfo = animator.GetNextAnimatorClipInfo(0);
        cameraView.InItTarget(lookTarget);
    }

    void Update()
    {
       
       // Move();
        //Animation();
        Button();

    }


    //移动
    private void Move(){
        moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //方向
        direction =  new Vector3(moveX,0,moveZ).normalized;

        //改变人物朝向
        if(direction != Vector3.zero){          
           transform.forward = direction;
        }
        
        //得到摄像头左右旋转量
        rotateValue = Quaternion.Euler(0,cameraView.LeftAndRightRotate,0);
        //上面的四方移动改变朝向旋转 在叠加上 摄像机的朝向旋转
        if(direction != Vector3.zero)
            transform.rotation *= rotateValue;
        //移动的位置
        Vector3 pos = new Vector3(transform.position.x+moveX,0,transform.position.z+moveZ);
     
        
        characterController.Move(rotateValue*pos);
    }
    //动画设置
    private void Animation(){
        animator.SetFloat("Speed",speed);

    }
    //按键
    private void Button(){

        if(Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") != 0)
            speed = 1;
        else
            speed = 0;

        if(Input.GetKey(KeyCode.LeftShift) && direction != Vector3.zero){
            speed = 6;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift)){
            speed = 1;
        }
        if(Input.GetMouseButtonDown(0)){
            animator.SetTrigger("Attack");
        }
    }
   
  
}
