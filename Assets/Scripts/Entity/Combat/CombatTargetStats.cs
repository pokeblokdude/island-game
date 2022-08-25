using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="newCombatData", menuName="Data/Entity/CombatStats")]
public class CombatTargetStats : ScriptableObject {
    
    public int maxHealth = 10;
    public int armor = 5;

}
