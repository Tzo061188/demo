using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using XLua;
using static UnityEngine.InputSystem.InputAction;



public static class Lua_Attribute{

    [CSharpCallLua]  //unity 使用  lua的
    public static List<Type> csharpCallLua = new List<Type>{ 
            typeof(System.Action<UnityEngine.InputSystem.InputAction.CallbackContext>),
            typeof(UnityAction<bool>),
            typeof(UnityAction<int>),
            typeof(UnityAction<float>),
            typeof(UnityAction),
            typeof(PlayerHealthSystem)
    };

    [LuaCallCSharp]   //lua使用unity的
    public static List<Type> luaCallCSharp= new List<Type>{ 
         typeof(PlayerHealthSystem),
          typeof(UnityAction<int>),
    };

    public static Vector2 GetPos(RectTransform Canvas){
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas,Input.mousePosition,null,out pos);
        return pos;
    }

}