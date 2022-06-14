using UnityEngine;

[CreateAssetMenu(fileName="newPlayerData", menuName="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject {
    
    public float maxFallSpeed = -20;

    [Header("Movement")]
    public float maxSpeed = 5f;
    public float maxAirSpeed = 7f;
    [Range(0, 1)] public float crouchSpeedMult = 0.5f;
    public float acceleration = 1f;
    public float airAcceleration = 2f;
    public float friction = 5f;
    public float crouchFrictionMult = 2f;
    public float airFriction = 30f;
    public float gravity = 5;
    public float fallingGravityMult = 2;
    public float maxSlopeAngle = 55;

    [Header("Jump")]
    public float jumpForce = 10;
    public float jumpIncreaseTime = 0.5f;
    public float coyoteTime = 0.1f;
    public float jumpQueueTime = 0.4f;

    [Header("Landing")]
    public float hardLandingThreshold = -10f;

}
