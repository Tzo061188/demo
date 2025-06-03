using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendFunction
{
    /// <summary>
    /// 检测动画是否处于该标签
    /// </summary>
    /// <param name="layer">层级</param>
    /// <param name="Tag">标签</param>
    public static bool CheckAnimation_TagIs(this Animator animator,int layer,string Tag){
        return animator.GetCurrentAnimatorStateInfo(0).IsTag(Tag);
    }
}
