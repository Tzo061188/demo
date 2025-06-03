using System;
using System.Collections;
using System.Collections.Generic;



using UnityEngine;


public class RoleBase : MonoBehaviour
{
    public Animator animator;
    protected CharacterController characterController;

    //重力相关
    [SerializeField,Header("重力相关")] private float gravity = 9.8f;
    [SerializeField] private float currentGravitySpeed;
    [SerializeField] private float maxGravitySpeed = -20f;
    [SerializeField] private float minGravitySpeed = 0f;
    [SerializeField] Vector3 fall = new Vector3();
    //地面检测相关
    [SerializeField,Header("地面检测相关")] private bool isGround;
    [SerializeField] protected Transform groundCheckPoint;
    [SerializeField] protected float pointOffset;
    [SerializeField] protected float checkRadius;
    [SerializeField] LayerMask groundlayer;

    public LayerMask Obstaclelayer;
    protected virtual void Awake() {

        characterController = GetComponent<CharacterController>();
    }
    protected virtual void Update() {
        GravitySpeed();
        CheckGround();
    }
    public void Move(Vector3 Direction,float speed){
        
        //坡面检测
        Direction = SlopeCheck(Direction).normalized;
        //下落速度
        fall.Set(0,currentGravitySpeed,0);

        characterController.Move(Direction * speed * Time.deltaTime);
        
    }

    //计算重力速度
    private void GravitySpeed()
    {
        if(isGround){
            //在地面
            currentGravitySpeed = minGravitySpeed;
        }else{
            //不在地面上
            
            currentGravitySpeed -= Time.deltaTime * gravity;
            
            //实时更新重力
            
            fall.Set(0,currentGravitySpeed,0);

            characterController.Move(fall);
        }
    }


    //检测地面
    private void CheckGround(){
        Vector3 point = new Vector3(groundCheckPoint.position.x,
                                    groundCheckPoint.position.y + pointOffset,
                                    groundCheckPoint.position.z);
        isGround =  Physics.CheckSphere(point,checkRadius,groundlayer);
    }
    //坡面检测
    private Vector3 SlopeCheck(Vector3 direction){

        if(Physics.Raycast(this.transform.position,Vector3.down,out RaycastHit hitpoint,1.5f,groundlayer)){
            return Vector3.ProjectOnPlane(direction,hitpoint.normal);
        }
        return direction;
    }

    //障碍检测  
    public bool CheckFrontObstacle(){
        bool isObstacle = Physics.Raycast(transform.position+Vector3.up,transform.forward,1.5f,Obstaclelayer);
        return isObstacle;
    }

}
