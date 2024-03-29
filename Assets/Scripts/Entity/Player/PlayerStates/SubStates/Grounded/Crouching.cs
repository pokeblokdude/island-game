using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : Grounded {

    public Crouching(Player player, PlayerStateMachine stateMachine, EntityData playerData, string animBoolName)
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
            stateMachine.ChangeState(player.IdleState);
        }
        if(moveDir != 0) {
            stateMachine.ChangeState(player.CrouchingMovingState);
        }
    }

}
