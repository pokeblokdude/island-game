using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardLanding : Grounded {

    public HardLanding(Player player, PlayerStateMachine stateMachine, EntityData playerData, string animBoolName)
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
        playerData.friction *= 2;
        DO_MOVEMENT = false;
    }

    public override void Exit() {
        base.Exit();
        playerData.friction /= 2;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        // if(Time.time - startTime >= 1f) {
        //     stateMachine.ChangeState(player.IdleState);
        // }
        stateMachine.ChangeState(player.IdleState);
    }
}
