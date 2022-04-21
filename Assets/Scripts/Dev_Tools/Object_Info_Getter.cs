using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Info_Getter
{
    public static void ListProperties(System.Object obj, string label = "", System.Type type = null){
        Debug.Log("TYPE IS : " + type);
        if (type != null){
            obj = System.Convert.ChangeType(obj, type);
            Debug.Log("OBJECT IS: " + obj.GetType());
        }
        foreach (System.Reflection.PropertyInfo prop in obj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)){
            if (label == ""){
                Debug.Log("Property: " + prop);
            }
            else{
                Debug.Log(label + " Property: " + prop);
            }
        }
    }

    public static void ListMethods(System.Object obj, string label = "", System.Type type = null){
        if (type != null){
            obj = System.Convert.ChangeType(obj, type);
        }
        foreach (System.Reflection.MethodInfo meth in obj.GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)){
            if (label == ""){
                Debug.Log("Method: " + meth);
            }
            else{
                Debug.Log(label + " Method: " + meth);
            }
        }
    }

    public static void ListComponents(GameObject obj, string label=""){
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
