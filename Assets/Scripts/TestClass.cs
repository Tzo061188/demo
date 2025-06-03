using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass
{
    public int i;

    ~TestClass(){
        Debug.Log("析构函数执行");
    }
}
