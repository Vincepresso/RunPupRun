using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
    public Item item;
    private Image image;
    private Color transparent;
    void Start() {
        image  = GetComponent<Image>();
        transparent = new Color(1, 1, 1, 0);
        UnuseSlot();
    }
    public void UseSlot(Item newItem) {
        image.sprite = newItem.GetComponent<SpriteRenderer>().sprite;
        item = newItem;
        image.color = Color.white;
    }
    public void UnuseSlot() {
        if(item != null || image.sprite != null) {
            image.sprite = null;
            image.color = transparent;
            if(item != null) {
                Destroy(item.gameObject);
            }
            item = null;
        }
    }
}
