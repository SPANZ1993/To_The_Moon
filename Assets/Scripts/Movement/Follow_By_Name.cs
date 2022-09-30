using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_By_Name : Follow
{

    public string leadObjName;

    // Start is called before the first frame update
    protected void Start()
    {
        base.leadObj = GameObject.Find(leadObjName);
        base.Start();
    }
}
