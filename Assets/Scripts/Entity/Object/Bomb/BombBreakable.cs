using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BombBreakable : MonoBehaviour {
    
    // TODO: wall destroy effects

    public void Break() {
        Destroy(gameObject);
    }

}
