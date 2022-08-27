using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakingDamage : Airborn {

    int damageAmount;

    public TakingDamage(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
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
        damageAmount = player.healthInfo.lastDamageAmount;
        player.healthInfo.SetInvulnerable(true);
    }

    public override void Exit() {
        base.Exit();
        player.healthInfo.SetInvulnerable(false);
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        if(FIRST_FRAME) {
            if(grounded) {
                player.setGroundSpeed(
                    Mathf.Sign(player.transform.position.x - player.healthInfo.lastDamageSource.transform.position.x) * playerData.damageKnockbackSpeed
                );
            }
            else {
                player.setVelX(
                    Mathf.Sign(player.transform.position.x - player.healthInfo.lastDamageSource.transform.position.x) * playerData.damageKnockbackSpeed
                );
            }
            FIRST_FRAME = false;
        }
        else {
            if(grounded) {
                player.setGroundSpeed(
                    player.Accelerate(0, playerData.acceleration, playerData.maxSpeed, playerData.friction)
                );
            }
            else{
                player.setVelX(
                    player.AirAccelerate(0, m_airAcceleration, playerData.maxAirSpeed, playerData.airFriction)
                );
            }
        }

        if(Time.time - startTime > playerData.damageIFrameTime) {
            if(grounded) {
                stateMachine.ChangeState(player.IdleState);
            }
            else {
                stateMachine.ChangeState(player.FallingState);
            }
        }
    }
}
