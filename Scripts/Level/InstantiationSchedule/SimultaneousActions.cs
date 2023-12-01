using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimultaneousActions {
    protected int executeTime;
    public int ExecuteTime {get{return executeTime;}}
    public List<ReadableAction> actions;
    public int Count {get{return actions.Count;}}
    public SimultaneousActions(List<ReadableAction> actions, int executeTick) {
        this.actions = actions;
        this.executeTime = executeTick;
    }
    public void executeAll() {
        foreach (ReadableAction readableAction in actions) {
            readableAction.execute();
        }
    }
    public void addAction(ReadableAction readableAction) {
        this.actions.Add(readableAction);
    }
    public List<ReadableAction> getSpawnActions() {
        List<ReadableAction> returnList = new List<ReadableAction>();
        foreach (ReadableAction action in actions) {
            if (action is SpawnAction) {
                returnList.Add(action);
            }
        }
        return returnList;
    }
    
}
