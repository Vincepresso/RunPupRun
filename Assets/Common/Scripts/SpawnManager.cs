using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public static SpawnManager current;
    public GameObject[] mapList;
    public GameObject[] itemList;
    public GameObject[] trapList;
    public GameObject[] enemyList;
    private float itemSpawnTotalWeight;
    public float mapLocationBase = 32f;
    public GameObject initMap;
    void Awake() {
        current = this;
    }
    void Start() {
        SpawnEvents.current.onMapSpawnEnter += SpawnNextMap;
        SpawnEvents.current.onMapDespawnEnter += DespawnPreviousMap;
        itemSpawnTotalWeight = 0f;
        foreach(GameObject itemObject in itemList) {
            Item item = itemObject.GetComponent<Item>();
            itemSpawnTotalWeight += item.data.spawnWeight;
        }
        SpawnNextMap(initMap.GetComponent<Map>());
    }
    public void SpawnNextMap(Map currentMap) {
        currentMap.deactivateForBaobaogo = true;
        int mapListIndex = Random.Range(0, mapList.Length);
        // Debug.Log("Spawning mapList index: " + mapListIndex);
        GameObject nextMapObject = Instantiate(mapList[mapListIndex], new Vector3(currentMap.transform.position.x + mapLocationBase, 0, 0), Quaternion.identity);
        Map nextMap = nextMapObject.GetComponent<Map>();
        nextMap.mapId = currentMap.mapId + 1;
        SpawnItem(nextMap);
        SpawnTrap(nextMap);
        SpawnEnemy(nextMap);
    }
    public void DespawnPreviousMap(Map currentMap) {
        currentMap.deactivateForMero = true;
        foreach(GameObject otherMap in GameObject.FindGameObjectsWithTag("Map")) {
            if(otherMap.GetComponent<Map>().mapId <= (currentMap.mapId - 2)) {
                // Debug.Log("Despawning mapId: " + otherMap.GetComponent<Map>().mapId);
                Destroy(otherMap);
            }
        }
    }
    private void SpawnItem(Map nextMap) {
        float itemSpawnLikelihood = Random.Range(0f, 100f);
        if(itemSpawnLikelihood <= nextMap.itemSpawnProbability) {
            int itemPositionIndex = Random.Range(0, nextMap.itemPositions.Length);
            int itemListIndex = getItemListIndexByWeightedChance();
            // Debug.Log("------Spawning item: " + itemList[itemListIndex].name + "------");
            GameObject itemObject = Instantiate(itemList[itemListIndex], nextMap.itemPositions[itemPositionIndex].position, Quaternion.identity);
            itemObject.transform.SetParent(nextMap.itemPositions[itemPositionIndex]);
        }
    }
    private void SpawnTrap(Map nextMap) {
        float trapSpawnLikelihood = Random.Range(0f, 100f);
        if(trapSpawnLikelihood <= nextMap.trapSpawnProbability) {
            int trapPositionIndex = Random.Range(0, nextMap.trapPositions.Length);
            int trapListIndex = Random.Range(0, trapList.Length);
            GameObject trapObject = Instantiate(trapList[trapListIndex], nextMap.trapPositions[trapPositionIndex].position, Quaternion.identity);
            trapObject.transform.SetParent(nextMap.trapPositions[trapPositionIndex]);
        }
    }
    private void SpawnEnemy(Map nextMap) {
        for(int i = 0; i < nextMap.enemyPositions.Length; i++) {
            float enemySpawnLikehood = Random.Range(0f, 100f);
            if(enemySpawnLikehood <= nextMap.enemySpawnProbability) {
                int enemyListIndex = Random.Range(0, enemyList.Length);
                GameObject enemyObject = Instantiate(enemyList[enemyListIndex], nextMap.enemyPositions[i].position, Quaternion.identity);
                enemyObject.transform.SetParent(nextMap.enemyPositions[i]);
            }
        }
    }
    private int getItemListIndexByWeightedChance() {
        int index = 0;
        int lastIndex = itemList.Length - 1;
        float totalWeight = itemSpawnTotalWeight;
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
