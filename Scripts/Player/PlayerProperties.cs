using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == null) {
            return;
        }
        ObjectProperties objectProperties = collision.gameObject.GetComponent<ObjectProperties>();
        if (objectProperties.HitsPlayer) // Check for specific tag or other conditions
        {
            Debug.Log("hit");
        }
    }
}
