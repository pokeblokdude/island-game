using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Grounded {

    public Idle(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    : base(player, stateMachine, playerData, animBoolName) {
        
    }

    public override void DoLogicChecks() {
        base.DoLogicChecks();
    }
    public override void DoPhysicsChecks() {
        base.DoPhysicsChecks();
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        // do enqueued jump
        if(player.jumpBufferCounter > 0) {
            player.jumpBufferCounter = 0;
            stateMachine.ChangeState(player.JumpingState);
        }

        if(actionUp) {
            player.anim.SetBool("lookUp", true);
        }
        else {
            player.anim.SetBool("lookUp", false);
        }

        // State Change Checks
        if(!grounded) {
            stateMachine.ChangeState(player.FallingState);
        }
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
