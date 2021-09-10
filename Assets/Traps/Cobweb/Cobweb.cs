using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cobweb : MonoBehaviour {
    public int typeId;
    public Sprite diesSprite;
    public SpriteRenderer spriteRenderer;
    public float multiplier;
    public bool isInvincible;
    public float fadeTime;
    public void Dies() {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
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
        TrapEvents.current.CobwebEnter(collider.gameObject, this);        
    }
    void OnTriggerExit2D(Collider2D collider) {
        TrapEvents.current.CobwebExit(collider.gameObject, this);        
    }
}
