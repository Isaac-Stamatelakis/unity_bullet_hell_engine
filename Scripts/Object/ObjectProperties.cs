using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties : MonoBehaviour
{
    [SerializeField] protected Animation explosion;
    [SerializeField] protected bool hitsPlayer = true;
    
    public bool HitsPlayer {get{return hitsPlayer;} set{hitsPlayer=value;}}
    protected Colladable[] colladables;
    protected int lifespan;
    public int LifeSpan {get{return lifespan;} set{lifespan = value;}}
    
    protected int updateCounter = 0;
    protected int key = -1;
    public int Key {get{return key;} set{key = value;}}

    // Start is called before the first frame update
    void Start()
    {
        
        //CircleCollider2D circleCollider2D = gameObject.AddComponent<CircleCollider2D>();
        //Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        //rb.bodyType = RigidbodyType2D.Kinematic;
        //rb.useFullKinematicContacts=true;
        //colladables = GameObject.FindObjectsOfType<Colladable>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (updateCounter % 5 == 0) {
            checkRange();
            outOfBoundsTest();
        }
        updateCounter++;
        
        
    }

    protected void checkRange() {
        
        /*
        bool inRange = false;
        foreach (Colladable colladable in colladables) {
            Vector2 colladablePosition = colladable.gameObject.transform.position;
            if (Mathf.Abs(colladablePosition.x-transform.position.x) < 2 && Mathf.Abs(colladablePosition.y-transform.position.y) < 2) {
                inRange = true;
                break;
            } 
        }
        setColliderState(inRange);
        */
    }
    protected void outOfBoundsTest() {
        if (Mathf.Abs(transform.position.x) > 15 || Mathf.Abs(transform.position.y) > 10) {
            //GameObject.Destroy(gameObject);
            deactiveGameObject();
        }
    }


    protected void setColliderState(bool state) {
        GetComponent<CircleCollider2D>().enabled = state;
    }

    protected void deactiveGameObject() {
        ObjectSpawnerProperties objectSpawnerProperties = gameObject.GetComponent<ObjectSpawnerProperties>();
        PooledGameObjects.getInstance().deactiveGameObject(key,gameObject);
        if (objectSpawnerProperties != null) {
           
        } else {

        }
    }
}
