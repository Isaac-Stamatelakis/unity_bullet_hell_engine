using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**

**/
public class SpawnAction : ReadableAction

{
    [Header("Prefab to Instatiate")]
    [SerializeField] public GameObject prefab;
    [Header("Number of Prefabs to Instatiate")]
    [SerializeField] public uint spawnAmount = 1;
    [Header("Distance from Spawn Point")]
    [SerializeField] public Vector2 spawnDistance = Vector2.zero;
    [Header("Angle Change in Degrees per Spawn")]
    [SerializeField] public int spawnAngleChange;
    public Vector3 spawnLocation;
    public override void Start()
    {
        spawnLocation = transform.position;
    }
    public override void execute()
    {
        List<GameObject> pooledGameObjects = PooledGameObjects.getInstance().activateGameObjects(prefab,(int)spawnAmount);
        for (int n = 0; n < spawnAmount; n ++) {
            GameObject pooledGameObject = pooledGameObjects[n];
            pooledGameObject.SetActive(true);
            pooledGameObject.transform.position = spawnLocation;
        }
        
    }
}
