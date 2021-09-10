using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEvents : MonoBehaviour {
    public static TrapEvents current;
    void Awake() {
        current = this;
    }
    public event Action<GameObject, Cobweb> onCobwebEnter;
    public event Action<GameObject, Cobweb> onCobwebExit;
    public void CobwebEnter(GameObject actor, Cobweb cobweb) {
        if(onCobwebEnter != null) {
            onCobwebEnter(actor, cobweb);
        }
    } 
    public void CobwebExit(GameObject actor, Cobweb cobweb) {
        if(onCobwebExit != null) {
            onCobwebExit(actor, cobweb);
        }
    }
}
