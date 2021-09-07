using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public int mapId;
    public Transform[] itemPositions;
    public float itemSpawnProbability;
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("Player")) {
            SpawnEvents.current.MapSpawnEnter(this);
        } else if(collider.CompareTag("Mero")) {
            SpawnEvents.current.MapDespawnEnter(this);
        }
    }
}
