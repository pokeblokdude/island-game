using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airborn : PlayerState {

    protected float m_airAcceleration;

    public Airborn(Player player, PlayerStateMachine stateMachine, EntityData playerData, CombatStats playerCombatStats, string animBoolName) 
    : base(player, stateMachine, playerData, playerCombatStats, animBoolName) {
        
    }

    public override void DoLogicChecks() {
        base.DoLogicChecks();
    }
    public override void DoPhysicsChecks() {
        base.DoPhysicsChecks();
    }

    public override void Enter() {
        base.Enter();
        m_airAcceleration = playerData.airAcceleration;
        if(jump) {
            holdingJump = true;
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

        if(player.actualVelocity.y < 1) {
            gravity = playerData.gravity * playerData.fallingGravityMult;
        }

        if(jump && !holdingJump) {
            player.jumpBufferCounter = playerData.jumpBufferTime;
            holdingJump = true;
            Debug.Log("queuing a jump");
        }
        holdingJump = jump;
    }

}
