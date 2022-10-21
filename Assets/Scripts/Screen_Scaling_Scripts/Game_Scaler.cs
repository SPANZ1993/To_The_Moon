using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
using TMPro;

using System;
using System.Linq;


public class Game_Scaler : MonoBehaviour
{

    // If DebugMode then print info about the screen scaler
    public bool DebugMode = false;
    public float DevScreenWidthPx = 640f;
    public float DevScreenHeightPx = 1136f;

    // This is the screen size for iPhone 12
    public float DevScreenWidth = 4.620853f;
    public float DevScreenHeight = 10f;
    [HideInInspector]
    public float ScreenWidth, ScreenHeight, ScreenWidthPx, ScreenHeightPx;

    public float ScaleX, ScaleXPx;
    public float ScaleY, ScaleYPx;

    private List<String> Scaled_Scenes = new List<String>();

 
    private GameObject camobj;
    private Camera cam;


    //private

    public static Game_Scaler instance;
    public static int instanceID;

    // Start is called before the first frame update
    void Awake()
    {
        camobj = GameObject.Find("Main Camera");
        cam = camobj.GetComponent<Camera>();

        if (!instance){

            ScreenWidthPx = Screen.width;
            ScreenHeightPx = Screen.height;

            Camera cam = Camera.main;
            ScreenHeight = 2f * cam.orthographicSize;
            ScreenWidth = ScreenHeight * cam.aspect;


            if (DebugMode){
                Debug.Log("W " + ScreenWidth);
                Debug.Log("H " + ScreenHeight);
                Debug.Log("WPX " + ScreenWidthPx);
                Debug.Log("HPX " + ScreenHeightPx);
                Debug.Log("SCREEN RATIO: " + ScreenHeight/ScreenWidth);
                Debug.Log("SCREEN RATIO PX: " + ScreenHeightPx/ScreenWidthPx);
                Debug.Log("WDEV " + DevScreenWidth);
                Debug.Log("HDEV " + DevScreenHeight);
            }

            ScaleX = ScreenWidth / DevScreenWidth;
            ScaleY = ScreenHeight / DevScreenHeight;

            ScaleXPx = ScreenWidthPx / DevScreenWidthPx;
            ScaleYPx = ScreenHeightPx / DevScreenHeightPx;

            if (DebugMode){
                Debug.Log("SCALEX: " + ScaleX);
                Debug.Log("SCALEY: " + ScaleY);
            }
        
            instance = this;
            instanceID = gameObject.GetInstanceID();
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Start()
    {
        ScaleEverything();
    }

    void OnLevelWasLoaded()
    {
        camobj = GameObject.Find("Main Camera");
        cam = camobj.GetComponent<Camera>();
        ScaleEverything();
    }


    void ScaleEverything(){
        string id = gameObject.GetInstanceID().ToString();

        //if(Scaled_Scenes.Contains(SceneManager.GetActiveScene().name)
        if (gameObject.GetInstanceID() != instanceID){
            //Debug.Log(id + " NOT SCALING");
            return;
        }
        else{
            //Debug.Log(id + " SCALING");
            Scaled_Scenes.Add(SceneManager.GetActiveScene().name);
        }

        List<GameObject>canvases = new List<GameObject>();
        object[] obj = GameObject.FindSceneObjectsOfType(typeof (GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject) o;
            if (g.GetComponent<CanvasRenderer>() == null && g.GetComponent<Canvas>() == null){ // Don't scale UI Elements Since They Already Scale
                if (g.transform.childCount == 0 && g.name != "Reticle"){
                    ScaleObject(g);
                    //Debug.Log(g.name);
                }
                else{ 
                    //Debug.Assert(g.transform.localPosition == new Vector3(0f, 0f, 0f) || g.name == "Reticle");
                    if (g.transform.localPosition != new Vector3(0f, 0f, 0f) && g.name != "Reticle"){
                        // print(g.name + " Failed");
                    } 
                }
            }
            else if (g.GetComponent<Canvas>() != null){
                //Debug.Log("FOUND CANVAS!: " + g.name);
                canvases.Add(g);
            }
        }
        //ScaleUI(canvases);
    }

    struct ObjectsWithDepths
    {
        public ObjectsWithDepths(List<GameObject> objs, List<int> depths){
            Objs = objs;
            Depths = depths;
        }

        public List<GameObject> Objs {get; private set;}
        public List<int> Depths {get; private set;}
    }

    private ObjectsWithDepths FindAllChildren(GameObject parent){
        List<GameObject> children = new List<GameObject>();
        List<int> depths = new List<int>();
        ObjectsWithDepths owd = new ObjectsWithDepths(new List<GameObject>(), new List<int>());
        return _FindAllChildren(parent, 0);
    }

    private ObjectsWithDepths _FindAllChildren(GameObject parent, int curDepth=0)
    {
        //Debug.Log("CURDEPTH: " + curDepth);
        //Debug.Log("CHILDREN: " + ListToTextGameObj(children));
        ObjectsWithDepths owd = new ObjectsWithDepths(new List<GameObject>(), new List<int>());
        foreach (Transform child in parent.transform)
        {
            //Debug.Log("ADDING " + child.gameObject.name);
            owd.Objs.Add(child.gameObject);
            owd.Depths.Add(curDepth);
            //Debug.Log("CHILDREN INSIDE: " + ListToTextGameObj(children));
            ObjectsWithDepths sub_owd = _FindAllChildren(child.gameObject, curDepth+1);
            owd.Objs.AddRange(sub_owd.Objs);
            owd.Depths.AddRange(sub_owd.Depths);
            //Debug.Log("SUB OWD: " + ListToTextGameObj(sub_owd.Objs));
            //Debug.Log("NOW OWD: " + ListToTextGameObj(owd.Objs));
        }
        //Debug.Log("OUTSIDE OWD " + curDepth + " : " + ListToTextGameObj(owd.Objs) + " ... " + owd.Objs.Count);
        return owd;
    }
 

    private string ListToTextInt(List<int> list)
    {
        string result = "";
        foreach(var listMember in list)
        {
            result += listMember.ToString() + "\n";
        }
        return result;
    }

        private string ListToTextGameObj(List<GameObject> list)
    {
        string result = "";
        foreach(var listMember in list)
        {
            result += listMember.name + "\n";
        }
        return result;
    }


    void ScaleUI(List<GameObject>canvases){
        foreach(GameObject canvas in canvases)
        {  
            ObjectsWithDepths owd = FindAllChildren(canvas);
            print("OWD: " + ListToTextGameObj(owd.Objs));
            print("OWD: " + ListToTextInt(owd.Depths));
            for (int cur_depth = owd.Depths.Max(); cur_depth >= owd.Depths.Min(); cur_depth--){
                for (int i = 0; i < owd.Objs.Count; i++){
                    if (owd.Depths[i] == cur_depth){
                        ScaleUIObject(owd.Objs[i]);
                    }
                }
            }
        }
    }


    void ScaleUIObject(GameObject obj){
        // Debug.Log("SCALING UI: " + obj.name);
        if (obj.GetComponent<TextMeshProUGUI>() != null){
            //Debug.Log("GOT A TMP FOR THIS");
            TextMeshProUGUI TextMeshPro = obj.GetComponent<TextMeshProUGUI>();
        }
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        Vector2 orig_offsetMin = rectTransform.offsetMin;
        Vector2 orig_offsetMax = rectTransform.offsetMax;
        // Debug.Log("OFFSET MIN: " + orig_offsetMin);
        // Debug.Log("OFFSET MAX: " + orig_offsetMax);
        rectTransform.offsetMin = new Vector2 (Mathf.Floor(orig_offsetMin.x * ScaleXPx), Mathf.Floor(orig_offsetMin.y * ScaleYPx));
        rectTransform.offsetMax = new Vector2 (Mathf.Floor(orig_offsetMax.x * ScaleXPx), Mathf.Floor(orig_offsetMax.y * ScaleYPx));
    }


    public void ScaleObject(GameObject obj){
        Vector3 orig_position = obj.transform.position;
        Vector3 orig_scale = obj.transform.localScale;

        if (DebugMode){
            Debug.Log(obj.name);
            Debug.Log("ORIG POSITION: " + orig_position);
            Debug.Log("ORIG SCALE: " + orig_scale);
        }

        obj.transform.position = new Vector3(orig_position.x * ScaleX, orig_position.y * ScaleY, orig_position.z);
        obj.transform.localScale = new Vector3(orig_scale.x * ScaleX, orig_scale.y * ScaleY, orig_scale.z);
        
        if (DebugMode){
            Vector3 new_position = obj.transform.position;
            Vector3 new_scale = obj.transform.localScale;

            Debug.Log("NEW POSITION: " + new_position);
            Debug.Log("NEW SCALE: " + new_scale);
        }

    }

    public float Scale_Value_To_Screen_Width(float val){
        //return val * (ScreenWidth/DevScreenWidth);
        return val * ScaleX;
    }

    public float Scale_Value_To_Screen_Height(float val){
        //return val * (ScreenHeight/DevScreenHeight);
        return val * ScaleY;
    }


    public Vector3 WorldToScreenPoint(Vector3 pos){
        return cam.WorldToScreenPoint(pos);
    }

    public Vector3 ScreenToWorldPoint(Vector3 pos){
        return cam.ScreenToWorldPoint(pos);
    }

    public bool isVisible(GameObject g){
        Vector3 screenPos = cam.WorldToScreenPoint(g.transform.position);
        return screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;
    }

}
