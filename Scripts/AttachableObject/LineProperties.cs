using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineProperties : MonoBehaviour, IAttachableObject
{
    private static string TAG = "LineProperties"; 
    [SerializeField] protected float length;
    [SerializeField] protected float width;
    [SerializeField] protected float rad;

    void Start() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            Debug.LogError(TAG + " Sprite Renderer not assigned");
        } else {
            Vector2 spriteSize = spriteRenderer.size;
            length = spriteSize.x;
            width = spriteSize.y;
        }
        rad = Mathf.Deg2Rad*transform.localRotation.z;
        

    }
    void FixedUpdate() {
        transform.Rotate(0f,0f,1f);
    }
    public void move(Transform playerTransform, bool direction, float speed) {
        Vector3 playerLocation = playerTransform.localPosition;
        if (direction && playerLocation.x > -length/2) { //forward
            playerTransform.transform.localPosition = playerLocation - new Vector3(Mathf.Cos(rad),0) * speed;
        } else if (!direction && playerLocation.x < length/2) {
            playerTransform.transform.localPosition = playerLocation + new Vector3(Mathf.Cos(rad),0) * speed;
        }
    }
    public void attach(Transform playerTransform) {
        playerTransform.SetParent(transform);
        Vector2 playerLocation = playerTransform.localPosition;
        playerLocation.y = width/2;
        if (playerLocation.x > length/2) {
            playerLocation.x = length/2;
        }
        if (playerLocation.x < -length/2) {
            playerLocation.x = -length/2;
        } 
        playerTransform.localPosition=playerLocation; 
        Quaternion playerRotation = playerTransform.localRotation;
        playerRotation.z = 0;
        playerTransform.localRotation = playerRotation;
        //Vector3 adjustedScale = transform.localScale;
        //adjustedScale.x = playerTransform.localScale.x/adjustedScale.x; adjustedScale.y=playerTransform.localScale.y/adjustedScale.y;
        //playerTransform.localScale = adjustedScale;
    }

    
}
