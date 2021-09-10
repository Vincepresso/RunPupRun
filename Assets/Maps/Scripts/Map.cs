using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public int mapId;
    public Transform[] itemPositions;
    public Transform[] trapPositions;
    public Transform[] enemyPositions;
    public float itemSpawnProbability;
    public float trapSpawnProbability;
    public float enemySpawnProbability;
    public bool deactivateForBaobaogo;
    public bool deactivateForMero;
    void Start() {
        deactivateForBaobaogo = false;
        deactivateForMero = false;
    }
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("Player") && !deactivateForBaobaogo) {
            SpawnEvents.current.MapSpawnEnter(this);
        }
        if(collider.CompareTag("Mero") && !deactivateForMero) {
            SpawnEvents.current.MapDespawnEnter(this);
        }
    }
}
