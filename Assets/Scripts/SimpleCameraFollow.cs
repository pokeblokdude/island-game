using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SimpleCameraFollow : MonoBehaviour {

    public Transform target;

    const float ONE_SIXTEENTH = 0.0625f;
    PixelPerfectCamera ppc;

    void Start() {
        ppc = GetComponent<PixelPerfectCamera>();
    }

    void LateUpdate() {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        transform.position = ppc.RoundToPixel(transform.position);
    }
}
