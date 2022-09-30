using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Dark_Matter_Follow : Follow_By_Name
{

    float initialY;
    Vector3 pos;
    Quaternion q;

    // Start is called before the first frame update
    void Start()
    {
        q = new Quaternion();
        initialY = transform.position.y;
        base.Start();
    }

    void Update(){
        maintainYPos();
        maintainRotation();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
        maintainYPos();
        maintainRotation();
    }

    void maintainYPos(){
        pos = transform.position;
        pos.y = initialY;
        transform.position = pos;
    }

    void maintainRotation(){
        transform.rotation = q;
    }
}
