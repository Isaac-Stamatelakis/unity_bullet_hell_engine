using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiationRequest {
    public InstatiationRequest(int startTime, int finishTime,int amount, GameObject prefab) {
        this.startTime = startTime;
        this.finishTime = finishTime;
        this.amount = amount;
        this.prefab = prefab;
    }
    protected int amount;
    public int Amount {get{return amount;}}
    protected int startTime;
    public int StartTime {get{return startTime;} set{startTime=value;}}
    protected int finishTime;
    public int FinishTime {get{return finishTime;}}
    public int ActiveTicks {get{return finishTime-startTime;}}
    protected GameObject prefab;
    public GameObject Prefab {get{return prefab;}}
}
