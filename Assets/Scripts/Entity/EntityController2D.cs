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

    bool grounded = false;
    bool bumpingHead = false;
    bool touchingWall = false;
    int wallDir = 0;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    public Vector3 Move(Vector3 moveAmount) {
        // reset variables
        grounded = false;
        bumpingHead = false;
        
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
        // set the ray direction/length equal to the X wish velocity
        float directionX = Mathf.Sign(moveAmount.x);
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.right * directionX, rayLength, collisionMask);

        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(new Vector2(directionX == 1 ? bounds.max.x : bounds.min.x, bounds.min.y), Vector2.right * directionX * rayLength, color, Time.fixedDeltaTime);
        Debug.DrawRay(new Vector2(directionX == 1 ? bounds.max.x + rayLength : bounds.min.x - rayLength, bounds.max.y), Vector2.down * bounds.size.y, color, Time.fixedDeltaTime);
        Debug.DrawRay(new Vector2(directionX == 1 ? bounds.max.x : bounds.min.x, bounds.max.y), Vector2.right * directionX * rayLength, color, Time.fixedDeltaTime);

        if(hit) {
            moveAmount.x = (hit.distance - skinWidth) * directionX;
            //rayLength = hit.distance;
            touchingWall = true;
        }
        else {
            touchingWall = false;
        }
        wallDir = touchingWall ? (int)Mathf.Sign(hit.point.x - transform.position.x) : 0;
    }

    void VerticalCollisions(ref Vector3 moveAmount) {
        // set the ray direction/length equal to the Y wish velocity
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.up * directionY, rayLength, collisionMask);
        
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(new Vector2(bounds.min.x, directionY == 1 ? bounds.max.y : bounds.min.y), Vector2.up * directionY * rayLength, color, Time.fixedDeltaTime);
        Debug.DrawRay(new Vector2(bounds.min.x, directionY == 1 ? bounds.max.y + rayLength : bounds.min.y - rayLength), Vector2.right * bounds.extents.x, color, Time.fixedDeltaTime);
        Debug.DrawRay(new Vector2(bounds.max.x, directionY == 1 ? bounds.max.y : bounds.min.y), Vector2.up * directionY * rayLength, color, Time.fixedDeltaTime);

        if(hit) {
            moveAmount.y = (hit.distance - skinWidth) * directionY;
            rayLength = hit.distance;
            grounded = (hit.point.y - transform.position.y) < 0 || grounded ? true : false;
            bumpingHead = (hit.point.y - transform.position.y) > 0 || bumpingHead ? true : false;
        }
        
    }

    public bool canUncrouch() {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.up, 0.4f, collisionMask);
        
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(new Vector2(bounds.min.x, bounds.max.y), Vector2.up * 0.4f, color, Time.fixedDeltaTime);
        Debug.DrawRay(new Vector2(bounds.min.x, bounds.max.y + 0.4f), Vector2.right * bounds.size.x, color, Time.fixedDeltaTime);
        Debug.DrawRay(new Vector2(bounds.max.x, bounds.max.y), Vector2.up * 0.4f, color, Time.fixedDeltaTime);

        return !hit;
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
}
