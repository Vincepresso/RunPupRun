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
    public HashSet<int> activeTrapTypes;
    public bool isStaggered;
    public Transform staggerSource;
    public float staggerForce;
    public float staggerTime;
    public bool gamePaused;
    public bool gameOver;

    // Private Access Modifier
    private bool isJumpInitiated;
    private float nextJumpTime = 0f;
    private bool isJumping;
    private bool canJump;
    void Start() {
        gamePaused = false;
        canJump = true;
        speedMultiplier = defaultMultiplier;
        jumpMultiplier = defaultMultiplier;
        activeItemTypes = new HashSet<int>();
        activeTrapTypes = new HashSet<int>();
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
        RoseItemEffect();
        ShieldItemEffect();
        BootsItemEffect();
        Fall();
        if(gameOver) {
            animator.SetBool("Stagger", true);
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }
    private void Run() {
        if(isGrounded && !isStaggered) {
            animator.SetBool("Run", true);
            animator.SetBool("IsGrounded", true);
            rb.velocity = new Vector2(speed * speedMultiplier, rb.velocity.y);
        } else if(!isStaggered) {
            animator.SetBool("Run", false);
            animator.SetBool("IsGrounded", false);
        }
    }
    private void Jump() {
        bool jump = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        if(isGrounded && jump && canJump && !isStaggered && !gamePaused) {
            isJumping = true;
            isGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce * jumpMultiplier));
            nextJumpTime = Time.time + jumpDelay;
            canJump = false;
            Invoke("ResetJump", jumpDelay);
        } else {
            isJumping = false;
        }
    }
    private void ResetJump() {
        canJump = true;
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
    private void RoseItemEffect() {
        if(isJumping && activeItemTypes.Contains(0)) {
            baobaogoSfx.PlayHighJumpSfx();
        } else if(isJumping && !activeItemTypes.Contains(0)) {
            baobaogoSfx.PlayJumpSfx();
        }
        if(activeItemTypes.Contains(0)) {
            roseParticle.SetActive(true);
        } else {
            roseParticle.SetActive(false);
        }
    }
    private void ShieldItemEffect() {
        if(isInvincible || activeItemTypes.Contains(1)) {
            shieldParticle.SetActive(true);
        } else {
            shieldParticle.SetActive(false);
        }
    }
    private void BootsItemEffect() {
        if(isGrounded && activeItemTypes.Contains(2)) {
            bootsParticle.SetActive(true);
        } else {
            bootsParticle.SetActive(false);
        }       
    }
    public void CobwebEnter(Cobweb cobweb) {
        if(!activeTrapTypes.Contains(cobweb.typeId)) {
            activeTrapTypes.Add(cobweb.typeId);
            speedMultiplier += cobweb.multiplier;
            GetComponent<Animator>().speed += cobweb.multiplier;
        }
    }
    public void CobwebExit(Cobweb cobweb) {
        if(activeTrapTypes.Contains(cobweb.typeId)) {
            speedMultiplier -= cobweb.multiplier;
            GetComponent<Animator>().speed -= cobweb.multiplier;
            activeTrapTypes.Remove(cobweb.typeId);
        }
    }

    public void Stagger() {
        StartCoroutine(StaggerCoroutine());
    }

    private IEnumerator StaggerCoroutine() {
        isStaggered = true;
        baobaogoSfx.PlayHitSfx();
        animator.SetBool("Stagger", true);
        rb.AddForceAtPosition(new Vector2(-1f * staggerForce, staggerForce), new Vector2(staggerSource.position.x, staggerSource.position.y));
        yield return new WaitForSeconds(staggerTime);
        animator.SetBool("Stagger", false);
        isStaggered = false;
    }

    void OnDestroy() {
        gameBegin = false;
        gamePaused = false;
        gameOver = false;
    }

}
