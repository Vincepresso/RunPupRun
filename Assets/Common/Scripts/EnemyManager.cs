using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager current;
    public Baobaogo baobaogo;
    public Mero mero;
    private void Awake() {
        current = this;
    }
    void Start() {
        EnemyEvents.current.onBirdEnter += BirdEnter;
    }

    public void BirdEnter(GameObject actor, Bird bird) {
        if(actor.CompareTag(baobaogo.tag)) {
            if(baobaogo.isInvincible) {
                bird.Dies();
            } else if(!baobaogo.isStaggered) {
                baobaogo.Stagger();
            }
        }
        if(actor.CompareTag(mero.tag)) {
            bird.Dies();
        }
    }
    void OnDestroy() {
        EnemyEvents.current.onBirdEnter -= BirdEnter;
    }
}
