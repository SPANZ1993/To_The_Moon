using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Onboarding_Event_Scriptable_Object", menuName = "ScriptableObjects/Events/Onboarding_Event_Scriptable_Object", order = 1)]
public class Onboarding_Event_Trigger_Scriptable_Object : Event_Trigger_Scriptable_Object
{
    void OnEnable(){
        Onboarding_Manager.OnboardingEndedInfo += onOnboardingEnded;
    }

    void OnDisable(){
        Onboarding_Manager.OnboardingEndedInfo -= onOnboardingEnded;
    }


    public override bool shouldTrigger(){
        bool shouldTrigger = base.shouldTrigger();
        if(shouldTrigger){
            if(Game_Manager.instance.metrics.numGameStartups == 1 || Game_Manager.instance.userDisplayName == null || Game_Manager.instance.coinName == null){ 
                shouldTrigger = true;
            }
            else{
                shouldTrigger = false;
            }
        }
        if(shouldTrigger){
            Debug.Log("Should trigger onboarding sequence");
        }
        return shouldTrigger;
    }

    public override void _trigger(){
        Debug.Log("TRIGGERING ONBOARDING SEQUENCE");
        Onboarding_Manager onboardingManager = Game_Manager.instance.gameObject.AddComponent<Onboarding_Manager>();
        onboardingManager.ExecuteOnboarding(2.5f);
    }

    public void onOnboardingEnded(){
        Debug.Log("ALERTING ONBOARDING SEQUENCE OVER");
        base._alertManagerOnEventEnd();
        // IEnumerable alertOnboardEnded(){
        //     yield return new WaitForSeconds(0.25f); // Wait just a smidge before starting another event if there is one
        //     base._alertManagerOnEventEnd();
        // }
        // Debug.Log("Alerting progression manager onboarding ended");
        // StartCoroutine(alertOnboardEnded());
    }
































}
