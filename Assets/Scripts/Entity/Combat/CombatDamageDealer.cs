using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController2D))]
public class CombatDamageDealer : MonoBehaviour {
    
    [SerializeField] CombatStats stats;

    [Header("Options")]
    [SerializeField] LayerMask targetLayer;
    [SerializeField] bool bodyIsDamageSource = false;
    [SerializeField] bool applyDamageOnEnable = true;

    [Header("Separate Hitbox")]
    [SerializeField] Vector2 size;
    [SerializeField] Vector2 offset;

    EntityController2D controller;
    BoxCollider2D col;

    // TODO: take CombatStats into account when applying damage (instead of just 1 damage every time)

    void Start() {
        controller = GetComponent<EntityController2D>();
        col = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate() {
        if(bodyIsDamageSource) {
            Collider2D[] targets = Physics2D.OverlapBoxAll(
                transform.position + new Vector3(0, (col.size.y / 2), 0),
                col.size,
                0,
                targetLayer
            );
            ApplyDamageToTargets(targets);
        }
    }

    void OnEnable() {
        if(!bodyIsDamageSource && applyDamageOnEnable) {
            Collider2D[] targets = Physics2D.OverlapBoxAll(
                transform.position + new Vector3(offset.x * controller.lookDir, (col.size.y / 2) + offset.y, 0),
                size,
                0,
                targetLayer
            );

            // debugging box
            #region debugging vis
            Color color = targets.Length == 0 ? Color.red : Color.green;
            Debug.DrawRay(
                new Vector3(
                    transform.position.x  - (size.x / 2) + (offset.x * controller.lookDir), 
                    transform.position.y + (col.size.y/2) - (size.y / 2) + offset.y,
                    transform.position.z
                ),
                Vector3.right * size.x,
                color,
                0.1f
            );
            Debug.DrawRay(
                new Vector3(
                    transform.position.x  - (size.x / 2) + (offset.x * controller.lookDir), 
                    transform.position.y + (col.size.y/2) - (size.y / 2) + offset.y,
                    transform.position.z
                ),
                Vector3.up * size.y,
                color,
                0.1f
            );
            Debug.DrawRay(
                new Vector3(
                    transform.position.x + (size.x / 2) + (offset.x * controller.lookDir), 
                    transform.position.y + (col.size.y/2) + (size.y / 2) + offset.y,
                    transform.position.z
                ),
                Vector3.left * size.x,
                color,
                0.1f
            );
            Debug.DrawRay(
                new Vector3(
                    transform.position.x + (size.x / 2) + (offset.x * controller.lookDir), 
                    transform.position.y + (col.size.y/2) + (size.y / 2) + offset.y,
                    transform.position.z
                ),
                Vector3.down * size.y,
                color,
                0.1f
            );
            #endregion

            ApplyDamageToTargets(targets);
        }
    }

    /// <summary>
    /// Apply damage to multiple Collider2Ds (assuming they have CombatTarget components), utilizing this CombatDamageDealer's CombatStats
    /// </summary>
    public void ApplyDamageToTargets(Collider2D[] targets) {
        for(int i = 0; i < targets.Length; i++) {
            CombatTarget target = targets[i].GetComponent<CombatTarget>();
            if(target != null) {
                target.TakeDamage(transform, stats, 1);
            }
        }
    }
    /// <summary>
    /// Apply damage to multiple CombatTargets, utilizing this CombatDamageDealer's CombatStats
    /// </summary>
    public void ApplyDamageToTargets(CombatTarget[] targets) {
        for(int i = 0; i < targets.Length; i++) {
            targets[i].TakeDamage(transform, stats, 1);
        }
    }

    /// <summary>
    /// Apply damage to a given CombatTarget, utilizing this CombatDamageDealer's CombatStats
    /// </summary>
    public void ApplyDamageToTarget(CombatTarget target) {
        if(target != null) {
            target.TakeDamage(transform, stats, 1);
        }
    }
}
