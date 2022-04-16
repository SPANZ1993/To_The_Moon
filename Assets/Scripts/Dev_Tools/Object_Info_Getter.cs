using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Info_Getter
{
    public void ListProperties(Object obj, string label=""){
        foreach (System.Reflection.PropertyInfo prop in obj.GetType().GetProperties()){
            if (label == ""){
                Debug.Log("Property: " + prop);
            }
            else{
                Debug.Log(label + " Property: " + prop);
            }
        }
    }

    public void ListMethods(Object obj, string label=""){
        foreach (System.Reflection.MethodInfo meth in obj.GetType().GetMethods()){
            if (label == ""){
                Debug.Log("Method: " + meth);
            }
            else{
                Debug.Log(label + " Method: " + meth);
            }
        }
    }

    public void ListComponents(GameObject obj, string label=""){
        Component[] components = obj.GetComponents(typeof(Component));
        foreach(Component component in components) {
            if (label == ""){
                Debug.Log("Component: " + component);
            }
            else{
                Debug.Log(label + " Component: " + component);
            }
        }
    }
}
