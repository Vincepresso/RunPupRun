using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    private float length;
    private float startPos;
    public GameObject target;
    public float parallaxRate;
    void Start() {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    void Update() {
        float temp = target.transform.position.x * (1 - parallaxRate);
        float dist = target.transform.position.x * parallaxRate;
        Vector3 newPosition = new Vector3(startPos + dist, transform.position.y, transform.position.z);
        transform.position = newPosition;
        if(temp > startPos + length / 2) {
            startPos += length;
        } else if(temp < startPos - length / 2) {
            startPos -= length;
        }
    }
}
