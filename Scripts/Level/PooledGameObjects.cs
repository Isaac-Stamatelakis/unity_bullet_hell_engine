using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

///<summary>
/// Contains all pooled gameObjects
/// Can be referenced by using their prefab as a key
///</summary>
public class PooledGameObjects
{
    private static PooledGameObjects instance;
    protected Dictionary<int,GameObjectPool> pools = new Dictionary<int, GameObjectPool>();
    protected Dictionary<GameObject, int> gameObjectKeyMap = new Dictionary<GameObject, int>();
    protected static Transform poolContainer;
    protected static Transform activeGameObjectContainer;
    private PooledGameObjects() {
        poolContainer = GameObject.Find("PooledGameObjects").transform;
        activeGameObjectContainer = GameObject.Find("ActiveGameObjects").transform;
    }
    public static PooledGameObjects getInstance() {
        if (instance == null) {
            instance = new PooledGameObjects();
        }
        return instance;
    }
    /// <summary>
    /// Gets a list of inactive gameobjects to activate
    /// If this method cannot find gameobjects in the pool, it will create them which IS VERY BAD LAGGY
    /// <param name="key">Prefab key for pool</param>
    /// <param name="amount">Amount to remove</param>
    /// </summary>
    public List<GameObject> activateGameObjects(GameObject key, int amount) {
        return pools[gameObjectKeyMap[key]].GetGameObjects(amount);
    }
    /// <summary>
    /// Returns a gameobject back into the a pool
    /// <param name="key">Prefab key for pool</param>
    /// <param name="gameObject">GameObject to be returned to given pool</param>
    /// </summary>
    public void deactiveGameObject(int key, GameObject gameObject) {

        pools[key].putBack(gameObject);

        
    }
    /// <summary>
    /// Increase a pool size by given amount
    /// </summary>
    public bool addToPool(int poolAmount, GameObject prefab) {
        pools[gameObjectKeyMap[prefab]].changeSize(poolAmount);
        return true;
    }
    
    /// <summary>
    /// Initalizes a pool for given gameobject prefab
    /// integer key is assigned in order of added
    /// <param name="prefab">Prefab to be initalized into a pool</param>
    /// </summary>
    public bool initPool(GameObject prefab) {
        foreach (GameObjectPool gameObjectPool in pools.Values) {
            if (gameObjectPool.prefab == prefab) {
                return false;
            }
        } 
        int key = pools.Count;
        GameObjectPool newPool = new GameObjectPool(0,prefab,key);
        pools[key] = newPool;
        gameObjectKeyMap[prefab] = key;
        return true;
    }
    /// <summary>
    /// Instantiates amount in given pool
    /// </summary>
    public void instantiatePool(int amount,GameObject prefabKey) {
        pools[gameObjectKeyMap[prefabKey]].instantiate(amount);
    }

    public int getIntKey(GameObject prefab) {
        return gameObjectKeyMap[prefab];
    }
    
    protected class GameObjectPool {
        public GameObjectPool(int size, GameObject prefab, int integerKey) {
            this.desiredSize = size;
            this.prefab = prefab;
            this.key = integerKey;
        }
        protected int desiredSize = 0;
        public int DesiredSize {get{return desiredSize;}}
        public Queue<GameObject> inactiveGameObjects = new Queue<GameObject>();
        public int activeGameObjectCount = 0;
        public int RealSize {get{return inactiveGameObjects.Count + activeGameObjectCount;}}
        
        public GameObject prefab;
        protected int key;
        public int Key {get{return key;} set{key=value;}}
        public void changeSize(int amount) {
            desiredSize += amount;
        }

        public List<GameObject> GetGameObjects(int amount) {
            activeGameObjectCount += amount;
            List<GameObject> list = new List<GameObject>();
            while (inactiveGameObjects.Count > 0 && amount > 0) {
                GameObject toActivate = inactiveGameObjects.Dequeue();
                toActivate.transform.SetParent(activeGameObjectContainer);
                list.Add(toActivate);
                amount--;
            }
            if (amount == 0) {
                return list;
            }
            // BAD THIS IS WHAT WE WANT TO AVOID. VERY EXPENSIVE TO INSTANTIATE ON THE SPOT.
            desiredSize += amount;
            for (int n = 0; n < amount; n ++) {
                Debug.LogWarning("Pool too small     " + prefab.name);
                GameObject instantiated = GameObject.Instantiate(prefab);
                ObjectProperties objectProperties = instantiated.GetComponent<ObjectProperties>();
                objectProperties.Key=key;
                instantiated.transform.SetParent(activeGameObjectContainer);
                list.Add(instantiated);
            }
            return list;
        }
        
        public void instantiate(int amount) {
            for (int n = 0; n < amount; n++) {
                if (RealSize < desiredSize) {
                    GameObject instantiated = GameObject.Instantiate(prefab);
                    ObjectProperties objectProperties = instantiated.GetComponent<ObjectProperties>();
                    if (objectProperties == null) {
                        Debug.LogError("Attempted to spawn object with no object properties");
                        return;
                    }
                    objectProperties.Key=key;
                    instantiated.transform.SetParent(poolContainer);
                    instantiated.SetActive(false);
                    inactiveGameObjects.Enqueue(instantiated);
                }
            }
        }
        public void putBack(GameObject toReturn) {
            inactiveGameObjects.Enqueue(toReturn);
            toReturn.transform.SetParent(poolContainer);
            toReturn.SetActive(false);
        }
    }

    protected class MultiKeyMap<PooledGameObjects> {
        protected ILookup<object, PooledGameObjects> lookup;

        public IEnumerable<PooledGameObjects> GetValues(object key) {
            return lookup[key];
        }
    }
}
