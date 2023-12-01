using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ObjectSpawnInstruction {
    [Header("Prefab to Spawn")]
    public GameObject prefab;
    [Header("Game Ticks between Spawns (50 ticks/second)")]
    public int spawnRate;
    [Header("-value is inf spawn amount")]
    public int maxSpawns = -1;
    public int spawnAmount = 1;
    [Header("Amount to Pool\nPooling improves performance\nSet to max amount of active gameobjects")]
    public int poolAmount = 0;
    [Header("Angle change of each spawn in degrees")]
    public int spawnAngleChange = 0;
    
}
