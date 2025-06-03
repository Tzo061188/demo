using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomCamrea : MonoBehaviour
{

    private Vector2  mousePos;

    private float UD;
    private float LR;
    private Transform Camera;

    private Transform target;
    public float YfollowSpeed;
    public  float speedMultp = 1;
    public Vector3 cameraToRoleDistacne;
    public AnimationCurve CameralenghtCurve;
    public AnimationCurve LookFaceCurve;

    private void Awake() {
        Camera = transform.GetChild(0).transform;
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if(GameManager.Instance.isCameraCanMove == true){

            CamreaRotate();
            CameraFollow();
            CameraLenght();
        }
    }

    public void InitTarget(Transform target){
        this.target = target;
        this.transform.localPosition = target.position;
    }

    public void CamreaRotate(){
        mousePos =  RoleInputSystem.Instance.MousePos;
        UD -= mousePos.y * speedMultp;
        LR += mousePos.x * speedMultp;
        UD = Mathf.Clamp(UD,5,60);
       // print(UD+""+LR);
        this.transform.rotation = Quaternion.Euler(UD,LR,0);
    }

    public void CameraFollow(){
        if(target != null){

            float newY =  Mathf.Lerp(transform.position.y,target.position.y,YfollowSpeed*Time.deltaTime);
            transform.position = new Vector3(target.position.x,newY,target.position.z);
        }
    }
    public void CameraLenght(){
       Camera.localPosition = new Vector3(cameraToRoleDistacne.x,cameraToRoleDistacne.y,CameralenghtCurve.Evaluate(UD)+cameraToRoleDistacne.z);
    }
}

