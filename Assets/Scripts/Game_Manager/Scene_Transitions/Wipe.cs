using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wipe : Scene_Transition
{

    // public static Wipe instance;

    GameObject Wipe_Rect;
    //GameObject Wipe_Rect_Fill;
    //float wipeTime = 1.5f;
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
        //Wipe_Rect_Fill = GameObject.Find("Wipe_Rect_Fill");
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


    // private void wipeStartOLD(string nextScene){
    //     string transitionClipName = "UI_Transition_Out";
    //     if(!Audio_Manager.instance.IsPlaying(transitionClipName)){
    //         Audio_Manager.instance.Play(transitionClipName);
    //     }

    //     void OnLeavingWipeComplete(){
    //         base._LeavingSceneComplete(nextScene);
    //     }
        
    //     Wipe_Rect = GameObject.Find("Wipe_Rect");
    //     base.EnableUIElement(Wipe_Rect);
    //     RectTransform rt = Wipe_Rect.GetComponent<RectTransform>();
    //     float startY = rt.anchoredPosition.y;
    //     Vector3 newPos;
    //     float wipeTime = Audio_Manager.instance.GetAudioSource(transitionClipName).clip.length;
    //     leavingWipeTweenId = LeanTween.value(Wipe_Rect, startY, 0.0f, wipeTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate(
    //         (value) => 
    //         {
    //             newPos = rt.anchoredPosition;
    //             newPos.y = value;
    //             rt.anchoredPosition = newPos;
    //         }
    //     ).id;
    //     LeanTween.descr(leavingWipeTweenId).setOnComplete(OnLeavingWipeComplete);
    // }




    private void wipeStart(string nextScene){
        //Debug.Log("STARTING WIPE");
        string transitionClipName = "UI_Transition_Out";
        if(!Audio_Manager.instance.IsPlaying(transitionClipName)){
            Audio_Manager.instance.Play(transitionClipName);
        }

        void OnLeavingWipeComplete(){
            base._LeavingSceneComplete(nextScene);
        }
        
        Wipe_Rect = GameObject.Find("Wipe_Rect");
        //base.EnableUIElement(Wipe_Rect);
        //RectTransform rt = Wipe_Rect.GetComponent<RectTransform>();
        //float startY = rt.anchoredPosition.y;
        //Vector3 newPos;
        Image Wipe_Image = Wipe_Rect.GetComponent<Image>();
        float wipeTime = Audio_Manager.instance.GetAudioSource(transitionClipName).clip.length;
        leavingWipeTweenId = LeanTween.value(Wipe_Rect, 0f, 1f, wipeTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate(
            (value) => 
            {
                //Debug.Log("WIPE START --- SETTING FILL AMOUNT TO: " +  value);
                Wipe_Image.fillAmount = value;
            }
        ).id;
        LeanTween.descr(leavingWipeTweenId).setOnComplete(OnLeavingWipeComplete);
    }





    // private void wipeEndOLD(){
    //     string transitionClipName = "UI_Transition_In";
    //     IEnumerator _startTransitionSoundNextFrame(){
    //         yield return new WaitForSeconds(0);
    //         //Debug.Log("STARTING TRANSITION SOUND!");
    //         if(!Audio_Manager.instance.IsPlaying(transitionClipName)){
    //             //Debug.Log("WE AREN'T PLAYING IT YET.. HERE WE GO");
    //             Audio_Manager.instance.Play(transitionClipName);
    //         }
    //     }
    //     StartCoroutine(_startTransitionSoundNextFrame());




    //     void OnEnteringWipeComplete(){
    //         base._EnteringSceneComplete();
    //     }

    //     Wipe_Rect = GameObject.Find("Wipe_Rect");
    //     base.EnableUIElement(Wipe_Rect);
    //     RectTransform rt = Wipe_Rect.GetComponent<RectTransform>();
    //     float startY = rt.anchoredPosition.y;
    //     Vector3 newPos;
    //     float wipeTime = Audio_Manager.instance.GetAudioSource(transitionClipName).clip.length;
    //     rt.anchoredPosition = new Vector3(rt.anchoredPosition.x, 0.0f, rt.anchoredPosition.y);
    //     enteringWipeTweenId = LeanTween.value(Wipe_Rect, 0.0f, startY, wipeTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate(
    //         (value) => 
    //         {
    //             newPos = rt.anchoredPosition;
    //             newPos.y = value;
    //             rt.anchoredPosition = newPos;
    //         }
    //     ).id;
    //     LeanTween.descr(enteringWipeTweenId).setOnComplete(OnEnteringWipeComplete);
    // }



    private void wipeEnd(){
        Wipe_Rect = null;
        string transitionClipName = "UI_Transition_In";
        IEnumerator _startTransitionSoundNextFrame(){
            yield return new WaitForSeconds(0);
            //Debug.Log("STARTING TRANSITION SOUND!");
            if(!Audio_Manager.instance.IsPlaying(transitionClipName)){
                //Debug.Log("WE AREN'T PLAYING IT YET.. HERE WE GO");
                Audio_Manager.instance.Play(transitionClipName);
            }
        }
        StartCoroutine(_startTransitionSoundNextFrame());




        void OnEnteringWipeComplete(){
            base._EnteringSceneComplete();
        }
        Wipe_Rect = GameObject.Find("Wipe_Rect");
        Image Wipe_Image = Wipe_Rect.GetComponent<Image>();
        float wipeTime = Audio_Manager.instance.GetAudioSource(transitionClipName).clip.length;
        Wipe_Image.fillAmount = 1f;
        enteringWipeTweenId = LeanTween.value(Wipe_Rect, 1f, 0f, wipeTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate(
            (value) => 
            {
                //Debug.Log("WIPE END --- SETTING FILL AMOUNT TO: " + value);
                Wipe_Image.fillAmount = value;
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
