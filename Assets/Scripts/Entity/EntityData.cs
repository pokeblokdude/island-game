using UnityEngine;

[CreateAssetMenu(fileName="newEntityData", menuName="Data/Entity/Base Data")]
public class EntityData : ScriptableObject {
    
    public float maxFallSpeed = -20;

    [Header("Movement")]
    public float maxSpeed = 4f;
    public float maxAirSpeed = 5f;
    [Range(0, 1)] public float crouchSpeedMult = 0.5f;
    public float acceleration = 2f;
    public float airAcceleration = 0.6f;
    public float friction = 15f;
    public float crouchFrictionMult = 2f;
    public float airFriction = 30f;
    public float gravity = 20;
    public float fallingGravityMult = 2.5f;
    public float maxSlopeAngle = 55f;

    [Header("Jump")]
    public float jumpForce = 6.5f;
    public float jumpIncreaseTime = 0.15f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.2f;

    [Header("Landing")]
    public float hardLandingThreshold = -50f;
}
