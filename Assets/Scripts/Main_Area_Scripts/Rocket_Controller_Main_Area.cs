using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Controller_Main_Area : MonoBehaviour
{

    private bool startedLaunch = false;
    public bool launchComplete = false;

    Rocket_Tower_Manager rocketTowerManager;
    
    GameObject rocketLaunchWaypoint;
    GameObject rocketCamWaypoint;
    GameObject rocketParticleSystemObj;
    ParticleSystem rocketParticleSystem;
    ParticleSystem smokeParticleSystemLeft;
    ParticleSystem smokeParticleSystemRight;
    GameObject rocketLogo;

    GameObject camObj;
    Camera cam;
    Camera_Shake camShake;
    Camera_Tracking camTrack;
    Move_Camera camMove;

    int rocketTweenId;
    int rocketScaleTweenId;

    private Vector3 origRocketScale;
    private Vector3 origParticleSystemOffset;
    private Vector3 origLogoOffset;

    private Vector3 rocketCurPos;
    private Vector3 rocketPrevPos;
    private Vector3 rocketOrigPos;


    private Sound rocketRumbleSound;


    private bool startedMoveTween = false;
    private bool cameraMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        launchComplete = false;
        rocketTowerManager = GameObject.Find("Rocket_Tower").GetComponent<Rocket_Tower_Manager>();
        rocketLaunchWaypoint = GameObject.Find("Rocket_Launch_Waypoint");
        rocketCamWaypoint = GameObject.Find("Rocket_Cam_Waypoint");
        rocketParticleSystemObj = GameObject.Find("Rocket_Particles");
        rocketParticleSystem = rocketParticleSystemObj.GetComponent<ParticleSystem>();
        smokeParticleSystemLeft = GameObject.Find("Smoke_Particles_Left").GetComponent<ParticleSystem>();
        smokeParticleSystemRight = GameObject.Find("Smoke_Particles_Right").GetComponent<ParticleSystem>();
        rocketLogo = GameObject.Find("Rocket_Logo");
        camObj = GameObject.Find("Main Camera");
        cam = camObj.GetComponent<Camera>();
        camShake = camObj.GetComponent<Camera_Shake>();
        camTrack = camObj.GetComponent<Camera_Tracking>();
        camMove = camObj.GetComponent<Move_Camera>();
    }

    // Update is called once per frame
    bool firstUpdate = true;
    void Update()
    {
        if(firstUpdate){
            //Debug.Log("FIRSTY");
            origRocketScale = transform.localScale;
            origParticleSystemOffset = transform.position - rocketParticleSystemObj.transform.position;
            origLogoOffset = transform.position - rocketLogo.transform.position;
            firstUpdate = false;
            rocketOrigPos = transform.position;
        }

        if(LeanTween.isTweening(rocketTweenId)){
            Vector3 inverseScale = new Vector3(transform.localScale.x/origRocketScale.x, transform.localScale.y/origRocketScale.y, transform.localScale.z/origRocketScale.z);
            rocketParticleSystemObj.transform.position = transform.position - Vector3.Scale(origParticleSystemOffset, inverseScale);
            rocketLogo.transform.position = transform.position - Vector3.Scale(origLogoOffset, inverseScale);
        }

        // if (rocketTowerManager.launched && !rocketTowerManager.launchComplete){
        //     camShake.shake(3f);
        // }
        if (rocketTowerManager.launchComplete && !startedLaunch){
            onRocketLaunchStarted(2f);
        }
        else if(startedLaunch && startedMoveTween){
            if (!LeanTween.isTweening(rocketTweenId)){
                launchComplete = true;
            }
        }

        if (cameraMoving){
            _moveCamera();
        }
    }



    void onRocketLaunchStarted(float shakeTime){
        if(!Audio_Manager.instance.IsPlaying("Rocket_Launch_Rumble")){
            Audio_Manager.instance.Play("Rocket_Launch_Rumble");
        }
        rocketParticleSystem.Play(true);
        smokeParticleSystemLeft.Play(true);
        smokeParticleSystemRight.Play(true);
        startedLaunch = true;
        camShake.shake(shakeTime);
        StartCoroutine(_onRocketLaunchStarted(shakeTime));
        StartCoroutine(_turnOffSmoke(shakeTime));
        startCameraMove();

        // LeanTween.value(rocketParticleSystem, 1.0f, 0.5f, 4f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(
        //     (value) =>
        //     {
        //         rocketParticleSystem.transform.localScale = new  Vector3(value, value, value);
        //     }
        // );

    }
    
    IEnumerator _onRocketLaunchStarted(float shakeTime){
        yield return new WaitForSeconds(shakeTime*.75f);
        
        
        rocketTweenId = LeanTween.moveY(gameObject, rocketLaunchWaypoint.transform.position.y, 4f).setEase(LeanTweenType.easeInQuad).id;
        rocketScaleTweenId = LeanTween.scale(gameObject, new Vector3(0.5f, 0.5f, 0.5f), 4f).setEase(LeanTweenType.easeInQuad).id;
        LeanTween.scale(rocketParticleSystemObj, new Vector3(0.5f, 0.5f, 0.5f), 4f).setEase(LeanTweenType.easeInQuad);
        LeanTween.scale(rocketLogo, new Vector3(0.5f, 0.5f, 0.5f), 4f).setEase(LeanTweenType.easeInQuad);
        startedMoveTween = true;
        // Tween Volume of Rocket Rumble
        Sound s = Audio_Manager.instance.GetSound("Rocket_Launch_Rumble");
        LeanTween.value(new GameObject(), Audio_Manager.instance.GetVolume(s), Audio_Manager.instance.GetVolume(s)*.1f, 4f).setEase(LeanTweenType.easeInOutSine).setOnUpdate(
            (value) =>
            {
                Audio_Manager.instance.SetVolume(s, value);
            }
        );
    }

    IEnumerator _turnOffSmoke(float shakeTime){
        yield return new WaitForSeconds(shakeTime*.75f);
        smokeParticleSystemLeft.Stop(true);
        smokeParticleSystemRight.Stop(true);
    }

    void startCameraMove(){
        camMove.enabled = false;
        camTrack.active = true;
        rocketCurPos = transform.position;
        rocketPrevPos = transform.position;

        //StartCoroutine(_moveCamera());
        cameraMoving = true;
    }

    void _moveCamera(){
        //yield return new WaitForSeconds(0);
        rocketPrevPos = rocketCurPos;
        rocketCurPos = transform.position;
        if (startedMoveTween && LeanTween.isTweening(rocketTweenId)){
            float rocketChangeY = rocketCurPos.y - rocketPrevPos.y;
            float curCamHeight = camTrack.upperLeft.y;
            float potentialNextHeight = rocketChangeY + curCamHeight;
            Vector3 potentialNextCamPos = new Vector3(camObj.transform.position.x, camObj.transform.position.y + rocketChangeY, camObj.transform.position.z);
            //Debug.Log("CAMPOS: " + curCamHeight + " CHANGE: " + rocketChangeY + " CAMPOS NEXT: " + rocketChangeY + curCamHeight + " POTENTIAL NEXT: " + potentialNextHeight);
            //Debug.Log("CAMPOS VECS: " + rocketPrevPos + " --- " + rocketCurPos + " --- " + potentialNextCamPos);
            //Debug.Log("POTENTIAL NEXT CAMPOS: " + potentialNextCamPos);
            if (potentialNextHeight <= rocketCamWaypoint.transform.position.y && !camShake.currentlyShaking && ((transform.position.y - rocketOrigPos.y)/(rocketCamWaypoint.transform.position.y - rocketOrigPos.y)) > 0.1f){
                //Debug.Log("CAMPOS AYooo");
                camObj.transform.position = potentialNextCamPos; 
                //camObj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

            }
        }
        //StartCoroutine(_moveCamera());
    }

}
