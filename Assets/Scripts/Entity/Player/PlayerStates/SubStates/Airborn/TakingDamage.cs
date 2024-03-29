using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakingDamage : Airborn {

    int damageAmount;
    CombatStats playerCombatStats;

    public TakingDamage(Player player, PlayerStateMachine stateMachine, EntityData playerData, string animBoolName)
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
        playerCombatStats = player.combatTarget.GetCombatStats();
        takingDamage = true;
        damageAmount = player.combatTarget.lastDamageAmount;
        player.OnPlayerTakeDamage.Invoke();
    }

    public override void Exit() {
        base.Exit();
        takingDamage = false;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        if(FIRST_FRAME) {
            if(grounded) {
                player.setGroundSpeed(
                    Mathf.Sign(player.transform.position.x - player.combatTarget.lastDamageSource.x) * playerCombatStats.groundedKnockbackSpeed
                );
            }
            else {
                player.setVelX(
                    Mathf.Sign(player.transform.position.x - player.combatTarget.lastDamageSource.x) * playerCombatStats.airbornKnockbackSpeed
                );
                player.setVelY(player.actualVelocity.y < 0 ? playerCombatStats.airbornKnockbackSpeed * 2 : player.actualVelocity.y);
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

        if(Time.time - startTime > playerCombatStats.damageIFrameTime) {
            if(grounded) {
                stateMachine.ChangeState(player.IdleState);
            }
            else {
                stateMachine.ChangeState(player.FallingState);
            }
        }
    }
}
