using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatTarget : MonoBehaviour {
    
    [SerializeField] bool debug = true;
    [SerializeField] CombatTargetStats combatTargetStats;
    [SerializeField] Text healthText;
    SpriteRenderer sr;
    Material mat;

    [SerializeField] bool damagable = true;

    public bool dead { get; private set; } = false;

    public int health { get; private set; }
    public int armor { get; private set; }

    public CombatDamageDealer lastDamageSource { get; private set; }
    public int lastDamageAmount { get; private set; }

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        mat = sr.material;
        health = combatTargetStats.maxHealth;
        armor = combatTargetStats.armor;
    }

    void FixedUpdate() {
        if(debug) {
            healthText.text = $"HP: {health.ToString("f0")}\n" +
                              $"A: {armor.ToString("f0")}";
        }
    }

    public void TakeDamage(CombatDamageDealer source, int amount) {
        if(damagable && !dead) {
            lastDamageAmount = 1;
            lastDamageSource = source;
            StartCoroutine(DamageFlash());
            health -= 1;
            if(health <= 0) {
                health = 0;
                Die();
            }
        }
    }

    IEnumerator DamageFlash() {
        mat.SetFloat("_DamageColorOpacity", 1);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_DamageColorOpacity", 0);
    }

    void Die() {
        dead = true;
        damagable = false;
        if(debug) {
            healthText.text = "Dead";
        }
    }

    public void SetInvulnerable(bool i) {
        lastDamageAmount = i ? 0 : lastDamageAmount;
        damagable = !i;
    }
}

