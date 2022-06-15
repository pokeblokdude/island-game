using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(EntityController2D))]
public class Player : MonoBehaviour {
    
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
   
    #endregion

    public SpriteRenderer sr { get; private set; }
    public Animator anim { get; private set; }
    public BoxCollider2D col { get; private set; }

    [SerializeField] PlayerData playerData;
    [SerializeField] public LedgeGrabPoint ledgeGrabPoint;
    [SerializeField] Text text;

    public EntityController2D controller { get; private set; }
    public PlayerInput input { get; private set; }
    public InputQueue inputQueue { get; private set; }
    public Vector3 wishVelocity { get; private set; }
    public float groundSpeed { get; private set; }
    public Vector3 actualVelocity { get; private set; }

    public bool CALCULATE_COLLISION = true;

    void Awake() {
        input = GetComponent<PlayerInput>();
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

        #endregion
    }

    void Start() {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        inputQueue = new InputQueue();
        StateMachine.Initialize(IdleState);
    }

    void Update() {
        StateMachine.CurrentState.LogicUpdate();
        setDebugText();
    }

    void FixedUpdate() {
        StateMachine.CurrentState.PhysicsUpdate();
        if(CALCULATE_COLLISION) {
            if(controller.isGrounded()) {
                groundSpeed = actualVelocity.x;
            }
            actualVelocity = controller.Move(wishVelocity * Time.fixedDeltaTime, groundSpeed * Time.fixedDeltaTime);
        }
        Debug.DrawRay(transform.position, actualVelocity, Color.green, Time.fixedDeltaTime);
        Debug.DrawRay(transform.position, wishVelocity, Color.blue, Time.fixedDeltaTime);
        
        //anim.SetFloat("vSpeed", actualVelocity.y);
    }

    public void setVelX(float f) {
        wishVelocity = new Vector3(f, wishVelocity.y, wishVelocity.z);
        //groundSpeed = f;
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
        if(input.moveDir != 0) {
            if(Mathf.Sign(wishDir) != Mathf.Sign(speed) || Mathf.Abs(speed) < maxAirSpeed) {
                speed += wishDir * 330 * Time.fixedDeltaTime * airAcceleration / airFriction;
            }
        }
        return speed;
    }

    void setDebugText() {
        text.text =     $"FPS: {(1/Time.deltaTime).ToString("F0")}\n" +
                        $"deltaTime: {Time.deltaTime}\n\n" +
                        $"Current State: {StateMachine.CurrentState.Name()}\n" +
                        $"Speed: {actualVelocity.magnitude.ToString("f2")}\n" +
                        $"HSpeed: {actualVelocity.x.ToString("F4")}\n" +
                        $"VSpeed: {actualVelocity.y.ToString("F4")}\n" +
                        $"WishVel: {wishVelocity.ToString("F4")}\n" +
                        $"Position: {transform.position.ToString("F4")}\n" +
                        $"MoveDir: {input.moveDir}\n" +
                        $"Grounded: {controller.isGrounded()}\n" +
                        $"Slope Angle: {controller.SlopeAngle()}\n" +
                        $"Calculating Gravity: {StateMachine.CurrentState.calculatingGravity()}\n" +
                        $"Can Uncrouch: {controller.canUncrouch()}\n" +
                        $"Bumping Head: {controller.isBumpingHead()}\n" +
                        $"Touching Wall: {controller.isTouchingWall()}\n"
                        
        ;
    }

}
