using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lateral_Booster_Controller : MonoBehaviour
{

    Rocket_Control rocketControl;

    ParticleSystem LateralBoostL;
    ParticleSystem LateralBoostR;

    [SerializeField]
    float maxStartLifetime = 2.5f;

    float int_angle;

    // Start is called before the first frame update
    void Start()
    {
        rocketControl = GameObject.Find("Rocket").GetComponent<Rocket_Control>();

        LateralBoostL = GameObject.Find("Lat_Booster_Particles_L").GetComponent<ParticleSystem>();
        LateralBoostR = GameObject.Find("Lat_Booster_Particles_R").GetComponent<ParticleSystem>();

        // LateralBoostL.main.startLifetime = 0f;
        // LateralBoostR.main.startLifetime = 0f;
        SetStartLifetime(LateralBoostL, 0f);
        SetStartLifetime(LateralBoostR, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Upgrades_Manager.instance.upgradesUnlockedDict[Upgrade.Lateral_Boosters]){
            if(rocketControl != null && rocketControl.getUserHasControl()){
                SetLateralBoosters();
            }
            else{
                SetStartLifetime(LateralBoostL, 0f);
                SetStartLifetime(LateralBoostR, 0f);
            }
        }
    }


    void SetLateralBoosters(){

        int_angle = rocketControl.getIntAngle();
        if(int_angle == 0f || int_angle == 90f){
            // LateralBoostL.main.startLifetime = 0f;
            // LateralBoostR.main.startLifetime = 0f;
            SetStartLifetime(LateralBoostL, 0f);
            SetStartLifetime(LateralBoostR, 0f);
        }
        else if(int_angle > 0){
            // LateralBoostL.main.startLifetime = Mathf.Lerp(0f, maxStartLifetime, int_angle/45f);
            // LateralBoostR.main.startLifetime = 0f;
            SetStartLifetime(LateralBoostL, Mathf.Lerp(0f, maxStartLifetime, int_angle/45f));
            SetStartLifetime(LateralBoostR, 0f);
        }
        else if(int_angle < 0){
            // LateralBoostL.main.startLifetime = 0f;
            // LateralBoostR.main.startLifetime = Mathf.Lerp(0f, maxStartLifetime, -int_angle/45f);
            SetStartLifetime(LateralBoostL, 0f);
            SetStartLifetime(LateralBoostR, Mathf.Lerp(0f, maxStartLifetime, -int_angle/45f));
        }

    }

    // Need to do it this way because Unity is really cool
    void SetStartLifetime(ParticleSystem ps, float lt){
        ps.Stop();
        var psmain = ps.main;
        psmain.startLifetime = lt;
        ps.Play();
    }
}
