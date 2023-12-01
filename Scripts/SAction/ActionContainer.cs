using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Is read by a level controller
/// any child who is an action will be read by the level controller
/// </summary>
public class ActionContainer : MonoBehaviour
{
    public ReadableAction[] getActions() {
        return GetComponentsInChildren<ReadableAction>();
    }
}
