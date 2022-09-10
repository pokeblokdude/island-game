using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController2D))]
public class Bomb : MonoBehaviour {

    [SerializeField] float fuseDuration = 7;
    [SerializeField] float explosionRadius = 2.5f;
    [SerializeField] float flashInterval = 0.5f;
    [SerializeField] LayerMask targets;
    [SerializeField] int damageToEntities = 5;

    [SerializeField] Sprite flashOff;
    [SerializeField] Sprite flashOn;

    WorldObject worldObject;
    SpriteRenderer sr;
    PolygonCollider2D col;
    CombatDamageDealer damageDealer;
    GameObject flame;

    float fuseTime;
    
    bool lit = false;
    int flashing = 0;

    void Awake() {
        worldObject = GetComponent<WorldObject>();
        sr = GetComponent<SpriteRenderer>();
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

    public void Light() {
        fuseTime = fuseDuration;
        lit = true;
        flame.SetActive(true);
    }

    public void Throw(float velocity) {
        worldObject.SetVelX(velocity);
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
