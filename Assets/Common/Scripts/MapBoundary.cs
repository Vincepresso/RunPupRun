using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoundary : MonoBehaviour {
    public Transform followTarget;
    void Start() {
        transform.position = followTarget.position;
    }
    void Update() {
        transform.position = new Vector3(followTarget.position.x, transform.position.y, transform.position.z);
    }
}
