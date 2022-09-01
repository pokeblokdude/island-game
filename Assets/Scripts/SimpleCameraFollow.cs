using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SimpleCameraFollow : MonoBehaviour {

    [SerializeField] Transform target;
    [SerializeField] Vector2 offset;
    PixelPerfectCamera ppc;

    void Start() {
        ppc = GetComponent<PixelPerfectCamera>();
    }

    void LateUpdate() {
        transform.position = new Vector3(target.transform.position.x + offset.x, target.transform.position.y + offset.y, transform.position.z);
        transform.position = ppc.RoundToPixel(transform.position);
    }
}
