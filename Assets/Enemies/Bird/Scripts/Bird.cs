using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {
    public float minSpeed;
    public float maxSpeed;
    public Rigidbody2D rb;
    private float speed;
    public SpriteRenderer spriteRenderer;
    public Sprite diesSprite;
    public float fadeTime;
    public float gravityScaleOnDie;
    void Start() {
        speed = Random.Range(minSpeed, maxSpeed);
    }
    void Update() {
        Move();
    }
    private void Move() {
        rb.velocity = new Vector2(-speed, 0f);
    }
    public void Dies() {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
        speed = 0f;
        rb.gravityScale = gravityScaleOnDie;
        spriteRenderer.sprite = diesSprite;
        StartCoroutine(FadeOnDie(fadeTime));
    }
    private IEnumerator FadeOnDie(float fadeTime) {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime) {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, t));
            spriteRenderer.color = newColor;
            yield return null;
        }
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collider) {
        EnemyEvents.current.BirdEnter(collider.gameObject, this);
    }
}
