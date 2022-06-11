using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

public class Checkbox_Group : ToggleGroup
{

    [SerializeField]
    private GameObject[] toggles;

    public List<Toggle> registeredToggles {get; private set;}

    void Awake(){
        registeredToggles = new List<Toggle>();
        foreach(GameObject t in toggles){
            Toggle curToggle = t.GetComponent<Toggle>();
            RegisterToggle(curToggle);
        }
    }

    public void RegisterToggle(Toggle toggle){
        base.RegisterToggle(toggle);
        if(!registeredToggles.Contains(toggle)){
            registeredToggles.Add(toggle);
        }
    }

    public void UnregisterToggle(Toggle toggle){
        base.UnregisterToggle(toggle);
        if(registeredToggles.Contains(toggle)){
            registeredToggles.Remove(toggle);
        }
    }

    public void SelectCheckbox(Toggle toggle){
    
        if(registeredToggles.Contains(toggle)){
            foreach(Toggle curToggle in registeredToggles){
                if(curToggle == toggle){
                    curToggle.isOn = true;
                }
                else{
                    curToggle.isOn = false;
                }
            }
        }
    }

    public Toggle? GetSelectedCheckbox(){
        foreach(Toggle curToggle in registeredToggles){
            if(curToggle.isOn){
                return curToggle;
            }
        }
        return null;
    }

}
