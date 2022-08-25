using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController2D))]
public class CombatDamageDealer : MonoBehaviour {
    
    // get stats for this damage source

    [SerializeField] LayerMask entityLayer;
    [SerializeField] Vector2 size;
    [SerializeField] Vector2 offset;

    EntityController2D controller;

    void Start() {
        controller = GetComponent<EntityController2D>();
    }

    void OnEnable() {
        Collider2D[] targets = Physics2D.OverlapBoxAll(
            transform.position + new Vector3(offset.x * controller.lookDir, offset.y, 0),
            size,
            0,
            entityLayer
        );


        // debugging box
        #region debugging vis
        Color color = targets.Length == 0 ? Color.red : Color.green;
        Debug.DrawRay(
            new Vector3(
                transform.position.x  - (size.x / 2) + (offset.x * controller.lookDir), 
                transform.position.y - (size.y / 2) + offset.y,
                transform.position.z
            ),
            Vector3.right * size.x,
            color,
            0.1f
        );
        Debug.DrawRay(
            new Vector3(
                transform.position.x  - (size.x / 2) + (offset.x * controller.lookDir), 
                transform.position.y - (size.y / 2) + offset.y,
                transform.position.z
            ),
            Vector3.up * size.y,
            color,
            0.1f
        );
        Debug.DrawRay(
            new Vector3(
                transform.position.x + (size.x / 2) + (offset.x * controller.lookDir), 
                transform.position.y + (size.y / 2) + offset.y,
                transform.position.z
            ),
            Vector3.left * size.x,
            color,
            0.1f
        );
        Debug.DrawRay(
            new Vector3(
                transform.position.x + (size.x / 2) + (offset.x * controller.lookDir), 
                transform.position.y + (size.y / 2) + offset.y,
                transform.position.z
            ),
            Vector3.down * size.y,
            color,
            0.1f
        );
        #endregion

        for(int i = 0; i < targets.Length; i++) {
            CombatTarget target = targets[i].GetComponent<CombatTarget>();
            if(target != null) {
                target.TakeDamage(this);
            }
        }
    }

    void OnDisable() {

    }
}
