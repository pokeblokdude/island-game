using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airborn : PlayerState {

    protected float m_airAcceleration;
    bool holdingJump = false;

    public Airborn(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
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
        if(!jump) {
            holdingJump = false;
        }

        if(player.actualVelocity.y < 0) {
            gravity = playerData.gravity * playerData.fallingGravityMult;
        }

        if(jump && !holdingJump) {
            player.inputQueue.Add(new Action(Action.ActionType.JUMP));
            Debug.Log("queuing a jump");
        }
    }

}
