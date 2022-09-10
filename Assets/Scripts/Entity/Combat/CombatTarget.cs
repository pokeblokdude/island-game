using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatTarget : MonoBehaviour {
    
    [SerializeField] bool debug = true;
    [SerializeField] CombatStats combatStats;
    [SerializeField] Text healthText;
    SpriteRenderer sr;
    Material mat;

    [SerializeField] bool damagable = true;

    public bool dead { get; private set; } = false;

    public int health { get; private set; }
    public int armor { get; private set; }

    public Vector3 lastDamageSource { get; private set; }
    public int lastDamageAmount { get; private set; }
    public bool recentlyDamaged { get; private set; } = false;

    // TODO: take CombatStats into account when taking damage

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        mat = sr.material;
        health = combatStats.maxHealth;
        armor = combatStats.armor;
    }

    void FixedUpdate() {
        if(debug) {
            healthText.text = $"HP: {health.ToString("f0")}\n" +
                              $"A: {armor.ToString("f0")}";
        }
    }

    public void TakeDamage(Transform source, CombatStats sourceStats, int amount) {
        if(damagable && !dead) {
            lastDamageAmount = amount;
            lastDamageSource = source.position;
            StartCoroutine(RecentDamage());
            StartCoroutine(IFrames());
            StartCoroutine(DamageFlash());
            health -= amount;
            if(health <= 0) {
                health = 0;
                Die();
            }
        }
    }

    IEnumerator RecentDamage() {
        recentlyDamaged = true;
        yield return new WaitForSeconds(0.35f);
        recentlyDamaged = false;
    }

    IEnumerator IFrames() {
        damagable = false;
        yield return new WaitForSeconds(combatStats.damageIFrameTime);
        lastDamageAmount = 0;
        damagable = true;
    }

    IEnumerator DamageFlash() {
        mat.SetFloat("_DamageColorOpacity", 1);
        yield return new WaitForSeconds(0.25f);
        mat.SetFloat("_DamageColorOpacity", 0);
    }

    public void Heal(int amount) {
        if(health < combatStats.maxHealth) {
            health += amount;
        }
    }

    void Die() {
        dead = true;
        damagable = false;
        if(debug) {
            healthText.text = "Dead";
        }
    }

    public CombatStats GetCombatStats() {
        return combatStats;
    }
}

