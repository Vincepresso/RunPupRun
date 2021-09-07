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

    // Private Access Modifier
    private bool isJumpInitiated;
    private float nextJumpTime = 0f;

    void Start() {
        speedMultiplier = defaultMultiplier;
        jumpMultiplier = defaultMultiplier;
        activeItemTypes = new HashSet<int>();
        InvokeRepeating("IncreaseSpeed", speedIncreaseTime, speedIncreaseTime);
    }
    void Update() {
        isGrounded = false;
        isGrounded = IsTouchGround();
        if(isGrounded && animator.GetBool("Run")) {
            rb.velocity = new Vector2(1f * speed * speedMultiplier, rb.velocity.y);
        }
        bool jump = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        if (isGrounded && jump && Time.time > jumpDelay && gameBegin) {
            isGrounded = false;
            if(activeItemTypes.Contains(0)) {
                baobaogoSfx.PlayHighJumpSfx();
            } else {
                baobaogoSfx.PlayJumpSfx();
            }
            rb.AddForce(new Vector2(0f, jumpForce * jumpMultiplier));
            nextJumpTime = Time.time + jumpDelay;
        }
        PlayerFall();
    }
    public void StartRunning() {
        if(gameBegin) {
            animator.SetBool("Run", true);
        }
    }
    public void PlayerFall() {
        if(transform.position.y <= fallThreshold) {
            ActorEvents.current.CliffEnter(gameObject);
        }
    }
    public bool IsTouchGround() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundCheckLayerMask);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                StartRunning();
                animator.SetBool("IsGrounded", true);
                return true;
            }
        }
        animator.SetBool("Run", false);
        animator.SetBool("IsGrounded", false);
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
}
