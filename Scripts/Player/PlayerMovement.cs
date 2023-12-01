using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    protected int attachableLayer;
    protected const float spinMovementSpeed = 0.05f;
    protected const float attachRange = 0.75f;
    
    protected IAttachableObject attachedObject;
    // Start is called before the first frame update
    void Start()
    {
        attachableLayer = 1 << LayerMask.NameToLayer("Attachable");
        raycastRing();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            raycastRing();
        }
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A)) {
            if (attachedObject != null) {
                attachedObject.move(transform,true,spinMovementSpeed);
            }

        }
        if (Input.GetKey(KeyCode.D)) {
            if (attachedObject != null) {
                attachedObject.move(transform,false,spinMovementSpeed);
            }
            
        }
        
    }

    
    protected void raycastRing() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attachRange, Vector2.zero, Mathf.Infinity, attachableLayer);
        foreach (RaycastHit2D hit in hits) {
            IAttachableObject hitAttachable = hit.collider.gameObject.GetComponentInChildren<IAttachableObject>();
            if (hitAttachable != null && hitAttachable != attachedObject) {
                hitAttachable.attach(transform);
                attachedObject = hitAttachable;
                break; 
            }
            
        }
    }
    
}
