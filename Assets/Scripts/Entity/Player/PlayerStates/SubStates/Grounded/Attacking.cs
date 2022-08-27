using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : Grounded {

    public Attacking(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
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
        player.attackHitbox.enabled = true;
        DO_MOVEMENT = false;
    }

    public override void Exit() {
        base.Exit();
        player.attackHitbox.enabled = false;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if(FIRST_FRAME) {
            //player.setGroundSpeed(7 * player.lookDir);
            FIRST_FRAME = false;
        }
        else {
            player.setGroundSpeed(player.Accelerate(0, m_grounded_accel, m_grounded_maxSpeed, m_grounded_friction));
        }

        if(Time.time - startTime > 0.3f) {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
