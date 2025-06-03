using UnityEditor;
using UnityEngine;

/// <summary>
/// 一个控件的信息
/// </summary>
public class ControlStrInfo {
    //控件名称属性字符串
    public string controlName;
    //查找绑定字符串
    public string findStr;
    //添加事件绑定的字符串
    public string addEventStr;
    //控件函数的字符串
    public string funcStr;

    public static ControlStrInfo operator +(ControlStrInfo one,ControlStrInfo two) {
        if (two == null)
            return one;
        one.controlName += two.controlName;
        one.findStr += two.findStr;
        one.addEventStr += two.addEventStr;
        one.funcStr += two.funcStr;
        return one;
    }
}
        
