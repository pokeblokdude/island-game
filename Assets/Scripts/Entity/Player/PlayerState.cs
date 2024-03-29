using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState {
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected EntityData playerData;
    protected InputManager input;

    protected float startTime;

    private string animBoolName;

    protected bool CALCULATE_GRAVITY = true;
    protected bool FIRST_FRAME = false;

    protected float gravity;
    protected bool grounded;
    protected float moveDir = 0;
    protected bool crouch = false;
    protected bool jump = false;
    protected bool action = false;
    protected bool attack = false;
    protected bool actionUp = false;
    protected int touchingWall;

    protected bool holdingJump = false;
    protected bool takingDamage = false;

    public PlayerState(Player player, PlayerStateMachine stateMachine, EntityData playerData, string animBoolName) {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() {
        DoLogicChecks();
        player.anim.SetBool(animBoolName, true);
        startTime = Time.time;
        FIRST_FRAME = true;
        //Debug.Log(animBoolName);
        gravity = playerData.gravity;
        holdingJump = jump;
    }

    public virtual void Exit() {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate() {
        DoLogicChecks();
    }

    public virtual void PhysicsUpdate() {
        DoPhysicsChecks();
    }

    public virtual void DoLogicChecks() {
        grounded = player.controller.isGrounded();
        moveDir = GameInput.Player.moveDir;
        crouch = GameInput.Player.crouch;
        jump = GameInput.Player.jump;
        action = GameInput.Player.action;
        actionUp = GameInput.Player.actionUp;
        touchingWall = player.controller.isTouchingWall();
        attack = GameInput.Player.attack;
    }

    public virtual void DoPhysicsChecks() {
        if(!jump) {
            holdingJump = false;
        }

        if(player.jumpBufferCounter > 0) {
            player.jumpBufferCounter -= Time.fixedDeltaTime;
        }
        
        // GRAVITY
        if(CALCULATE_GRAVITY) {
            if(grounded || player.controller.isBumpingHead()) {
                player.setVelY(-gravity * Time.fixedDeltaTime);
            }
            else if(player.actualVelocity.y >= playerData.maxFallSpeed && !grounded) {
                player.setVelY(player.wishVelocity.y - (gravity * Time.fixedDeltaTime));
            }
        }

        if(player.combatTarget.lastDamageAmount > 0 && !takingDamage) {
            stateMachine.ChangeState(player.TakingDamageState);
        }
    }

    public string Name() {
        return animBoolName;
    }

    #region DEBUGGING
    public bool calculatingGravity() {
        return CALCULATE_GRAVITY;
    }
    #endregion
}  
