using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mero : MonoBehaviour {
    public float speed;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundCheckLayerMask;
    public Rigidbody2D rb;
    public float speedIncreaseFactor;
    public float speedIncreaseValue;
    public float speedIncreaseTime;
    void Start() {
        InvokeRepeating("IncreaseSpeed", speedIncreaseTime, speedIncreaseTime);
    }
    void Update() {
        isGrounded = false;
        isGrounded = IsTouchGround();
        if(isGrounded) {
            rb.velocity = new Vector2(1f * speed, rb.velocity.y);
        }
    }

    public bool IsTouchGround() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundCheckLayerMask);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                return true;
            }
        }
        
        return false;
    }
    private void IncreaseSpeed() {
        speed += speedIncreaseFactor * speedIncreaseValue;
        Debug.Log(this.name + " speed is now " + speed);
    }
    void OnTriggerEnter2D(Collider2D collider) {
        ActorEvents.current.MeroTouch(collider.gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision) {
        ActorEvents.current.MeroTouch(collision.gameObject);
    }
}
