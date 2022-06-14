using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Grounded {
    
    bool holdingJump;
    bool jumpOnFirstFrame = false;

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
        if(jump) {
            holdingJump = true;
            Debug.Log("holding jump");
        }
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
        // do enqueued jump
        if(jumpOnFirstFrame) {
            stateMachine.ChangeState(player.JumpingState);
        }
        // reset jump
        if(!jump) {
            holdingJump = false;
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
