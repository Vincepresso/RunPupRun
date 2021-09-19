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
    public Transform bounceSource;
    public float bounceForce;
    public Animator animator;
    public float staggerTime;
    public bool gameOver;
    public AudioSource rollAudioSource;
    public AudioSource laughAudioSource;
    public AudioSource meroIsClosedAudioSource;
    void Start() {
        gameOver = false;
        InvokeRepeating("IncreaseSpeed", speedIncreaseTime, speedIncreaseTime);
        StartMove();
    }
    void Update() {
        isGrounded = false;
        isGrounded = IsTouchGround();
        Run();
    }
    private void Run() {
        if(isGrounded && animator.GetBool("Move")) {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }
    private bool IsTouchGround() {
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
    public void Stagger() {
        StartCoroutine(StaggerEnumerator());
    }
    public IEnumerator StaggerEnumerator() {
        BounceBackward();
        StopMove();
        yield return new WaitForSeconds(staggerTime);
        StartMove();
    }
    public void BounceBackward() {
        rb.AddForceAtPosition(-1 * new Vector2(bounceForce, bounceForce), new Vector2(bounceSource.position.x, bounceSource.position.y));
    }
    public void StartMove() {
        animator.SetBool("Move", true);
    }
    public void StopMove() {
        animator.SetBool("Move", false);
    }
    void OnTriggerEnter2D(Collider2D collider) {
        ActorEvents.current.MeroTouch(collider.gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision) {
        ActorEvents.current.MeroTouch(collision.gameObject);
    }
}
