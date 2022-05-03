using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scene_Transition: MonoBehaviour
{
    public delegate void LeavingSceneComplete(string nextScene, Scene_Transition instance);
    public static event LeavingSceneComplete LeavingSceneCompleteInfo;

    public delegate void EnteringSceneComplete();
    public static event EnteringSceneComplete EnteringSceneCompleteInfo;
    
    public abstract void BeginLeavingScene(string nextScene);
    public abstract void BeginEnteringScene();

    private UI_Controller uiController;
    protected static Dictionary<GameObject,Vector3> UIElementLocalScales;


    private void indexUILocalScaleSizes(){
        UIElementLocalScales = new Dictionary<GameObject, Vector3>();
        GameObject Wipe_Rect = GameObject.Find("Wipe_Rect");
        if (Wipe_Rect != null){
            UIElementLocalScales[Wipe_Rect] = Wipe_Rect.transform.localScale;
        }
    }


    protected void EnableUIElement(GameObject UI){
        uiController.EnableUIElement(UI, localScalesDict:UIElementLocalScales);
    }


    private void MakeAllTransitionElementsInvisible(){
        GameObject Wipe_Rect = GameObject.Find("Wipe_Rect");
        GameObject Circle_Wipe_Rect = GameObject.Find("Circle_Wipe_Rect");
        GameObject Circle_Wipe_Iris = GameObject.Find("Circle_Wipe_Iris");

        if (Circle_Wipe_Rect != null){
            Circle_Wipe_Rect.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (Circle_Wipe_Iris != null){
            Circle_Wipe_Iris.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (uiController != null){
            if (Wipe_Rect != null){
                //Wipe_Rect.GetComponent<Image>().enabled = false;
                uiController.DisableUIElement(Wipe_Rect);
            }
        }
    }

    public void Awake(){
        uiController = GameObject.Find("UI_Controller").GetComponent<UI_Controller>();
        if (UIElementLocalScales == null || true){
            indexUILocalScaleSizes();
        }
    }

    public void OnLevelWasLoaded(){
        indexUILocalScaleSizes();
        MakeAllTransitionElementsInvisible();
        BeginEnteringScene();
    }


    protected void _LeavingSceneComplete(string nextScene){
        if (LeavingSceneCompleteInfo != null){
            LeavingSceneCompleteInfo(nextScene, this);
        }
    }

    protected void _EnteringSceneComplete(){
        if (EnteringSceneCompleteInfo != null){
            EnteringSceneCompleteInfo();
        }
        Destroy(this);
    }
}
