using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatTarget : MonoBehaviour {
    
    [SerializeField] CombatTargetStats combatTargetStats;
    [SerializeField] Slider healthBar;

    public int health { get; private set; }
    public int armor { get; private set; }

    void Start() {
        health = combatTargetStats.maxHealth;
        armor = combatTargetStats.armor;
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    void Update() {
        healthBar.value = health;
    }

    public void TakeDamage(CombatDamageDealer source) {
        health -= 1;
    }

}

