using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController2D))]
public class Bomb : MonoBehaviour {
    
    [SerializeField] EntityData data;
    [SerializeField] float fuseDuration = 7;
    [SerializeField] float explosionRadius = 2.5f;
    [SerializeField] float flashInterval = 0.5f;
    [SerializeField] LayerMask targets;
    [SerializeField] int damageToEntities = 5;

    [SerializeField] Sprite flashOff;
    [SerializeField] Sprite flashOn;

    SpriteRenderer sr;
    EntityController2D controller;
    PolygonCollider2D col;
    CombatDamageDealer damageDealer;
    GameObject flame;

    Vector3 wishVel;
    Vector3 actualVel;
    bool useGravity = true;
    bool grounded = false;
    float fuseTime;
    
    bool lit = false;
    int flashing = 0;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        controller = GetComponent<EntityController2D>();
        col = GetComponent<PolygonCollider2D>();
        damageDealer = GetComponent<CombatDamageDealer>();
        flame = transform.GetChild(0).gameObject;
    }

    void Start() {
        Light();
    }

    void Update() {
        if(lit) {
            fuseTime -= Time.deltaTime;
            if(fuseTime <= fuseDuration * 0.65f && flashing == 0) {
                StartCoroutine(Flash());
                flashing = 1;
            }
            if(fuseTime <= 0) {
                Explode();
            }
        }
    }

    IEnumerator Flash() {
        if(flashing == 1 && fuseTime <= fuseDuration * 0.25f) {
            flashing = 2;
            flashInterval /= 2;
        }
        sr.sprite = flashOff;
        yield return new WaitForSeconds(flashInterval);
        sr.sprite = flashOn;
        yield return new WaitForSeconds(flashInterval);
        StartCoroutine(Flash());
    }

    void FixedUpdate() {
        // GRAVITY
        if(useGravity) {
            if(grounded || controller.isBumpingHead()) {
                wishVel.y = -data.gravity * Time.fixedDeltaTime;
            }
            else if(actualVel.y >= data.maxFallSpeed && !grounded) {
                wishVel.y = wishVel.y - (data.gravity * Time.fixedDeltaTime);
            }
        }

        actualVel = controller.Move(wishVel * Time.fixedDeltaTime);
        grounded = controller.isGrounded();
    }

    public void Light() {
        fuseTime = fuseDuration;
        lit = true;
        flame.SetActive(true);
    }

    public void Throw(float velocity) {
        wishVel.x = velocity;
    }

    void Explode() {
        print("bomb exploded");
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position + (Vector3)Mathc.Average(col.points), explosionRadius, targets);
        if(objects.Length != 0) {
            foreach(Collider2D obj in objects) {
                CombatTarget t = obj.GetComponent<CombatTarget>();
                if(t != null) {
                    damageDealer.ApplyDamageToTarget(t);
                }
                else {
                    BombBreakable surface = obj.GetComponent<BombBreakable>();
                    if(surface != null) {
                        surface.Break();
                    }
                }
            }
        }
        Destroy(gameObject);
    }
}
