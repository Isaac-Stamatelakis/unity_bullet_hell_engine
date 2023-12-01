using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Signifies that a given class containing data is readable by level controller
/// </summary>
public abstract class ReadableAction : MonoBehaviour
{
    [Header("Game Tick to Execute On (50 ticks/sec)")]
    [SerializeField] public uint executeTick;
    public virtual void Start() {
        
    }
    public abstract void execute();
}
