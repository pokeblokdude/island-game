using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="newCombatData", menuName="Data/Entity/CombatStats")]
public class CombatStats : ScriptableObject {
    
    [Header("Stats")]
    public int maxHealth = 10;
    public int armor = 5;

    [Header("Taking Damage")]
    public float damageIFrameTime = 0.5f;
    public float groundedKnockbackSpeed = 6;
    public float airbornKnockbackSpeed = 4;

    [Header("Dealing Damage")]
    public float dealAmount = 1;

}
