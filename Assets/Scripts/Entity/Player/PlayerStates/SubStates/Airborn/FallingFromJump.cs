using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFromJump : Airborn {

    bool holdingJump;
    bool holdingAction;

    public FallingFromJump(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
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
        }
        if(action) {
            holdingAction = true;
        }
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        if(!action) {
            holdingAction = false;
        }
        if(!jump) {
            holdingJump = false;
        }

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
        if(moveDir == -1 && player.actualVelocity.x < 3) {
            player.sr.flipX = true;
        }
        if(moveDir == 1 && player.actualVelocity.x > -3) {
            player.sr.flipX = false;
        }
    }

}
