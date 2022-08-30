using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController2D))]
public class SpiritGuy : MonoBehaviour {
    
    [SerializeField] EntityData data;
    [SerializeField] bool useGravity = true;
    
    EntityController2D controller;
    Vector3 wishVel;
    Vector3 actualVel;
    bool grounded;

    void Awake() {
        controller = GetComponent<EntityController2D>();
    }

    void FixedUpdate() {
        // GRAVITY
        if(useGravity) {
            if(grounded || controller.isBumpingHead()) {
                wishVel.y = -data.gravity * Time.fixedDeltaTime;
            }
            else if(actualVel.y >= data.maxFallSpeed && !grounded) {
                wishVel.y = wishVel.y - (data.gravity * Time.fixedDeltaTime);
            }
        }

        wishVel.x = -1 * data.maxSpeed / 2;
        actualVel = controller.Move(wishVel * Time.fixedDeltaTime);
        grounded = controller.isGrounded();
    }
}
