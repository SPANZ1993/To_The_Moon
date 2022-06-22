using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Finder : MonoBehaviour
{
    public static GameObject findChildObjectByName(GameObject parent, string childName){
        foreach(Transform t in parent.transform){
            if(t.gameObject.name == childName){
                return t.gameObject;
            }
            else{
               GameObject found = findChildObjectByName(t.gameObject, childName);
               if(found != null){
                   return found;
               }
            }
        }
        return null;
    }
}
