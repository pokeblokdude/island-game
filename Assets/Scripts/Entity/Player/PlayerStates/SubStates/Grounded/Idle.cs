using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Grounded {
    
    bool holdingJump;

    public Idle(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        if(jump) {
            holdingJump = true;
        }
        Debug.Log($"time: {Time.time.ToString("f2")} jump time: {jumpQueueTimestamp.ToString("f2")}");
        Debug.Log($"jump queue time: {(Time.time - jumpQueueTimestamp).ToString("f2")}");
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        // reset jump
        if(!jump) {
            holdingJump = false;
        }
        if(Time.time - jumpQueueTimestamp < 0.5f) {
            holdingJump = false;
            jump = true;
        }

        if(actionUp) {
            player.anim.SetBool("lookUp", true);
        }
        else {
            player.anim.SetBool("lookUp", false);
        }

        // State Change Checks
        if(moveDir != 0) {
            stateMachine.ChangeState(player.MovingState);
        }
        if(crouch) {
            stateMachine.ChangeState(player.CrouchingState);
        }
        
        if(jump && !holdingJump) {
            stateMachine.ChangeState(player.JumpingState);
        }
        if(touchingWall != 0 && Time.time - startTime > 3) {
            stateMachine.ChangeState(player.LeaningState);
        }
    }

}
