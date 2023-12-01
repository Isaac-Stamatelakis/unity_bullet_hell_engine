using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
/**
Only one of these should ever exist at the same time even though this is not strictly enforced by unity
**/
public class LevelController : MonoBehaviour
{   
    protected const int MaxInstantiationsPerUpdate = 2;
    protected const int MaxDestructionsPerUpdate = 2;
    protected PooledGameObjects pooledGameObjects;
    protected LinkedList<SimultaneousActions> actionSchedule = new LinkedList<SimultaneousActions>();
    protected LinkedList<InstatiationRequest> instatiationSchedule = new LinkedList<InstatiationRequest>();  
    protected LinkedListNode<InstatiationRequest> instationFirstNode;  
    protected LinkedListNode<SimultaneousActions> actionFirstNode;
    [SerializeField] protected int gameTick = 0;
    [SerializeField] protected bool reportInstantiationSchedule;
    [SerializeField] protected bool reportActionSchedule;
    // Start is called before the first frame update
    void Start()
    {
        readActions();
        prepareContainers();
        instatiationSchedule = InstantiationScheduleGenerator.generateSchedule(actionSchedule,MaxInstantiationsPerUpdate);
        initalizePools();
        executeEarlyInstatiations();        
    }
    // FixedUpdate is called 50 times/second
    void FixedUpdate()
    {
        executeInstantiationRequests();
        executeActions();
        gameTick++;
    }
    
    protected void executeActions() {
        if (actionFirstNode != null) {
            SimultaneousActions simultaneousActions = actionFirstNode.Value;
            if (simultaneousActions.ExecuteTime == gameTick) {
                simultaneousActions.executeAll();
                actionFirstNode = actionFirstNode.Next;
            }
        }
    }
    protected void executeInstantiationRequests() {
        if (instationFirstNode == null) {
            return;
        }
        InstatiationRequest instatiationRequest = instationFirstNode.Value;
        if (instatiationRequest.StartTime == gameTick) {
            pooledGameObjects.addToPool(instatiationRequest.Amount,instatiationRequest.Prefab);
        }
        if (gameTick == instatiationRequest.FinishTime) {
            instationFirstNode = instationFirstNode.Next;
            executeInstantiationRequests();
            return;
        }
        if (gameTick >= instatiationRequest.StartTime) {
            pooledGameObjects.instantiatePool(MaxInstantiationsPerUpdate,instatiationRequest.Prefab);
        }
    }
    protected void initalizePools() {
        instationFirstNode = instatiationSchedule.First;
        while (instationFirstNode != null) {
            InstatiationRequest instatiationRequest = instationFirstNode.Value;
            pooledGameObjects.initPool(instatiationRequest.Prefab);
            instationFirstNode = instationFirstNode.Next;
        }
    }
    protected void executeEarlyInstatiations() {
        instationFirstNode = instatiationSchedule.First;
        if (reportInstantiationSchedule) {
            string report = "Instantiation Schedule:\n";
            while (instationFirstNode != null) {
                InstatiationRequest instatiationRequest = instationFirstNode.Value;
                report += "|Spawn " + instatiationRequest.Prefab.name + " from [" +  instationFirstNode.Value.StartTime + "," + instationFirstNode.Value.FinishTime + "]|->";
                instationFirstNode = instationFirstNode.Next;
            }
            report += "|Complete|";
            Debug.Log(report);
        }
        foreach (InstatiationRequest instatiationRequest in instatiationSchedule) {
            if (instatiationRequest.StartTime < 0) {
                int amount = instatiationRequest.Amount;
                if (instatiationRequest.FinishTime > 0) {
                    amount -= MaxInstantiationsPerUpdate * (instatiationRequest.FinishTime);
                    instatiationRequest.StartTime=0;
                }
                pooledGameObjects.addToPool(amount,instatiationRequest.Prefab);
                pooledGameObjects.instantiatePool(amount,instatiationRequest.Prefab);
            }
        }
        instationFirstNode = instatiationSchedule.First;
        while (instationFirstNode.Value.StartTime < 0) {
            instationFirstNode = instationFirstNode.Next;
            instatiationSchedule.RemoveFirst();
        } 
        
        
    }

    /// <summary>
    /// reads all actions in actioncontainers and places then in dict
    /// </summary>
    protected void readActions() {
        ActionContainer[] actionContainers = GetComponentsInChildren<ActionContainer>();
        Dictionary<int, SimultaneousActions> actionHistogram = new Dictionary<int, SimultaneousActions>();
        foreach (ActionContainer actionContainer in actionContainers) {
            ReadableAction[] actions = actionContainer.getActions();
            foreach (ReadableAction action in actions) {
                int tick = (int) action.executeTick;
                if (actionHistogram.ContainsKey(tick)) {
                    actionHistogram[tick].addAction(action);
                } else {
                    actionHistogram.Add(tick,new SimultaneousActions(new List<ReadableAction> {action},tick));
                }
            }
            //GameObject.Destroy(actionContainer.gameObject);
        }
        List<int> keys = actionHistogram.Keys.ToList();
        keys.Sort();

        foreach (int key in keys) {
            actionSchedule.AddLast(actionHistogram[key]);
        }
        actionFirstNode = actionSchedule.First;
        if (reportActionSchedule) {
            string report = "Action Schedule:";
            while (actionFirstNode != null) {
                SimultaneousActions simultaneousActions = actionFirstNode.Value;
                report += "(At tick " + simultaneousActions.ExecuteTime + " execute: ";
                foreach (ReadableAction readableAction in simultaneousActions.actions) {
                    if (readableAction is SpawnAction) {
                        SpawnAction spawnAction = (SpawnAction) readableAction;
                        report += "|Spawn " + spawnAction.spawnAmount + " " + spawnAction.prefab.name + "|";
                    }
                    report += ")->";
                }
                report += "->(Complete)";
                Debug.Log(report);
                actionFirstNode = actionFirstNode.Next;
            }
        }
        actionFirstNode = actionSchedule.First;
    }
    protected void prepareContainers() {
        GameObject pooledGameObjectContainer = new GameObject();
        pooledGameObjectContainer.name = "PooledGameObjects";
        pooledGameObjectContainer.transform.SetParent(transform);

        GameObject activeGameObjectContainer = new GameObject();
        activeGameObjectContainer.name = "ActiveGameObjects";
        activeGameObjectContainer.transform.SetParent(transform);

        pooledGameObjects = PooledGameObjects.getInstance();
    }    
}

