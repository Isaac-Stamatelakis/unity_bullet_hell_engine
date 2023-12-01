using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerProperties : MonoBehaviour
{
    protected int tickCounter;
    [SerializeField] protected List<ObjectSpawnInstruction> spawnInstructions = new List<ObjectSpawnInstruction>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        foreach (ObjectSpawnInstruction objectSpawnInstruction in spawnInstructions) {
            if (tickCounter % objectSpawnInstruction.spawnRate == 0) {
                // Get game objects from pool
                List<GameObject> spawnedGameObjects = PooledGameObjects.getInstance().activateGameObjects(objectSpawnInstruction.prefab,objectSpawnInstruction.spawnAmount);
                int spawn = 0;
                foreach (GameObject spawnedGameObject in spawnedGameObjects) {
                    spawnedGameObject.SetActive(true);
                    spawnedGameObject.transform.position = transform.position;
                    TranslationalObjectMovement translationalObjectMovement = spawnedGameObject.GetComponent<TranslationalObjectMovement>();
                    if (translationalObjectMovement != null) {
                        Vector2 tempSpeed = translationalObjectMovement.Speed;
                        Quaternion rotation = Quaternion.Euler(0, 0,spawn*objectSpawnInstruction.spawnAngleChange);
                        tempSpeed = rotation*tempSpeed;
                        translationalObjectMovement.RealSpeed = tempSpeed;
                        Vector2 tempAcc = translationalObjectMovement.Accerlation;
                        tempAcc = rotation*tempAcc;
                        translationalObjectMovement.RealAccerleration = tempAcc;
                    }
                    spawn ++;
                }
            }
        }
        tickCounter++;
    }
    public void addSpawnInstruction(ObjectSpawnInstruction objectSpawnInstruction) {
        spawnInstructions.Add(objectSpawnInstruction);
    }
    public List<ObjectSpawnInstruction> getSpawnInstructions() {
        return this.spawnInstructions;
    }
}
