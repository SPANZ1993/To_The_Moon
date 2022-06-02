using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onboarding_Manager : MonoBehaviour
{

    private GameObject nameInputBox;
    private GameObject screenTint;
    
    // Sequence Variables
    private bool waitingForNameSubmit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ExecuteOnboarding(float delay){
        StartCoroutine(_executeOnboarding(delay));
    }

    IEnumerator _executeOnboarding(float delay){
        yield return new WaitForSeconds(delay);

        nameInputBox = GameObject.Find("Name_Input_Box");
        screenTint = GameObject.Find("Screen_Tint");

        UI_Controller.instance.EnableUIElement(screenTint);
        UI_Controller.instance.EnableUIElement(nameInputBox);

        waitingForNameSubmit = true;
        StartCoroutine(_waitForNameSubmit());
        //TouchScreenKeyboard.Open("Name", keyboardType = TouchScreenKeyboardType.ASCIICapable;
    }

    private IEnumerator _waitForNameSubmit(){
        if(waitingForNameSubmit){
            yield return new WaitForSeconds(0);
        }
    }
}
