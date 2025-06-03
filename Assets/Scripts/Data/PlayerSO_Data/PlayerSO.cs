using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerSO",menuName ="ScriptableObjectData/PlayerSO",order = 0)]
public class PlayerSO : ScriptableObject
{
    public MovementData movementData = new MovementData();
    public AttackModuleData attackModuleData = new AttackModuleData();
}
