using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(EntityController2D))]
public class Player : MonoBehaviour {
    
    public UnityEvent OnPlayerTakeDamage;

    #region State Machine Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public Idle IdleState { get; private set; }
    public Leaning LeaningState { get; private set; }
    public Moving MovingState { get; private set; }
    public Crouching CrouchingState { get; private set; }
    public CrouchingMoving CrouchingMovingState { get; private set; }

    public HardLanding HardLandingState { get; private set; }

    public Falling FallingState { get; private set; }
    public FallingFromJump FallingFromJumpState { get; private set; }
    public Jumping JumpingState { get; private set; }
   
    public Attacking AttackingState { get; private set; }
    public TakingDamage TakingDamageState { get; private set; }
    #endregion

    public SpriteRenderer sr { get; private set; }
    public Animator anim { get; private set; }
    public BoxCollider2D col { get; private set; }

    [SerializeField] EntityData playerData;
    [SerializeField] Text text;

    public CombatDamageDealer damageDealer { get; private set; }
    public CombatTarget combatTarget { get; private set; }

    public EntityController2D controller { get; private set; }
    public float groundSpeed { get; private set; }
    public Vector3 wishVelocity { get; private set; }
    public Vector3 actualVelocity { get; private set; }
    public int lookDir { get; private set; } = 1;

    [HideInInspector]
    public float jumpBufferCounter;

    public bool CALCULATE_COLLISION = true;

    void Awake() {
        controller = GetComponent<EntityController2D>();

        #region StateMachine Init
        StateMachine = new PlayerStateMachine();

        IdleState = new Idle(this, StateMachine, playerData, "idle");
        LeaningState = new Leaning(this, StateMachine, playerData, "leaning");
        MovingState = new Moving(this, StateMachine, playerData, "moving");
        CrouchingState = new Crouching(this, StateMachine, playerData, "crouching");
        CrouchingMovingState = new CrouchingMoving(this, StateMachine, playerData, "crouchingMoving");

        HardLandingState = new HardLanding(this, StateMachine, playerData, "hardLanding");

        JumpingState = new Jumping(this, StateMachine, playerData, "jumping");
        FallingState = new Falling(this, StateMachine, playerData, "falling");
        FallingFromJumpState = new FallingFromJump(this, StateMachine, playerData, "fallingFromJump");

        AttackingState = new Attacking(this, StateMachine, playerData, "attacking");
        TakingDamageState = new TakingDamage(this, StateMachine, playerData, "damaged");
        #endregion
    }

    void Start() {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        StateMachine.Initialize(IdleState);
        damageDealer = GetComponent<CombatDamageDealer>();
        damageDealer.enabled = false;
        combatTarget = GetComponent<CombatTarget>();
    }

    void Update() {
        StateMachine.CurrentState.LogicUpdate();
        setDebugText();
        if(GameInput.Debug.toggleDebugRays) {
            controller.ToggleDebugMode();
        }
    }

    void FixedUpdate() {
        StateMachine.CurrentState.PhysicsUpdate();
        if(CALCULATE_COLLISION) {
            if(controller.isGrounded()) {
                wishVelocity = new Vector3(
                    groundSpeed,
                    wishVelocity.y,
                    0
                );
            }
            actualVelocity = controller.Move(wishVelocity * Time.fixedDeltaTime);
            if(!controller.isGrounded()) {
                groundSpeed = actualVelocity.x;
            }
            
        }
        lookDir = sr.flipX ? -1 : 1;
        controller.OverrideLookDirection(lookDir);
        //Debug.DrawRay(transform.position, actualVelocity /* Time.fixedDeltaTime * 3*/, Color.green, Time.fixedDeltaTime);
        //Debug.DrawRay(transform.position, wishVelocity /* Time.fixedDeltaTime * 3*/, Color.blue, Time.fixedDeltaTime);
        
        //anim.SetFloat("vSpeed", actualVelocity.y);
    }

    public void setGroundSpeed(float f) {
        groundSpeed = f;
    }

    public void setVelX(float f) {
        wishVelocity = new Vector3(f, wishVelocity.y, wishVelocity.z);
    }

    public void setVelY(float f) {
        wishVelocity = new Vector3(wishVelocity.x, f, wishVelocity.z);
    }

    public void setVelZero() {
        wishVelocity = Vector2.zero;
        actualVelocity = Vector2.zero;
    }

    public void setVelActual(Vector2 vel) {
        actualVelocity = vel;
    }

    public void setVelActual(float x, float y) {
        actualVelocity = new Vector3(x, y, 0);
    }

    public float Accelerate(float wishDir, float acceleration, float maxSpeed, float friction) {
        float speed = groundSpeed;
        if(wishDir != 0) {
            // 330 was about the framerate I was playing at before I fixed the deltaTime issue
            if(Mathf.Abs(speed) < maxSpeed) {
                speed += wishDir * 330 * Time.fixedDeltaTime * acceleration / friction;
            }
            if(Mathf.Abs(speed) > maxSpeed) {
                speed -= Mathf.Sign(speed) * friction * acceleration * Time.fixedDeltaTime;
            }
        }
        else {
            float decelSpeed = speed - Mathf.Sign(speed) * friction * Time.fixedDeltaTime;
            speed = (Mathf.Sign(decelSpeed) != Mathf.Sign(speed)) ? 0 : decelSpeed;
        }
        return speed;
    }

    public float AirAccelerate(float wishDir, float airAcceleration, float maxAirSpeed, float airFriction) {
        float speed = actualVelocity.x;
        if(GameInput.Player.moveDir != 0) {
            if(Mathf.Sign(wishDir) != Mathf.Sign(speed) || Mathf.Abs(speed) < maxAirSpeed) {
                speed += wishDir * 330 * Time.fixedDeltaTime * airAcceleration / airFriction;
            }
        }
        return speed;
    }

    void setDebugText() {
        text.text =     $"FPS: {(1/Time.deltaTime).ToString("F0")}\n" +
                        $"deltaTime: {Time.deltaTime}\n\n" +
    
                        $"HP: {combatTarget.health}\n\n" +

                        $"Current State: {StateMachine.CurrentState.Name()}\n" +
                        $"Speed: {actualVelocity.magnitude.ToString("f2")}\n" +
                        $"GroundSpeed: {groundSpeed.ToString("f4")}\n" +
                        $"HSpeed: {actualVelocity.x.ToString("F4")}\n" +
                        $"VSpeed: {actualVelocity.y.ToString("F4")}\n" +
                        $"WishVel: {wishVelocity.ToString("F4")}\n" +
                        $"Position: {transform.position.ToString("F4")}\n" +
                        $"MoveDir: {GameInput.Player.moveDir}\n" +
                        $"LookDir: {lookDir}\n" +
                        $"Grounded: {controller.isGrounded()}\n" +
                        $"Slope Angle: {controller.SlopeAngle()}\n" +
                        $"Calculating Gravity: {StateMachine.CurrentState.calculatingGravity()}\n" +
                        $"Can Uncrouch: {controller.canUncrouch()}\n" +
                        $"Bumping Head: {controller.isBumpingHead()}\n" +
                        $"Touching Wall: {controller.isTouchingWall()}\n\n" +

                        $"Jump Buffer: {jumpBufferCounter}\n"
                        
        ;
    }

}
