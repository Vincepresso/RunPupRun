using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorEvents : MonoBehaviour {
    public static ActorEvents current;
    private void Awake() {
        current = this;
    }
    public event Action<GameObject> onCliffEnter;
    public event Action<GameObject> onMeroTouch;
    public void CliffEnter(GameObject actor) {
        if(onCliffEnter != null) {
            onCliffEnter(actor);
        }
    }
    public void MeroTouch(GameObject actor) {
        if(onMeroTouch != null) {
            onMeroTouch(actor);
        }
    }
}
