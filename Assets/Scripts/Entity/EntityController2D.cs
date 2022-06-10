using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EntityController2D : MonoBehaviour {
    
    public LayerMask collisionMask;

    const float skinWidth = 0.015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public float maxSlopeAngle = 50;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D col;
    Rigidbody2D rb;
    RaycastOrigins raycastOrigins;

    bool grounded = false;
    bool bumpingHead = false;
    bool touchingWall = false;
    int wallDir = 0;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public Vector3 Move(Vector3 moveAmount) {
        // reset variables
        grounded = false;
        bumpingHead = false;

        // reset ray origins to line up with the hitbox
        UpdateRaycastOrigins();
        
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

        return moveAmount / Time.deltaTime;
    }

    void HorizontalCollisions(ref Vector3 moveAmount) {
        bool touchingWallTemp = false;
        int touchingWallCount = 0;

        // set the ray direction/length equal to the X wish velocity
        float directionX = Mathf.Sign(moveAmount.x);
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        // go through each horizontal ray, starting from the bottom
        for(int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red, Time.deltaTime);

            if(hit) {
                moveAmount.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
                touchingWallTemp = true;
                wallDir = touchingWallTemp ? (int)Mathf.Sign(hit.point.x - transform.position.x) : 0;
            }
            if(touchingWallTemp) {touchingWallCount++;}
        }
        touchingWall = touchingWallCount > 0;
    }

    void VerticalCollisions(ref Vector3 moveAmount) {

        // set the ray direction/length equal to the Y wish velocity
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        // go through each vertical ray, starting from the left.
        for(int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red, Time.deltaTime);

            if(hit) {
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                grounded = (hit.point.y - transform.position.y) < 0 || grounded ? true : false;
                bumpingHead = (hit.point.y - transform.position.y) > 0 || bumpingHead ? true : false;
            }
        }
    }

    void UpdateRaycastOrigins() {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing() {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public bool canUncrouch() {
        RaycastHit2D hit1 = Physics2D.Raycast(raycastOrigins.topLeft, Vector2.up, 0.5f, collisionMask);
        RaycastHit2D hit2 = Physics2D.Raycast(raycastOrigins.topRight, Vector2.up, 0.5f, collisionMask);
        Debug.DrawRay(raycastOrigins.topLeft, Vector2.up * 0.4f, Color.red, Time.deltaTime);
        Debug.DrawRay(raycastOrigins.topRight, Vector2.up * 0.4f, Color.red, Time.deltaTime);

        return !(hit1 || hit2);
    }

    struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
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

    public Vector2[] edgeMiddles() {
        Vector2[] list = {
            new Vector2(raycastOrigins.bottomLeft.x + col.bounds.extents.x, raycastOrigins.bottomLeft.y),
            new Vector2(raycastOrigins.bottomLeft.x, raycastOrigins.bottomLeft.y + col.bounds.extents.y),
            new Vector2(raycastOrigins.topLeft.x + col.bounds.extents.x, raycastOrigins.topLeft.y),
            new Vector2(raycastOrigins.topRight.x, raycastOrigins.bottomRight.y + col.bounds.extents.y)
        };
        return list;
    }

    public Vector2[] corners() {
        Vector2[] list = {
            raycastOrigins.bottomLeft,
            raycastOrigins.topLeft,
            raycastOrigins.topRight,
            raycastOrigins.bottomRight
        };
        return list;
    }
}
