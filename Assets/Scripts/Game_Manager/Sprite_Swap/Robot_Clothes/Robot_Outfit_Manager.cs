using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

using UnityEngine.SceneManagement;

public class Robot_Outfit_Manager : MonoBehaviour
{
    [SerializeField]
    private Robot_Outfit_Scriptable_Object[] allOutfits; // All outfits that exist in the game currently
    
    public int CurOutfitID { get { return curOutfitID; } private set { curOutfitID = value; } }

    private int curOutfitID = 0;

    public static Robot_Outfit_Manager instance;
    
    void Awake()
    {
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }


    private Robot_Outfit_Scriptable_Object getOutfitById(int outfitId){
        Robot_Outfit_Scriptable_Object outfit = Array.Find(allOutfits, o => o.OutfitId == outfitId);
        return outfit;
    }


    // Set his outfit whenever we change his outfit id
    public void setCurRobotOutfitId(int outfitId){
        CurOutfitID = outfitId;
        setRobotOutfit(CurOutfitID);
    }

    // Set his outfit whenever we load a new level
    void OnLevelWasLoaded()
    {
        setRobotOutfit(CurOutfitID);
    }



    public void setRobotOutfit(int outfitId){
        Robot_Outfit_Scriptable_Object outfit = null;
        try{
                outfit = getOutfitById(CurOutfitID);
        }
        catch(Exception e){
            Debug.Log("Couldn't find outfit with id " + outfitId);
        }
        if (SceneManager.GetActiveScene().name == "Main_Area"){
            if(outfit != null){
                GameObject.Find("Robot_Clothes").GetComponent<SpriteRenderer>().sprite = outfit.RobotOutfitSprite;
            }
        }
        else if(SceneManager.GetActiveScene().name == "Mine_Game"){
            Debug.Log("SET OSCARS OUTFIT!!!!!!!!");
        }

        if(outfit == null){
            // If something goes fucky, then just put him in his default outfit
            setRobotOutfit(0);
        }
    }
}
