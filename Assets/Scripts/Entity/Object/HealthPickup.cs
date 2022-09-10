using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other) {
        CombatTarget target = other.GetComponent<CombatTarget>();
        if(target != null) {
            target.Heal(1);
            Destroy(gameObject);
        }
    }

}
