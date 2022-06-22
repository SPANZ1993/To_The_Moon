using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Circle_Wipe : Scene_Transition
{

    GameObject Wipe_Rect;
    GameObject Wipe_Iris;
    float wipeTime = 1.5f;
    int leavingWipeTweenId;
    int enteringWipeTweenId;



    void Awake(){
        base.Awake();
        Wipe_Rect = GameObject.Find("Circle_Wipe_Rect");
        Wipe_Iris = GameObject.Find("Circle_Wipe_Iris");
    }


    

    private void wipeStart(string nextScene){
        string transitionClipName = "UI_Transition_Out";
        if(!Audio_Manager.instance.IsPlaying(transitionClipName)){
            Audio_Manager.instance.Play(transitionClipName);
        }


        void OnLeavingWipeComplete(){
            base._LeavingSceneComplete(nextScene);
        }

        Wipe_Rect = GameObject.Find("Circle_Wipe_Rect");
        Wipe_Iris = GameObject.Find("Circle_Wipe_Iris");
        Wipe_Rect.GetComponent<SpriteRenderer>().enabled = true;
        Wipe_Iris.GetComponent<SpriteRenderer>().enabled = true;
        // RectTransform rt = Wipe_Rect.GetComponent<RectTransform>();
        // float startY = rt.anchoredPosition.y;
        Vector3 newPos;
        Vector3 newSize = new Vector3(0f, 0f, 0f);
        // leavingWipeTweenId = LeanTween.value(Wipe_Rect, startY, 0.0f, wipeTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate(
        //     (value) => 
        //     {
        //         newPos = rt.anchoredPosition;
        //         newPos.y = value;
        //         rt.anchoredPosition = newPos;
        //     }
        // ).id;
        float wipeTime = Audio_Manager.instance.GetAudioSource(transitionClipName).clip.length;
        leavingWipeTweenId = LeanTween.scale(Wipe_Iris, newSize, wipeTime).id;
        LeanTween.descr(leavingWipeTweenId).setOnComplete(OnLeavingWipeComplete);
    }




    private void wipeEnd(){
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
            //Debug.Log("WIPE COMPLETE");
            base._EnteringSceneComplete();
        }

        Wipe_Rect = GameObject.Find("Circle_Wipe_Rect");
        Wipe_Iris = GameObject.Find("Circle_Wipe_Iris");
        Wipe_Rect.GetComponent<SpriteRenderer>().enabled = true;
        Wipe_Iris.GetComponent<SpriteRenderer>().enabled = true;
        // RectTransform rt = Wipe_Rect.GetComponent<RectTransform>();
        // float startY = rt.anchoredPosition.y;
        Wipe_Iris.transform.localScale = new Vector3(0f, 0f, 0f);
        Vector3 newSize = new Vector3(15f, 15f, 15f);
        if(SceneManager.GetActiveScene().name == "Main_Area"){
            newSize = Game_Manager.instance.circleWipeOpenSize;
        }
        // leavingWipeTweenId = LeanTween.value(Wipe_Rect, startY, 0.0f, wipeTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate(
        //     (value) => 
        //     {
        //         newPos = rt.anchoredPosition;
        //         newPos.y = value;
        //         rt.anchoredPosition = newPos;
        //     }
        // ).id;
        float wipeTime = Audio_Manager.instance.GetAudioSource(transitionClipName).clip.length;
        enteringWipeTweenId = LeanTween.scale(Wipe_Iris, newSize, wipeTime).id;
        LeanTween.descr(enteringWipeTweenId).setOnComplete(OnEnteringWipeComplete);
    }



    public override void BeginLeavingScene(string nextScene){
        wipeStart(nextScene);
    }

    public override void BeginEnteringScene(){
        wipeEnd();
    }

}
