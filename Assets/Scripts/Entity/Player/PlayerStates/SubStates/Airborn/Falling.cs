using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : Airborn {

    public Falling(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
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
        if(player.controller.isGrounded()) {
            if(player.wishVelocity.y < playerData.hardLandingThreshold) {
                stateMachine.ChangeState(player.HardLandingState);
            }
            else {
                if(moveDir == 0) {
                    stateMachine.ChangeState(player.IdleState);
                }
                else {
                    stateMachine.ChangeState(player.MovingState);
                }
            }
        }

        base.PhysicsUpdate();

        player.setVelX(player.AirAccelerate(moveDir, m_airAcceleration, playerData.maxAirSpeed, playerData.airFriction));

        // sprite flipping
        if(moveDir == -1 && player.actualVelocity.x < 4) {
            player.sr.flipX = true;
        }
        if(moveDir == 1 && player.actualVelocity.x > -4) {
            player.sr.flipX = false;
        }
    }

}
