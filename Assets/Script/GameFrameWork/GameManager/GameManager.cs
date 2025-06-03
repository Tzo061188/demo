using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.Video;
public class GameManager: SingletonBase_Mono<GameManager>{
    public AudioClip BG_music;
    public GameObject NPC;
    public GameObject player;
    public CustomCamrea customCamrea;
    public GameObject currentAttackEnemy;
    public GameObject eventSystem;

    public bool isCameraCanMove = true;
    public bool isPlayerCanMove = true;

    [Header("加载场景时")]
    public RectTransform Canvas;
    private Image backGround;
    private Text text;
    public MainPanel mainPanel;
TestClass testClass;
    protected override void Awake() {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(Canvas.gameObject);
        DontDestroyOnLoad(eventSystem);

        

        MusicManager.Instance.PlayMusic(BG_music);

        backGround = Canvas.Find("SceneLoadBar").GetComponent<Image>();
        text = Canvas.Find("SceneLoadBar/Text").GetComponent<Text>();
        

        testClass = new TestClass();
        testClass.i = 10;
        

    }
    //lua 调用了
    public void LoadScene(){

        SceneLoadManager.Instance.AsyncLoadScene("AttackScene",() => {
            CreatePlayer();        
        });

    }

    public void Show_LoadScene_UI_Animation(AsyncOperation operation){
        StartCoroutine(ToValueControlSceneLoad(operation)); 
    }



    public void CreatePlayer(){
        testClass = null;

        RoleInputSystem.Instance.Enable();

        GameObject bornPoint =  GameObject.FindGameObjectWithTag("BornPoint");
        GameObject  playObj =  ResourcesManager.Instance.loadRes<GameObject>("Prefabs/Model/Player");


        playObj = Instantiate(playObj,bornPoint.transform.position,bornPoint.transform.rotation);
        

        player = playObj;
        mainPanel.playerHealthSystem = player.GetComponent<PlayerHealthSystem>();
        
        Player play =  playObj.GetComponent<Player>();

        play.animator = play.gameObject.transform.Find("安比").GetComponent<Animator>();
        //初始化状态机
        play.playerAttackStateMachine = new PlayerAttackStateMachine(play, play.animator);
        play.playerMovementStateMachine = new PlayerMovementStateMachine(play, play.animator);

        play.playerAttackStateMachine.ChangedState(play.playerAttackStateMachine.nullState);
        play.playerMovementStateMachine.ChangedState(play.playerMovementStateMachine.idleState);

    }


    private IEnumerator ToValueControlSceneLoad(AsyncOperation operation){
        operation.allowSceneActivation = false;
        yield return ToValue(1);

        operation.allowSceneActivation = true;
        yield return ToValue(0);
    } 

    private IEnumerator ToValue(float a_Value){

        float a_Current_Value  = backGround.color.a;
        backGround.gameObject.SetActive(true);

        Color color_BG = backGround.color;
        Color color_text = text.color;

        while (a_Current_Value != a_Value)
        {
            if(a_Current_Value > a_Value){

                a_Current_Value -= Time.deltaTime; 
                if(a_Current_Value <= a_Value)  //减完后 如果小了设置值  那么直接等于 退出循环
                    a_Current_Value = a_Value;

            }else{

                a_Current_Value += Time.deltaTime;
                if(a_Current_Value >= a_Value)
                    a_Current_Value = a_Value;

            }
            color_BG.a = a_Current_Value;
            color_text.a = a_Current_Value;

            backGround.color = color_BG;
            text.color = color_text;

            yield return null;          
        }

        backGround.gameObject.SetActive(false);

    } 

}