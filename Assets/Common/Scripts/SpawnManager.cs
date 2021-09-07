using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public static SpawnManager current;
    public GameObject[] mapList;
    public GameObject[] itemList;
    private float spawnTotalWeight;
    public float mapLocationBase = 32f;
    public GameObject initMap;
    void Awake() {
        current = this;
    }
    void Start() {
        SpawnEvents.current.onMapSpawnEnter += SpawnNextMap;
        SpawnEvents.current.onMapDespawnEnter += DespawnPreviousMap;
        spawnTotalWeight = 0f;
        foreach(GameObject itemObject in itemList) {
            Item item = itemObject.GetComponent<Item>();
            spawnTotalWeight += item.data.spawnWeight;
        }
        SpawnNextMap(initMap.GetComponent<Map>());
    }
    public void SpawnNextMap(Map currentMap) {
        int mapListIndex = Random.Range(0, mapList.Length);
        // Debug.Log("Spawning mapList index: " + mapListIndex);
        GameObject nextMapObject = Instantiate(mapList[mapListIndex], new Vector3(currentMap.transform.position.x + mapLocationBase, 0, 0), Quaternion.identity);
        Map nextMap = nextMapObject.GetComponent<Map>();
        nextMap.mapId = currentMap.mapId + 1;
        SpawnItem(nextMap);
    }
    public void DespawnPreviousMap(Map currentMap) {
        foreach(GameObject otherMap in GameObject.FindGameObjectsWithTag("Map")) {
            if(otherMap.GetComponent<Map>().mapId <= (currentMap.mapId - 2)) {
                // Debug.Log("Despawning mapId: " + otherMap.GetComponent<Map>().mapId);
                Destroy(otherMap);
            }
        }
    }
    private void SpawnItem(Map nextMap) {
        float itemSpawnLikelyhood = Random.Range(0f, 100f);
        if(itemSpawnLikelyhood <= nextMap.itemSpawnProbability) {
            int itemPositionIndex = Random.Range(0, nextMap.itemPositions.Length);
            int itemListIndex = getItemListIndexByWeightedChance();
            // Debug.Log("------Spawning item: " + itemList[itemListIndex].name + "------");
            GameObject itemObject = Instantiate(itemList[itemListIndex], nextMap.itemPositions[itemPositionIndex].position, Quaternion.identity);
            itemObject.transform.SetParent(nextMap.itemPositions[itemPositionIndex]);
        }
    }
    private int getItemListIndexByWeightedChance() {
        int index = 0;
        int lastIndex = itemList.Length - 1;
        float totalWeight = spawnTotalWeight;
        while (index < lastIndex) {
            Item item = itemList[index].GetComponent<Item>();
            float randChance = Random.Range(0, totalWeight);
            // Debug.Log("r:" + randChance + "-t:" + totalWeight + "-i:" + item.data.spawnWeight + "-b:" + (randChance < item.data.spawnWeight));
            if(randChance < item.data.spawnWeight) {
                return index;
            } 
            totalWeight -= item.data.spawnWeight;
            index++;
        }
        return index;
    }
    void OnDestroy() {
        SpawnEvents.current.onMapSpawnEnter -= SpawnNextMap;
        SpawnEvents.current.onMapDespawnEnter -= DespawnPreviousMap;
    }
}
