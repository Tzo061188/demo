using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInputSystem : SingletonBase_MonoAuto<RoleInputSystem>
{

    public RoleInput roleInput;
    void Awake()
    {
        if(roleInput == null)
            roleInput = new RoleInput();
        DontDestroyOnLoad(this.gameObject);
    }

    public void Enable() {
        roleInput.Enable();
    }
    public void Disable() {
        roleInput.Disable();
    }

    void OnApplicationQuit()
    {
        if (RoleInputSystem.Instance != null)
        {
            Destroy(this.gameObject);
        }
    }   

    //键输入
    public Vector2 Move{
        get => roleInput.Role.Move.ReadValue<Vector2>();
    }
    public bool Attack{
        get=> roleInput.Role.Attack.triggered;
    }
    public bool Sprint{
        get=> roleInput.Role.Run.triggered;
    }
    public bool Run{
        get=> roleInput.Role.Run.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
    }

    public Vector2 MousePos{
        get  => roleInput.Role.Look.ReadValue<Vector2>();
    }
    public bool NormalSkill{
        get => roleInput.Role.NormalSkill.triggered;
    }
}
