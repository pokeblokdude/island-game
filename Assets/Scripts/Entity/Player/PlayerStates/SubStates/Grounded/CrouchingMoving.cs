using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingMoving : Grounded {

    public CrouchingMoving(Player player, PlayerStateMachine stateMachine, EntityData playerData, string animBoolName)
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
        m_grounded_friction *= playerData.crouchFrictionMult;
        m_grounded_maxSpeed *= playerData.crouchSpeedMult;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if(!crouch && player.controller.canUncrouch()) {
            stateMachine.ChangeState(player.MovingState);
        }
        if(moveDir == 0) {
            stateMachine.ChangeState(player.CrouchingState);
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
