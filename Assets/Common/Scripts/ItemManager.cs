using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    public static ItemManager current;
    public GameObject baobaogo;
    public GameObject[] itemSlotList;
    void Awake() {
        current = this;
    }
    void Start() {
        ItemEvents.current.onItemPickup += HandleItemPickup;
        ItemEvents.current.onItemEffectFinished += HandleItemEffectFinished;
    }
    public void HandleItemPickup(Item item) {
        baobaogo.GetComponent<Baobaogo>().ApplyItem(item);
        foreach(GameObject itemSlotObject in itemSlotList) {
            ItemSlot itemSlot = itemSlotObject.GetComponent<ItemSlot>();
            if(itemSlot.item != null && itemSlot.item.data.typeId == item.data.typeId) {
                itemSlot.UnuseSlot();
                itemSlot.UseSlot(item);
                return;
            }
        }
        foreach(GameObject itemSlotObject in itemSlotList) {
            ItemSlot itemSlot = itemSlotObject.GetComponent<ItemSlot>();
            if(itemSlot.item == null) {
                itemSlot.UseSlot(item);
                return;
            }
        }
    }
    public void HandleItemEffectFinished(Item item) {
        foreach(GameObject itemSlotObject in itemSlotList) {
            ItemSlot itemSlot = itemSlotObject.GetComponent<ItemSlot>();
            if(itemSlot.item != null && itemSlot.item.data.typeId == item.data.typeId) {
                itemSlot.UnuseSlot();
                return;
            }
        }
    }
    void OnDestroy() {
        ItemEvents.current.onItemPickup -= HandleItemPickup;
        ItemEvents.current.onItemEffectFinished -= HandleItemEffectFinished;
    }
}
