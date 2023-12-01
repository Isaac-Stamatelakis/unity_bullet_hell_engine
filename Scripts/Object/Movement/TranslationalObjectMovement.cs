using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationalObjectMovement : ObjectMovement
{
    [SerializeField] protected Vector2 speed;
    public Vector2 Speed {get{return this.speed;} set{speed=value;}}
    [SerializeField] protected Vector2 accerlation;
    public Vector2 Accerlation {get{return this.accerlation;} set{accerlation=value;}}

    protected Vector2 realSpeed;
    public Vector2 RealSpeed {set{realSpeed=value;}}
    protected Vector2 realAccerleration;
    public Vector2 RealAccerleration {set{realAccerleration=value;}}
    // Start is called before the first frame update
    protected override void move() {
        Vector2 position = transform.localPosition;
        position += realSpeed + realAccerleration * fixedUpdateCounter;
        transform.localPosition = position;
    }
}
