using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaobaogoItem : MonoBehaviour {
    private Baobaogo baobaogo;
    private IEnumerator itemEnumerator_0;
    private IEnumerator itemEnumerator_1;
    private IEnumerator itemEnumerator_2;
    void Start() {
        baobaogo = GetComponent<Baobaogo>();
    }
    public void ApplyItem(Item item) {
        if(item.data.typeId == 0) {
            HandleItemCoroutine_0(item);
        } else if(item.data.typeId == 1) {
            HandleItemCoroutine_1(item);
        } else if(item.data.typeId == 2) {
            HandleItemCoroutine_2(item);
        }
    }
    //Handle Rose
    private void HandleItemCoroutine_0(Item item) {
        if(baobaogo.activeItemTypes.Contains(item.data.typeId)) {
            StopCoroutine(itemEnumerator_0);
        }
        baobaogo.activeItemTypes.Add(item.data.typeId);
        itemEnumerator_0 = ItemCoroutine_0(item);
        StartCoroutine(itemEnumerator_0);
    }
    private IEnumerator ItemCoroutine_0(Item item) {
        baobaogo.jumpMultiplier = item.data.multiplier;
        yield return new WaitForSeconds(item.data.effectiveTime);
        baobaogo.jumpMultiplier = baobaogo.defaultMultiplier;
        baobaogo.activeItemTypes.Remove(item.data.typeId);
        ItemEvents.current.ItemEffectFinished(item);
    }
    //Handle Shield
    private void HandleItemCoroutine_1(Item item) {
        if(baobaogo.activeItemTypes.Contains(item.data.typeId)) {
            StopCoroutine(itemEnumerator_1);
        }
        baobaogo.activeItemTypes.Add(item.data.typeId);
        itemEnumerator_1 = ItemCoroutine_1(item);
        StartCoroutine(itemEnumerator_1);
    }
    private IEnumerator ItemCoroutine_1(Item item) {
        baobaogo.isInvincible = true;
        yield return new WaitForSeconds(item.data.effectiveTime);
        baobaogo.isInvincible = false;
        baobaogo.activeItemTypes.Remove(item.data.typeId);
        ItemEvents.current.ItemEffectFinished(item);
    }
    //Handle Boots
    private void HandleItemCoroutine_2(Item item) {
        if(baobaogo.activeItemTypes.Contains(item.data.typeId)) {
            StopCoroutine(itemEnumerator_2);
        }
        baobaogo.activeItemTypes.Add(item.data.typeId);
        itemEnumerator_2 = ItemCoroutine_2(item);
        StartCoroutine(itemEnumerator_2);
    }
    private IEnumerator ItemCoroutine_2(Item item) {
        baobaogo.speedMultiplier = item.data.multiplier;
        baobaogo.GetComponent<Animator>().speed = item.data.multiplier;
        yield return new WaitForSeconds(item.data.effectiveTime);
        baobaogo.speedMultiplier = baobaogo.defaultMultiplier;
        baobaogo.GetComponent<Animator>().speed = baobaogo.defaultMultiplier;
        baobaogo.activeItemTypes.Remove(item.data.typeId);
        ItemEvents.current.ItemEffectFinished(item);
    }
}
