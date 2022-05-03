using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wipe : Scene_Transition
{

    // public static Wipe instance;

    GameObject Wipe_Rect;
    float wipeTime = 1.5f;
    public int leavingWipeTweenId;
    public int enteringWipeTweenId;


    // public delegate void SerializationStarted();
    // public static event SerializationStarted SerializationStartedInfo;

    // // void OnLevelWasLoaded(){
    // //     Wipe_Rect = GameObject.Find("Wipe_Rect");
    // // }


    void Awake(){
        base.Awake();
        Wipe_Rect = GameObject.Find("Wipe_Rect");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void wipe(string nextScene, string direction="Up"){
        
    // }


    private void wipeStart(string nextScene){

        void OnLeavingWipeComplete(){
            base._LeavingSceneComplete(nextScene);
        }
        
        Wipe_Rect = GameObject.Find("Wipe_Rect");
        base.EnableUIElement(Wipe_Rect);
        RectTransform rt = Wipe_Rect.GetComponent<RectTransform>();
        float startY = rt.anchoredPosition.y;
        Vector3 newPos;
        leavingWipeTweenId = LeanTween.value(Wipe_Rect, startY, 0.0f, wipeTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate(
            (value) => 
            {
                newPos = rt.anchoredPosition;
                newPos.y = value;
                rt.anchoredPosition = newPos;
            }
        ).id;
        LeanTween.descr(leavingWipeTweenId).setOnComplete(OnLeavingWipeComplete);
    }




    private void wipeEnd(){

        void OnEnteringWipeComplete(){
            base._EnteringSceneComplete();
        }

        Wipe_Rect = GameObject.Find("Wipe_Rect");
        base.EnableUIElement(Wipe_Rect);
        RectTransform rt = Wipe_Rect.GetComponent<RectTransform>();
        float startY = rt.anchoredPosition.y;
        Vector3 newPos;
        rt.anchoredPosition = new Vector3(rt.anchoredPosition.x, 0.0f, rt.anchoredPosition.y);
        enteringWipeTweenId = LeanTween.value(Wipe_Rect, 0.0f, startY, wipeTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate(
            (value) => 
            {
                newPos = rt.anchoredPosition;
                newPos.y = value;
                rt.anchoredPosition = newPos;
            }
        ).id;
        LeanTween.descr(enteringWipeTweenId).setOnComplete(OnEnteringWipeComplete);
    }



    
    public override void BeginLeavingScene(string nextScene){
        wipeStart(nextScene);
    }

    public override void BeginEnteringScene(){
        wipeEnd();
    }


}
