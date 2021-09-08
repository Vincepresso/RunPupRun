using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaobaogoItem))]
[RequireComponent(typeof(BaobaogoSfx))]
public class Baobaogo : MonoBehaviour {

    // Public Access Modifier
    public float speed;
    public float speedMultiplier;
    public float jumpForce;
    public float jumpMultiplier;
    public float defaultMultiplier;
    public Animator animator;
    public Rigidbody2D rb;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundCheckLayerMask;
    public float jumpDelay = 1.5f;
    public float fallThreshold = -15f;
    public bool isInvincible;
    public float speedIncreaseFactor;
    public float speedIncreaseValue;
    public float speedIncreaseTime;
    public bool gameBegin;
    public HashSet<int> activeItemTypes;
    public BaobaogoItem baobaogoItem;
    public BaobaogoSfx baobaogoSfx;
    public GameObject bootsParticle;
    public GameObject shieldParticle;
    public GameObject roseParticle;
    public float bounceForce;
    public Transform bounceSource;
    public bool passOut;

    // Private Access Modifier
    private bool isJumpInitiated;
    private float nextJumpTime = 0f;

    void Start() {
        speedMultiplier = defaultMultiplier;
        jumpMultiplier = defaultMultiplier;
        activeItemTypes = new HashSet<int>();
        passOut = false;
        gameBegin = false;
        InvokeRepeating("IncreaseSpeed", speedIncreaseTime, speedIncreaseTime);
    }
    void Update() {
        isGrounded = false;
        isGrounded = IsTouchGround();
        if(gameBegin && !passOut) {
            Run();
            Jump();
        }
        ShieldParticle();
        // RoseParticle();
        Fall();
    }
    private void Run() {
        if(isGrounded) {
            animator.SetBool("Run", true);
            animator.SetBool("IsGrounded", true);
            rb.velocity = new Vector2(speed * speedMultiplier, rb.velocity.y);
            if(activeItemTypes.Contains(2)) {
                bootsParticle.SetActive(true);
            } else {
                bootsParticle.SetActive(false);
            }
        } else {
            bootsParticle.SetActive(false);
            animator.SetBool("Run", false);
            animator.SetBool("IsGrounded", false);
        }
    }
    private void Jump() {
        bool jump = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        if(isGrounded && jump && Time.time > jumpDelay) {
            isGrounded = false;
            if(activeItemTypes.Contains(0)) {
                roseParticle.SetActive(true);
                baobaogoSfx.PlayHighJumpSfx();
            } else {
                roseParticle.SetActive(false);
                baobaogoSfx.PlayJumpSfx();
            }
            rb.AddForce(new Vector2(0f, jumpForce * jumpMultiplier));
            nextJumpTime = Time.time + jumpDelay;
        }
    }
    private void Fall() {
        if(transform.position.y <= fallThreshold) {
            ActorEvents.current.CliffEnter(gameObject);
        }
    }
    private bool IsTouchGround() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundCheckLayerMask);
        for(int i = 0; i < colliders.Length; i++) {
            if(colliders[i].gameObject != gameObject) {
                return true;
            }
        }
        return false;
    }
    private void IncreaseSpeed() {
        speed += speedIncreaseFactor * speedIncreaseValue;
        Debug.Log(this.name + " speed is now " + speed);
    }
    public void ApplyItem(Item item) {
        baobaogoSfx.PlayItemPickupSfx();
        baobaogoItem.ApplyItem(item);
    }
    public void BounceForward() {
        rb.AddForceAtPosition(new Vector2(bounceForce, bounceForce), new Vector2(bounceSource.position.x, bounceSource.position.y));
    }
    private void ShieldParticle() {
        if(isInvincible || activeItemTypes.Contains(1)) {
            shieldParticle.SetActive(true);
        } else {
            shieldParticle.SetActive(false);
        }
    }
    private void RoseParticle() {
        if(isGrounded && activeItemTypes.Contains(0)) {
            roseParticle.SetActive(false);
        }
    }
}
