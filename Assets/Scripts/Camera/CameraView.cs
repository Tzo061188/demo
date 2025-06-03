using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    private float UpAndDownRotate = 0f;
    public float LeftAndRightRotate = 0f;
    public int rotateSpeed = 5;
    public int lerpSpeed = 5;
    public AnimationCurve CameraCurveLenght;
    private Transform target;
    private Transform Camera;
    private void Awake() {
        Camera = transform.GetChild(0);
    }
    void Update()
    {  
        CameraFollow();
        CameraMove();
        SetCameraLenght();
    }

    public void InItTarget(Transform target){
        this.target = target;
    }

    //摄像机跟随
    private void CameraFollow(){
        Vector3 pos = target.position;
        float newY = Mathf.Lerp(transform.position.y,pos.y,Time.deltaTime*lerpSpeed);
        this.transform.position = new Vector3(pos.x,newY,pos.z);

    }
   
    //摄像机移动
    private void CameraMove(){

        LeftAndRightRotate += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        UpAndDownRotate -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
        UpAndDownRotate =  Mathf.Clamp(UpAndDownRotate,-10,60);
        
        this.transform.rotation = Quaternion.Euler(UpAndDownRotate,LeftAndRightRotate,0);

    }
     //设置相机长度曲线
    private void SetCameraLenght(){
        //Camera.transform.position = new Vector3(0,0,CameraCurveLenght.Evaluate(UpAndDownRotate)*-1);
    }
}
