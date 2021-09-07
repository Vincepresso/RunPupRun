using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baobaogo : MonoBehaviour {
    
    public float speed;
    private float speedMultiplier;
    public float jumpForce;
    private float jumpMultiplier;
    public float defaultMultiplier;
    public Animator animator;
    public Rigidbody2D rb;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundCheckLayerMask;
    private bool isJumpInitiated;
    private float nextJumpTime = 0f;
    public float jumpDelay = 1.5f;
    public float fallThreshold = -15f;
    public bool isInvincible;
    public float speedIncreaseFactor;
    public float speedIncreaseValue;
    public float speedIncreaseTime;
    public bool gameBegin;

    void Start() {
        speedMultiplier = defaultMultiplier;
        jumpMultiplier = defaultMultiplier;
        activeItemTypes = new HashSet<int>();
        InvokeRepeating("IncreaseSpeed", speedIncreaseTime, speedIncreaseTime);
        // StartRunning();
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

    // Handle audio here
    public AudioSource audioSource;
    public AudioClip footstepsSfx;
    public void PlayFootstepsSfx() {
        audioSource.PlayOneShot(footstepsSfx);
    }

    // Handle Items given to Baobaogo
    private HashSet<int> activeItemTypes;
    private IEnumerator itemEnumerator_0;
    private IEnumerator itemEnumerator_1;
    private IEnumerator itemEnumerator_2;
    public void ApplyItem(Item item) {
        if(item.data.typeId == 0) {
            HandleItemCoroutine_0(item);
        } else if(item.data.typeId == 1) {
            HandleItemCoroutine_1(item);
        } else if(item.data.typeId == 2) {
            HandleItemCoroutine_2(item);
        }
    }
    private void HandleItemCoroutine_0(Item item) {
        if(activeItemTypes.Contains(item.data.typeId)) {
            StopCoroutine(itemEnumerator_0);
        }
        activeItemTypes.Add(item.data.typeId);
        itemEnumerator_0 = ItemCoroutine_0(item);
        StartCoroutine(itemEnumerator_0);
    }
    private IEnumerator ItemCoroutine_0(Item item) {
        jumpMultiplier = item.data.multiplier;
        yield return new WaitForSeconds(item.data.effectiveTime);
        jumpMultiplier = defaultMultiplier;
        activeItemTypes.Remove(item.data.typeId);
        ItemEvents.current.ItemEffectFinished(item);
    }
    private void HandleItemCoroutine_1(Item item) {
        if(activeItemTypes.Contains(item.data.typeId)) {
            StopCoroutine(itemEnumerator_1);
        }
        activeItemTypes.Add(item.data.typeId);
        itemEnumerator_1 = ItemCoroutine_1(item);
        StartCoroutine(itemEnumerator_1);
    }
    private IEnumerator ItemCoroutine_1(Item item) {
        isInvincible = true;
        yield return new WaitForSeconds(item.data.effectiveTime);
        isInvincible = false;
        activeItemTypes.Remove(item.data.typeId);
        ItemEvents.current.ItemEffectFinished(item);
    }
    private void HandleItemCoroutine_2(Item item) {
        if(activeItemTypes.Contains(item.data.typeId)) {
            StopCoroutine(itemEnumerator_2);
        }
        activeItemTypes.Add(item.data.typeId);
        itemEnumerator_2 = ItemCoroutine_2(item);
        StartCoroutine(itemEnumerator_2);
    }
    private IEnumerator ItemCoroutine_2(Item item) {
        speedMultiplier = item.data.multiplier;
        yield return new WaitForSeconds(item.data.effectiveTime);
        speedMultiplier = defaultMultiplier;
        activeItemTypes.Remove(item.data.typeId);
        ItemEvents.current.ItemEffectFinished(item);
    }
}
