using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{

    public static Singleton instance;
    public static int instanceID;

    void Awake()
    {

        if (!instance){
            instance = this;
            instanceID = gameObject.GetInstanceID();
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }
}
