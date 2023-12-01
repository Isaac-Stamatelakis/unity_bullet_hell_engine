using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingProperties : MonoBehaviour, IAttachableObject

{
    [SerializeField] protected Vector2 movementRadius;
    [SerializeField] protected float currentRad = 0;
    [SerializeField] protected Vector2 size;
    protected float ratioXtoY;
    public void Start() {
        size = transform.localScale;
        ratioXtoY = size.x/size.y;
    }
    public void move(Transform playerTransform,bool direction,float speed) {
        speed = direction ? speed : -speed;
        currentRad += speed;
        currentRad = currentRad % (Mathf.PI*2);
        playerTransform.localPosition=radToPosition(currentRad,movementRadius);
    }
    public void attach(Transform playerTransform) {
        playerTransform.SetParent(transform.parent);
        movementRadius = new Vector2(size.x,size.y) * 2.4f;
        currentRad = Mathf.Atan2(playerTransform.localPosition.y*ratioXtoY,playerTransform.localPosition.x);
        Quaternion playerRotation = playerTransform.localRotation;
        playerRotation.z = 0;
        playerTransform.localRotation = playerRotation;
        playerTransform.localPosition = radToPosition(currentRad,movementRadius);
    }
    protected Vector3 radToPosition(float radians, Vector2 radii) {
        return new Vector3(Mathf.Cos(radians)*radii.x,Mathf.Sin(radians)*radii.y,transform.position.z);
    }


}
