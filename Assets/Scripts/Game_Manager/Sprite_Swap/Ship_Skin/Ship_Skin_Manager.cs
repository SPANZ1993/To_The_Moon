using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

using UnityEngine.SceneManagement;

public class Ship_Skin_Manager : MonoBehaviour
{
    [SerializeField]
    private Ship_Skin_Scriptable_Object[] allSkins; // All skins that exist in the game currently
    
    public int CurSkinID { get { return curSkinID; } private set { curSkinID = value; } }

    [SerializeField]
    private int curSkinID;

    public static Ship_Skin_Manager instance;
    
    void Awake()
    {
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            curSkinID = 0;
        }
        else{
            Destroy(this.gameObject);
        }
    }


    private Ship_Skin_Scriptable_Object getSkinById(int skinId){
        Ship_Skin_Scriptable_Object skin = Array.Find(allSkins, o => o.SkinId == skinId);
        return skin;
    }

    public Ship_Skin_Scriptable_Object getCurSkin(){
        return getSkinById(CurSkinID);
    }


    // Set ship skin whenever we change the skin Id
    public void setCurShipSkinId(int skinId){
        CurSkinID = skinId;
        setShipSkin(CurSkinID);
    }

    // Set his skin whenever we load a new level
    void OnLevelWasLoaded()
    {
        setShipSkin(CurSkinID);
    }



    public void setShipSkin(int skinId){
        Ship_Skin_Scriptable_Object skin = null;
        try{
                skin = getSkinById(CurSkinID);
        }
        catch(Exception e){
            Debug.Log("Couldn't find skin with id " + skinId);
        }
        if (SceneManager.GetActiveScene().name.StartsWith("Main_Area")){
            if(skin != null){
                GameObject.Find("Rocket").GetComponent<SpriteRenderer>().sprite = skin.ShipSkinSprite;
            }
        }
        else if(SceneManager.GetActiveScene().name.StartsWith("Rocket_Flight")){
            GameObject.Find("Rocket").GetComponent<SpriteRenderer>().sprite = skin.ShipSkinSprite;
        }

        if(skin == null){
            // If something goes fucky, then just set the default skin
            setShipSkin(0);
        }
    }
}
