using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEvents : MonoBehaviour {
    public static ItemEvents current;
    private void Awake() {
        current = this;
    }

    public event Action<Item> onItemPickup;
    public event Action<Item> onItemEffectFinished;

    public void ItemPickup(Item item) {
        if(onItemPickup != null) {
            onItemPickup(item);
        }
    }
    public void ItemEffectFinished(Item item) {
        if(onItemEffectFinished != null) {
            onItemEffectFinished(item);
        }
    }
}
