using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMovement : MonoBehaviour {
    public float speed = 5f;
    void Start() {
        Time.timeScale = 1f;
    }
    void Update() {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y , transform.position.z);
    }
}
