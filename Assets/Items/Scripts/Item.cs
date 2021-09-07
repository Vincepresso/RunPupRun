using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    public ItemData data;
    private Vector3 initialPosition;
    public float bobbingSpeed;
    private int bobbingDirection;
    void Start() {
        initialPosition = transform.position;
        bobbingDirection = Random.Range(0, 2) == 0 ? -1 : 1;
    }
    void Update() {
        transform.position = initialPosition + new Vector3(0f, Mathf.Sin(Time.time * bobbingSpeed) * bobbingDirection, 0f);
    }
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("Player")) {
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.SetActive(false);
            gameObject.transform.SetParent(null);
            ItemEvents.current.ItemPickup(this);
        }
    }
}
