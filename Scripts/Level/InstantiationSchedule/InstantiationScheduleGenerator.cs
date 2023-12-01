using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationScheduleGenerator
{
    
    ///<summary>
    /// generates an instatiation schedule which minimizes prepared/pooled game objects subject to
    /// At maxTick, amount of prepared/pooled gameobjects is equal to amountRequired for any ScheduleRequest
    /// given the following simplying assumptions:
    /// i) Assume time when spawn amount is maximized is equal to maxSpawnAmount/(spawnRate*spawnAmount) for pool requests, execute time for prepare requests
    ///ii) Assume that the pool is empty when the request is made
    ///iii) Assume that every request uses all the instatiations at every tick its active
    /// <param name="levelActions">A collection of actions to generate schedule from</param>
    ///</summary>
    public static LinkedList<InstatiationRequest> generateSchedule(LinkedList<SimultaneousActions> levelActions, int MaxInstantiationsPerUpdate) {
        SortedRequestList scheduleRequests = generateRequest(levelActions,MaxInstantiationsPerUpdate);
        return generateScheduleFromRequests(scheduleRequests,MaxInstantiationsPerUpdate);
    }

    ///<summary>
    /// returns a list of schedule requests sorted by maxTick in descending order 
    ///</summary>
    protected static SortedRequestList generateRequest(LinkedList<SimultaneousActions> levelActions, int MaxInstantiationsPerUpdate) {
        SortedRequestList scheduleRequests = new SortedRequestList();
        LinkedListNode<SimultaneousActions> actionNode = levelActions.First;// values are sorted by execute time
        while (actionNode != null) {
            SimultaneousActions simultaneousActions = actionNode.Value;
            foreach (ReadableAction readableAction in simultaneousActions.actions) {
                if (readableAction is SpawnAction) {
                    // Pooling Spawn Action
                    SpawnAction spawnAction = (SpawnAction) readableAction;
                    
                    int tickMaxed = (int) spawnAction.executeTick;
                    scheduleRequests.add(new ScheduleRequest(tickMaxed,(int)spawnAction.spawnAmount,spawnAction.prefab));
                    ObjectSpawnerProperties objectSpawnerProperties = spawnAction.prefab.GetComponent<ObjectSpawnerProperties>();
                    if (objectSpawnerProperties != null) {
                        foreach (ObjectSpawnInstruction objectSpawnInstruction in objectSpawnerProperties.getSpawnInstructions()) {
                            // pool for Objects spawned from spawner
                            tickMaxed = objectSpawnInstruction.maxSpawns/(objectSpawnInstruction.spawnAmount*objectSpawnInstruction.spawnRate)+objectSpawnInstruction.spawnRate+(int) spawnAction.executeTick;
                            scheduleRequests.add(new ScheduleRequest(tickMaxed,objectSpawnInstruction.poolAmount,objectSpawnInstruction.prefab));
                        }
                    }
                }
            }
            actionNode = actionNode.Next;
        }
        return scheduleRequests;
    }

    ///<summary>
    /// Converts a descendingly SortedRequestList into a list of instatiation requests 
    ///</summary>
    public static LinkedList<InstatiationRequest> generateScheduleFromRequests(SortedRequestList scheduleRequests, int MaxInstantiationsPerUpdate) {
        List<InstatiationRequest> instatiationRequests = new List<InstatiationRequest>();
        foreach (ScheduleRequest scheduleRequest in scheduleRequests) {
            foreach (InstatiationRequest instatiationRequest in instatiationRequests) {
                if (scheduleRequest.MaxTick >= instatiationRequest.StartTime) { // Inside an interval
                    // Set time maximized to the first avaiable tick
                    scheduleRequest.MaxTick = instatiationRequest.StartTime;
                } 
            }
            int startTime = scheduleRequest.MaxTick - Mathf.CeilToInt((float)scheduleRequest.MaxAmount/MaxInstantiationsPerUpdate);
            int finishTime = scheduleRequest.MaxTick;
            instatiationRequests.Add(new InstatiationRequest(startTime,finishTime,scheduleRequest.MaxAmount,scheduleRequest.Prefab));
        }
        LinkedList<InstatiationRequest> sortedInstatiationRequests = new LinkedList<InstatiationRequest>();
        // Slow sorting algorithm but doesn't matter as this runs at start of game
        while (instatiationRequests.Count > 0) {
            int smallestIndex = 0;
            for (int n = 1; n < instatiationRequests.Count; n ++) {
                if (instatiationRequests[n].StartTime < instatiationRequests[smallestIndex].StartTime) {
                    smallestIndex = n;
                }
            }
            sortedInstatiationRequests.AddLast(instatiationRequests[smallestIndex]);
            instatiationRequests.RemoveAt(smallestIndex);
        }

        return sortedInstatiationRequests;

    }


    
    
    
}
