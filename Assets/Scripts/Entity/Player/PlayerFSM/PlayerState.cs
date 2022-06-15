using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState {
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected InputManager input;

    protected float startTime;

    private string animBoolName;

    protected bool CALCULATE_GRAVITY = true;

    protected float gravity;
    protected bool grounded;
    protected float moveDir = 0;
    protected bool crouch = false;
    protected bool jump = false;
    protected bool action = false;
    protected bool actionUp = false;
    protected int touchingWall;
    protected bool holdingJump = false;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() {
        DoLogicChecks();
        //TODO: RE-IMPLEMENT ANIMATIONS
        //player.anim.SetBool(animBoolName, true);
        startTime = Time.time;
        //Debug.Log(animBoolName);
        gravity = playerData.gravity;

        //player.ledgeGrabPoint.disable();
    }

    public virtual void Exit() {
        //TODO: see above
        //player.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate() {
        DoLogicChecks();
    }

    public virtual void PhysicsUpdate() {
        DoPhysicsChecks();
    }

    public virtual void DoLogicChecks() {
        grounded = player.controller.isGrounded();
        moveDir = player.input.moveDir;
        crouch = player.input.crouch;
        jump = player.input.jump;
        action = player.input.action;
        actionUp = player.input.actionUp;
        touchingWall = player.controller.isTouchingWall();

        UpdateInputQueue();
    }

    public virtual void DoPhysicsChecks() {
        if(!jump) {
            holdingJump = false;
        }
        // GRAVITY
        if(CALCULATE_GRAVITY) {
            if(grounded || player.controller.isBumpingHead()) {
                player.setVelY(-playerData.gravity * Time.fixedDeltaTime);
            }
            else if(player.actualVelocity.y >= playerData.maxFallSpeed) {
                player.setVelY(player.wishVelocity.y - (gravity * Time.fixedDeltaTime));
            }
        }
    }

    void UpdateInputQueue() {
        Action a = player.inputQueue.Check();
        if(a != null) {
            switch(a.actionType) {
                case Action.ActionType.JUMP:
                    if(Time.time - a.enqueueTime > playerData.jumpQueueTime) {
                        player.inputQueue.ClearOne();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public string Name() {
        return animBoolName;
    }

    // DEBUGGING
    public bool calculatingGravity() {
        return CALCULATE_GRAVITY;
    }
}  
