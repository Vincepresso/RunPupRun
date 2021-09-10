using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvents : MonoBehaviour {
    public static EnemyEvents current;
        void Awake() {
        current = this;
    }
    public event Action<GameObject, Bird> onBirdEnter;
    public void BirdEnter(GameObject actor, Bird bird) {
        if(onBirdEnter != null) {
            onBirdEnter(actor, bird);
        }
    }
}
