using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

using System.Linq;
using System.Reflection;

public class Block_Controller : MonoBehaviour
{

    [SerializeField]
    private Sprite Green_0, Green_1, Red_0, Red_1, White_0, White_1;

    private Color GreenLightColor, RedLightColor, WhiteLightColor;

    [SerializeField]
    private GameObject Guess_0, Guess_1, Guess_2, Guess_3, Guess_4;
    [SerializeField]
    private GameObject Hash_0, Hash_1, Hash_2, Hash_3, Hash_4;
    [SerializeField]
    private GameObject HitIndicatorWaypoint_0, HitIndicatorWaypoint_1, HitIndicatorWaypoint_2, HitIndicatorWaypoint_3;
    private GameObject HitIndicator;
    [SerializeField]
    private int HitIndicatorSelection = 3;
    private bool Solved = false;

    private static Game_Scaler gameScaler;

    enum LightColor {Green = 0, Red = 1, White = 2};
    enum LightState {Off = 0, On = 1};


    private bool[] hashArr = new bool[5];
    private bool[] guessArr = new bool[5];

    private GameObject[] hashObjs;
    private GameObject[] guessObjs;
    private GameObject[] HitIndicatorWaypointsObjs;

    private Vector3 initialBlockPos;

    private FieldInfo _LightCookieSprite; // Let's us access this field on Light2D since it's usually private. Little bit naughty.


    public delegate void BlockSolved();
    public static event BlockSolved BlockSolvedInfo;


    // Start is called before the first frame update
    void Start()
    {

        GreenLightColor = new Color(0.2588235f, 0.7372549f, 0.4980392f, 1f);
        RedLightColor = new Color(0.8666667f, 0.2156863f, 0.2705882f, 1f);
        WhiteLightColor = new Color(1f, 1f, 1f, 0.2f);

        HitIndicator = GameObject.Find("Hit_Indicator");
        HitIndicatorWaypointsObjs = new GameObject[] {HitIndicatorWaypoint_0, HitIndicatorWaypoint_1, HitIndicatorWaypoint_2, HitIndicatorWaypoint_3};
        hashObjs = new GameObject[] {Hash_0, Hash_1, Hash_2, Hash_3, Hash_4};
        guessObjs = new GameObject[] {Guess_0, Guess_1, Guess_2, Guess_3, Guess_4};

        Object_Info_Getter.ListProperties(Hash_0.GetComponent<Light2D>());
        


        initializeBlock();

        if (gameScaler == null){
            gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
            //Debug.Log("BLOCK GOT GAME SCALER");
        }

        scaleBlock();
        initialBlockPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSolved()){
            if (BlockSolvedInfo != null && !Solved){
                BlockSolvedInfo();
                Solved = true;
                updateGuessLights();
                updateHashLights();
            }
        }
    }


    private void _unscaleSingleObject(GameObject g){
        if(gameScaler.isVisible(gameObject)){
            //Debug.Log("VISIBLE!");
            Vector3 pos = g.transform.position;
            Vector3 scale = g.transform.localScale;
            //print(g.name + " POS: " + pos);
            //print("SCALEX: " + gameScaler.ScaleX);
            //print("SCALEY: " + gameScaler.ScaleY);
            g.transform.position = new Vector3(pos.x / gameScaler.ScaleX, pos.y / gameScaler.ScaleY, pos.z);
            g.transform.localScale = new Vector3(scale.x / gameScaler.ScaleX, scale.y / gameScaler.ScaleY, scale.z);

            //print(g.name + " NEW POS: " + g.transform.position);
        }
        else{
            //Debug.Log("NOT VISIBLE");
        }
    }

    private void scaleBlock(){
        // foreach (GameObject g in hashObjs){
        //     _scaleSingleObject(g);
        // }
        // foreach (GameObject g in guessObjs){
        //     _scaleSingleObject(g);
        // }
        // // foreach (GameObject g in HitIndicatorWaypointsObjs){
        // //     _scaleSingleObject(g);
        // // }
        // _scaleSingleObject(gameObject);
        Vector3 orig_scale = transform.localScale;
        transform.localScale = new Vector3(orig_scale.x * gameScaler.ScaleX, orig_scale.y * gameScaler.ScaleY, orig_scale.z);
        foreach (GameObject g in hashObjs){
            _unscaleSingleObject(g);
        }
        foreach (GameObject g in guessObjs){
            _unscaleSingleObject(g);
        }
        foreach (GameObject g in HitIndicatorWaypointsObjs){
            _unscaleSingleObject(g);
        }

    }

    private void initializeBlock(){
        randomizeHash();
        randomizeGuess();
        updateHashLights();
        updateGuessLights();
    }

    private string Arr2Str(bool[] arr){
        string s = "";
        foreach (bool i in arr)
            s += i.ToString() + " ";
        return s;
    }



    private void randomizeGuess(){
        for (int i = 0; i < guessArr.Length; i++){
            if (Random.value < 0.5f) {
                guessArr[i] = true;
            }
            else {
                guessArr[i] = false;
            }
        }
        if (isSolved() || !isSolvable(hashArr, guessArr)){
            randomizeGuess();
        }
    }

    private void randomizeHash(){
        for (int i = 0; i < hashArr.Length; i++){
            if (Random.value < 0.5f) {
                hashArr[i] = true;
            }
            else {
                hashArr[i] = false;
            }
        }
    }


    private void updateHashLights(){
        for (int i = 0; i < hashArr.Length; i++){
            LightColor CurLightColor;
            LightState CurLightState;
            if(hashArr[i] == guessArr[i] && !Solved){
                CurLightColor = LightColor.Green;
            }
            else if(!Solved){
                CurLightColor = LightColor.Red;
            }
            else{
                CurLightColor = LightColor.White;
            }
            updateSingleLight(hashObjs[i], hashArr[i] ? LightState.On : LightState.Off , CurLightColor);
        }
    }

    private void updateGuessLights(){
        for (int i = 0; i < guessArr.Length; i++){
            LightColor CurLightColor;
            LightState CurLightState;
            if(hashArr[i] == guessArr[i] && !Solved){
                CurLightColor = LightColor.Green;
            }
            else if(!Solved){
                CurLightColor = LightColor.Red;
            }
            else{
                CurLightColor = LightColor.White;
            }
            updateSingleLight(guessObjs[i], guessArr[i] ? LightState.On : LightState.Off , CurLightColor);
        }
    }



    private void updateSingleLight(GameObject light, LightState state, LightColor color){
        Animator anim = light.GetComponent<Animator>();
        anim.SetInteger("Num", (int)state);
        anim.SetInteger("Color", (int)color);
        
        _LightCookieSprite =  typeof( Light2D ).GetField( "m_LightCookieSprite", BindingFlags.NonPublic | BindingFlags.Instance );
        // Debug.Log("SETTING LIGHTS!");
        switch (state){
            case LightState.Off: 
            {
                switch (color){
                    case LightColor.Green:
                    {
                        //light.GetComponent<Light2D>().lightCookieSprite = Green_0;
                        _LightCookieSprite.SetValue(light.GetComponent<Light2D>(), Green_0);
                        light.GetComponent<Light2D>().color = GreenLightColor;
                        break;
                    }
                    case LightColor.Red:
                    {
                        //light.GetComponent<Light2D>().lightCookieSprite = Red_0;
                        _LightCookieSprite.SetValue(light.GetComponent<Light2D>(), Red_0);
                        light.GetComponent<Light2D>().color = RedLightColor;
                        break;
                    }
                    case LightColor.White:
                    {
                        //light.GetComponent<Light2D>().lightCookieSprite = White_0;
                        _LightCookieSprite.SetValue(light.GetComponent<Light2D>(), White_0);
                        light.GetComponent<Light2D>().color = WhiteLightColor;
                        break;
                    }
                    default: break;
                }
                break;
            }
            case LightState.On:
            {
                switch (color){
                    case LightColor.Green:
                    {
                        //light.GetComponent<Light2D>().lightCookieSprite = Green_1;
                        _LightCookieSprite.SetValue(light.GetComponent<Light2D>(), Green_1);
                        light.GetComponent<Light2D>().color = GreenLightColor;
                        break;
                    }
                    case LightColor.Red:
                    {
                        //light.GetComponent<Light2D>().lightCookieSprite = Red_1;
                        _LightCookieSprite.SetValue(light.GetComponent<Light2D>(), Red_1);
                        light.GetComponent<Light2D>().color = RedLightColor;
                        break;
                    }
                    case LightColor.White:
                    {
                        //light.GetComponent<Light2D>().lightCookieSprite = White_1;
                        _LightCookieSprite.SetValue(light.GetComponent<Light2D>(), White_1);
                        light.GetComponent<Light2D>().color = WhiteLightColor;
                        break;
                    }
                    default: break;
                }
                break;
            }
            default: break;
        }
    }


    private bool isSolved(){
        return Enumerable.SequenceEqual(guessArr, hashArr);
    }

    private bool isSolvable(bool[] hashArr, bool[] guessArr){
        int diffcount = 0;
        for (int i = 0; i < hashArr.Length; i++){
            if (hashArr[i] != guessArr[i])
            {
                diffcount++;
            }
        }
        return diffcount%2==0;
    }



    public bool Hit(int HitIndicatorSelection){
        guessArr[HitIndicatorSelection] = !guessArr[HitIndicatorSelection];
        guessArr[HitIndicatorSelection+1] = !guessArr[HitIndicatorSelection+1];
        updateHashLights();
        updateGuessLights();
        return isSolved();
    }

    // public bool HitPeek(int HitIndicatorSelection){ // Return True if a hit at this HitIndicatorSelection would solve the block
    //     guessArr[HitIndicatorSelection] = !guessArr[HitIndicatorSelection];
    //     guessArr[HitIndicatorSelection+1] = !guessArr[HitIndicatorSelection+1];
    //     bool solved = isSolved(); // Just do the operation, check for solved, then undo it
    //     guessArr[HitIndicatorSelection] = !guessArr[HitIndicatorSelection];
    //     guessArr[HitIndicatorSelection+1] = !guessArr[HitIndicatorSelection+1];
    //     return solved;
    // }

}
