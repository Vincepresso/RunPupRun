using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEvents : MonoBehaviour {
    public static SpawnEvents current;
    void Awake() {
        current = this;
    }
    public event Action<Map> onMapSpawnEnter;
    public event Action<Map> onMapDespawnEnter;
    public void MapSpawnEnter(Map map) {
        if(onMapSpawnEnter != null) {
            onMapSpawnEnter(map);
        }
    }
    public void MapDespawnEnter(Map map) {
        if(onMapDespawnEnter != null) {
            onMapDespawnEnter(map);
        }
    }
}