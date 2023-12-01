using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// 
///</summary>
public class ScheduleRequest {
    protected int maxTick;
    public int MaxTick {get{return maxTick;} set{maxTick=value;}}
    protected int maxAmount;
    public int MaxAmount {get{return maxAmount;} set{maxAmount=value;}}
    protected GameObject prefab;
    public GameObject Prefab {get{return prefab;}}
    public ScheduleRequest(int maxTick, int amountRequired, GameObject prefab) {
        this.maxTick = maxTick;
        this.maxAmount = amountRequired;
        this.prefab = prefab;
    }
}
