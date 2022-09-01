using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : Airborn {

    float ogGravity;

    public Jumping(Player player, PlayerStateMachine stateMachine, EntityData playerData, string animBoolName) 
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
        player.setVelActual(player.actualVelocity.x, 0);
        player.setVelY(playerData.jumpForce);
        ogGravity = gravity;
        gravity = 0;
    }

    public override void Exit() {
        base.Exit();
        gravity = ogGravity;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();    
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        
        if(!jump || Time.time - startTime > playerData.jumpIncreaseTime) {
            stateMachine.ChangeState(player.FallingFromJumpState);
        }

        player.setVelX(player.AirAccelerate(moveDir, m_airAcceleration, playerData.maxAirSpeed, playerData.airFriction));
    }

}
