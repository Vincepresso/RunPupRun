using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour {
    public static TrapManager current;
    public GameObject baobaogoObj;
    public GameObject meroObj;
    private Baobaogo baobaogo;
    private Mero mero;
    void Awake() {
        current = this;
    }
    void Start() {
        TrapEvents.current.onCobwebEnter += CobwebEnter;
        TrapEvents.current.onCobwebExit += CobwebExit;
        baobaogo = baobaogoObj.GetComponent<Baobaogo>();
        mero = meroObj.GetComponent<Mero>();
    }
    private void CobwebEnter(GameObject actor, Cobweb cobweb) {
        if(actor.CompareTag(baobaogoObj.tag)) {
            if(baobaogo.isInvincible && !cobweb.isInvincible) {
                cobweb.Dies();
            } 
            if(!baobaogo.isInvincible && !baobaogo.isStaggered) {
                cobweb.isInvincible = true;
                baobaogo.CobwebEnter(cobweb);
            }
        } else if(actor.CompareTag(meroObj.tag) && !cobweb.isInvincible) {
            cobweb.Dies();
        }
    }
    private void CobwebExit(GameObject actor, Cobweb cobweb) {
        if(actor.CompareTag(baobaogoObj.tag)) {
            if(!baobaogo.isInvincible && !baobaogo.isStaggered) {
                cobweb.isInvincible = false;
                baobaogo.CobwebExit(cobweb);
            }
        }
    }
    void OnDestroy() {
        TrapEvents.current.onCobwebEnter -= CobwebEnter;
        TrapEvents.current.onCobwebEnter -= CobwebExit;
    }

}
