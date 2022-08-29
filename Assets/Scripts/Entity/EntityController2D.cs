using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EntityController2D : MonoBehaviour {
    
    [SerializeField] EntityData playerData;
    public LayerMask collisionMask;
    [SerializeField] bool debug;
    [SerializeField] int defaultLookDir;

    BoxCollider2D col;
    Rigidbody2D rb;

    const float skinWidth = 0.015f;

    float slopeAngle, slopeAngleOld;
    Vector2 slopePoint;
    bool climbingSlope, descendingSlope;
    bool grounded = false;
    bool bumpingHead = false;
    bool touchingWall = false;
    int wallDir = 0;

    public int lookDir { get; private set; } = 1;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    public Vector3 Move(Vector3 moveAmount) {
        ResetVariables();

        if(moveAmount.y < 0) {
            DescendSlope(ref moveAmount);
        }
        // do horizontal collisions if moving.
        if(moveAmount.x != 0) {
            HorizontalCollisions(ref moveAmount);
        }
        // do vertical collisions if moving.
        if(moveAmount.y != 0) {
            VerticalCollisions(ref moveAmount);
        }
        
        // apply velocity
        rb.MovePosition(new Vector3(rb.position.x, rb.position.y, transform.position.z) + moveAmount);
        if(moveAmount.x != 0) {
            lookDir = (int)Mathf.Sign(moveAmount.x);
        }
        return moveAmount / Time.fixedDeltaTime;
    }

    void HorizontalCollisions(ref Vector3 moveAmount) {
        // set the ray direction/length equal to the X wish velocity
        float directionX = Mathf.Sign(moveAmount.x);
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        // first raycast to check for slopes
        RaycastHit2D slopeHit = Physics2D.Raycast(
            new Vector2(bounds.center.x, bounds.min.y + (climbingSlope ? moveAmount.y : 0)),
            Vector2.right * directionX,
            bounds.extents.x + rayLength,
            collisionMask
        );
        if(debug) Debug.DrawRay(new Vector2(bounds.center.x, bounds.min.y + (climbingSlope ? moveAmount.y : 0)), Vector2.right * directionX * (bounds.extents.x + rayLength), Color.cyan, Time.fixedDeltaTime);
        
        slopeAngle = slopeAngle == 0 ? Vector2.Angle(slopeHit.normal, Vector2.up) : slopeAngle;
        if(slopeHit && slopeAngle <= playerData.maxSlopeAngle) {
            if(debug) Debug.DrawRay(slopeHit.point, slopeHit.normal * 0.5f, Color.white, Time.fixedDeltaTime);
            float distanceToSlope = 0;
            if(slopeAngle != slopeAngleOld) {
                distanceToSlope = slopeHit.distance - bounds.extents.x - skinWidth;
                moveAmount.x -= distanceToSlope * directionX;
            }
            ClimbSlope(ref moveAmount, slopeAngle);
            moveAmount.x += distanceToSlope * directionX;
        }

        // now check for walls
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.right * directionX, rayLength, collisionMask);
        if(debug) {
            Color color = hit ? Color.green : Color.red;
            Debug.DrawRay(new Vector2(directionX == 1 ? bounds.max.x : bounds.min.x, bounds.min.y), Vector2.right * directionX * rayLength, color, Time.fixedDeltaTime);
            Debug.DrawRay(new Vector2(directionX == 1 ? bounds.max.x + rayLength : bounds.min.x - rayLength, bounds.max.y), Vector2.down * bounds.size.y, color, Time.fixedDeltaTime);
            Debug.DrawRay(new Vector2(directionX == 1 ? bounds.max.x : bounds.min.x, bounds.max.y), Vector2.right * directionX * rayLength, color, Time.fixedDeltaTime);
        }
        if(hit && Vector2.Angle(hit.normal, Vector2.up) > playerData.maxSlopeAngle && (!climbingSlope || slopeAngle > playerData.maxSlopeAngle)) {
            if(debug) Debug.DrawRay(hit.point, hit.normal * 0.5f, Color.white, Time.fixedDeltaTime);
            moveAmount.x = (hit.distance - skinWidth) * directionX;
            touchingWall = true;
        }
        else {
            touchingWall = false;
        }
        wallDir = touchingWall ? (int)Mathf.Sign(hit.point.x - transform.position.x) : 0;
    }

    void VerticalCollisions(ref Vector3 moveAmount) {
        // set the ray direction/length equal to the Y wish velocity
        float directionY = climbingSlope ? -1 : Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + (descendingSlope ? 2 * skinWidth : skinWidth);

        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.center.y), Vector2.up * directionY, bounds.extents.y + rayLength, collisionMask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.center.y), Vector2.up * directionY, bounds.extents.y + rayLength, collisionMask);
        if(debug) {
            Color color = hit1 || hit2 ? Color.green : Color.red;
            Debug.DrawRay(new Vector2(bounds.min.x, bounds.center.y), Vector2.up * directionY * (bounds.extents.y + rayLength), color, Time.fixedDeltaTime);
            Debug.DrawRay(new Vector2(bounds.max.x, bounds.center.y), Vector2.up * directionY * (bounds.extents.y + rayLength), color, Time.fixedDeltaTime);
        }

        // pick the closer collision point
        RaycastHit2D closerHit = new RaycastHit2D();
        if(hit1) {
            if(hit2) {
                closerHit = hit1.distance < hit2.distance ? hit1 : hit2;
            }
            else {
                closerHit = hit1;
            }
        }
        else if(hit2) {
            closerHit = hit2;
        }

        // actual collision
        if(closerHit) {
            if(debug) {
                Debug.DrawRay(closerHit.point, closerHit.normal * 0.5f, Color.white, Time.fixedDeltaTime);
                Debug.DrawRay(closerHit.point, closerHit.normal * -0.5f, Color.black, Time.fixedDeltaTime);
            }
            float newAngle = Vector2.Angle(closerHit.normal, Vector2.up);
            if(slopeAngle == 0 && newAngle != 0) {
                slopeAngle = newAngle;
            }
            grounded = (closerHit.point.y - bounds.center.y) < 0 || grounded ? true : false;
            bumpingHead = (closerHit.point.y - (rb.position.y + bounds.extents.y)) > 0 || bumpingHead ? true : false;
            moveAmount.y = (closerHit.distance - bounds.extents.y - skinWidth) * directionY;
        }
    }

    void ClimbSlope(ref Vector3 amount, float angle) {
        float moveDistance = Mathf.Abs(amount.x);
        float climbVelocityY = Mathf.Sin(Mathf.Deg2Rad * angle) * moveDistance;
        if(amount.y <= climbVelocityY) {
            amount.y = climbVelocityY;
            amount.x = Mathf.Cos(Mathf.Deg2Rad * angle) * moveDistance * Mathf.Sign(amount.x);
            climbingSlope = true;
            grounded = true;
        }
    }

    void DescendSlope(ref Vector3 amount) {
        Bounds bounds = col.bounds;
        bounds.Expand(-2 * skinWidth);

        float directionX = Mathf.Sign(amount.x);
        Vector2 origin = directionX == -1 ? new Vector2(bounds.max.x, bounds.min.y) : new Vector2(bounds.min.x, bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1, collisionMask);

        Color color = hit ? Color.green : Color.cyan;
        if(debug) Debug.DrawRay(origin, Vector2.down, color, Time.fixedDeltaTime);

        if(hit) {
            slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle != 0 && slopeAngle <= playerData.maxSlopeAngle) {
                if(Mathf.Sign(hit.normal.x) == directionX) {
                    if(hit.distance - 2 * skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(amount.x)) {
                        float moveDistance = Mathf.Abs(amount.x);
                        float climbVelocityY = Mathf.Sin(Mathf.Deg2Rad * slopeAngle) * moveDistance;
                        amount.x = Mathf.Cos(Mathf.Deg2Rad * slopeAngle) * moveDistance * Mathf.Sign(amount.x);
                        amount.y -= climbVelocityY;
                        descendingSlope = true;
                        grounded = true;
                    }
                }
            }
        }
    }

    void ResetVariables() {
        grounded = false;
        bumpingHead = false;
        slopeAngleOld = slopeAngle;
        slopeAngle = 0;
        slopePoint = Vector2.zero;
        climbingSlope = false;
        descendingSlope = false;
    }

    public bool canUncrouch() {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, new Vector2(bounds.size.x, 0.4f + skinWidth), 0, Vector2.up, 0.4f, collisionMask);
        
        if(debug) {
            Color color = hit ? Color.green : Color.red;
            //Debug.DrawRay(new Vector2(bounds.min.x, bounds.max.y), Vector2.up * 0.4f, color, Time.fixedDeltaTime);
            //Debug.DrawRay(new Vector2(bounds.min.x, bounds.max.y + 0.4f), Vector2.right * bounds.size.x, color, Time.fixedDeltaTime);
            //Debug.DrawRay(new Vector2(bounds.max.x, bounds.max.y), Vector2.up * 0.4f, color, Time.fixedDeltaTime);
        }

        return !hit;
    }

    public void ToggleDebugMode() {
        debug = !debug;
    }

    public bool isGrounded() {
        return grounded;
    }
    public bool isBumpingHead() {
        return bumpingHead;
    }
    public int isTouchingWall() {
        return touchingWall ? wallDir : 0;
    }
    public float SlopeAngle() {
        return slopeAngle;
    }

    public void OverrideLookDirection(int dir) {
        if(dir == 0) {
            return;
        }
        lookDir = Mathf.Clamp(dir, -1, 1);
    }
}
