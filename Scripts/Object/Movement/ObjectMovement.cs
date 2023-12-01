using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectMovement : MonoBehaviour
{
    protected int fixedUpdateCounter;
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
    public virtual void FixedUpdate()
    {
        move();
        fixedUpdateCounter++;
    }
    protected abstract void move();
}
