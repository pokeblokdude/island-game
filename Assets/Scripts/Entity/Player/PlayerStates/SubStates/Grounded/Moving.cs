using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : Grounded {
    
    bool jumpOnFirstFrame;

    public Moving(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
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
        jumpOnFirstFrame = false;
        Action a = player.inputQueue.Read();
        if(a != null) {
            if(a.actionType == Action.ActionType.JUMP) {
                float jumpTime = Time.time - a.enqueueTime;
                if(jumpTime < playerData.jumpQueueTime) {
                    jumpOnFirstFrame = true;
                }
                else {
                    Debug.Log("jump queued for too long " + (Time.time - a.enqueueTime));
                    jumpOnFirstFrame = false;
                }
            }
        }
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if(jumpOnFirstFrame) {
            stateMachine.ChangeState(player.JumpingState);
        }

        if(crouch) {
            stateMachine.ChangeState(player.CrouchingMovingState);
        }

        if(jump && !holdingJump) {
            stateMachine.ChangeState(player.JumpingState);
        }

        if(player.actualVelocity.x == 0) {
            stateMachine.ChangeState(player.IdleState);
        }

        // sprite flipping
        if(moveDir == -1) {
            player.sr.flipX = true;
        }
        if(moveDir == 1) {
            player.sr.flipX = false;
        }
    }

}
