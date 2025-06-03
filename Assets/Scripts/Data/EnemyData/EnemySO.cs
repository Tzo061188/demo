using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemySO",menuName ="ScriptableObjectData/EnemySO")]
public class EnemySO :ScriptableObject{

    public EnemyReuseData EnemyReuseData = new EnemyReuseData();
}
