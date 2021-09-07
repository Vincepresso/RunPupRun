using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject {
    public int typeId;
    public float spawnWeight;
    public float effectiveTime;
    public float multiplier;
    public string[] descriptions;
}
