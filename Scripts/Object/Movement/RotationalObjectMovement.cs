using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationalObjectMovement : ObjectMovement
{
    [SerializeField] protected float rotationSpeed;
    public float RotationSpeed {set{rotationSpeed = value;}}
    [Header("Use <void> to center at 0,0\nUse<parent> to center on spawner\nUse <none> for spawn location")]
    [SerializeField] protected string targetName;
    public string TargetName {get{return targetName;}}
    protected Vector2 targetLocation;
    protected float rad;

    public override void Start()
    {
        base.Start();
        if (targetName == "<void>") {
            targetLocation = Vector2.zero;
        }
        getRad();
        

    }
    protected override void move()
    {
        rad += rotationSpeed;
        float radius = getDistanceFromTarget();
        Vector2 newPosition = radToPosition(rad,getDistanceFromTarget());
        transform.position = newPosition;

    }

    protected float getDistanceFromTarget() {
        return Mathf.Sqrt(Mathf.Pow(transform.position.x-targetLocation.x,2) + Mathf.Pow(transform.position.y-targetLocation.y,2));
    }

    protected float getRad() {
        float xDif = targetLocation.x-transform.position.x;
        float yDif = targetLocation.y-transform.position.y;
        if (xDif == 0f) {
            return Mathf.PI/2;
        }
        if (xDif > 0) {
            return Mathf.PI + Mathf.Atan((targetLocation.y-transform.position.y)/(targetLocation.x-transform.position.x));
        } else {
            return Mathf.Atan((targetLocation.y-transform.position.y)/(targetLocation.x-transform.position.x));
        }
        
    }
    protected Vector2 radToPosition(float radians, float radius) {
        return new Vector2(Mathf.Cos(radians)*radius,Mathf.Sin(radians)*radius);
    }
}
