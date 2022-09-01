using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : Grounded {

    public Moving(Player player, PlayerStateMachine stateMachine, EntityData playerData, string animBoolName)
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
        if(player.jumpBufferCounter > 0) {
            stateMachine.ChangeState(player.JumpingState);
        }

        if(attack) {
            stateMachine.ChangeState(player.AttackingState);
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
