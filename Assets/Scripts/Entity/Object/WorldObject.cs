using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour {
    
    [SerializeField] EntityData data;
    EntityController2D controller;

    Vector3 wishVel;
    Vector3 actualVel;
    bool useGravity = true;
    bool grounded = false;

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

        actualVel = controller.Move(wishVel * Time.fixedDeltaTime);
        grounded = controller.isGrounded();
    }

    public void SetVelX(float f) {
        wishVel.x = f;
    }
    public void SetVelY(float f) {
        wishVel.y = f;
    }
}
