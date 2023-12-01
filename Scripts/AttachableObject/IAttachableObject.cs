using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttachableObject
{
    public void move(Transform transform, bool direction, float speed);
    public void attach(Transform transform);
    
    
}
