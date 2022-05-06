using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement;

public class Speech_Object_Generator : MonoBehaviour
{

    public static Speech_Object_Generator instance;
    private Localization_Manager localizationManager;

    private UI_Characters.Characters2Emotions characters2Emotions;
    
    private List<string> allKeysList; // All possible keys for loading strings
    private Dictionary<string, UI_Characters.Characters> script2Character;
    private Dictionary<string, UI_Characters.Emotions> script2Emotion;
    private Dictionary<string, UI_Characters.Emotions> script2PostEmotion;

    string robotStringTableName = "Robot_Script_Table";
    string main_area_ui_table_name = "UI_Banner";

    void Awake(){

        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }

    void Start(){
        localizationManager = Localization_Manager.instance;

        characters2Emotions = new UI_Characters.Characters2Emotions();

        script2Character = new Dictionary<string, UI_Characters.Characters>(){
            // Robot
            {"Robot_Script.Autopilot_Return.Normal.No_Gems.1.1", UI_Characters.Characters.Robot}, // 1
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.1", UI_Characters.Characters.Robot}, // 2
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.2", UI_Characters.Characters.Robot}, // 3
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.1", UI_Characters.Characters.Robot }, // 4
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.2", UI_Characters.Characters.Robot} // 5
        };
        script2Emotion = new Dictionary<string, UI_Characters.Emotions>(){
            // Robot
            {"Robot_Script.Autopilot_Return.Normal.No_Gems.1.1", UI_Characters.Emotions.Talking}, // 1
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.1", UI_Characters.Emotions.Talking}, // 2
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.2", UI_Characters.Emotions.Talking}, // 3
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.1", UI_Characters.Emotions.Talking}, // 4
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.2", UI_Characters.Emotions.Talking} // 5
        };
        script2PostEmotion = new Dictionary<string, UI_Characters.Emotions>(){
            // Robot
            {"Robot_Script.Autopilot_Return.Normal.No_Gems.1.1", UI_Characters.Emotions.Idle}, // 1
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.1", UI_Characters.Emotions.Idle}, // 2
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.2", UI_Characters.Emotions.Idle}, // 3
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.1", UI_Characters.Emotions.Idle}, // 4
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.2", UI_Characters.Emotions.Idle} // 5
        };

        allKeysList = getAllKeys();
        Validate_Speech_Object_Generator_Data();
    }


    private List<string> getAllKeys(){
        List<string> keys = new List<string>();
        List<string> script2CharacterKeys = new List<string>(this.script2Character.Keys);
        keys = keys.Concat(script2CharacterKeys).ToList();
        List<string> script2EmotionKeys = new List<string>(this.script2Emotion.Keys);
        keys = keys.Concat(script2EmotionKeys).ToList();
        List<string> script2PostEmotionKeys = new List<string>(this.script2PostEmotion.Keys);
        keys = keys.Concat(script2PostEmotionKeys).ToList();
        keys = keys.Distinct().ToList();

        return keys;
    }


    private void Validate_Speech_Object_Generator_Data(){


        List<string> keys = getAllKeys();
        UI_Characters.Characters curCharacter;
        UI_Characters.Emotions curEmotion;
        UI_Characters.Emotions curPostEmotion;
        string localizedStrTemp = "";
        foreach(string key in keys){
            if(!(script2Character.ContainsKey(key) && script2Emotion.ContainsKey(key) && script2PostEmotion.ContainsKey(key))){
                throw new Exception("Key: " + key + " Not in All Dictionaries");
            }
            curCharacter = script2Character[key];
            curEmotion = script2Emotion[key];
            curPostEmotion = script2PostEmotion[key];
            if (!characters2Emotions.characterHasEmotion(curCharacter, curEmotion) || !characters2Emotions.characterHasEmotion(curCharacter, curEmotion)){
                throw new Exception("Key: " + key + " Has Invalid Emotion");
            }

            string curStringTableName = "";
            if(key.StartsWith("Robot_Script")){
                localizedStrTemp = localizationManager.GetLocalizedString(robotStringTableName, key);
                curStringTableName = robotStringTableName;
            }
            else{
                throw new Exception("Key: " + key + " doesn't follow naming conventions");
            }
            if (localizedStrTemp == "" || localizedStrTemp.StartsWith("No translation found")){
                throw new Exception("Key: " + key + " not present in stringtable " + curStringTableName);
            }
        }
    }


    private string SelectRandomStartKey(string keyStringBase){ // Given the base of a key string, choose a random key to start the speech object
        List<string> possibleStarts = new List<string>();
        foreach (string key in allKeysList){
            Debug.Log("CHECKING KEY: " + key + " against " + keyStringBase);
            if (key.StartsWith(keyStringBase) && key.EndsWith(".1")){
                possibleStarts.Add(key);
            }
        }
        Debug.Log("Possible Starts: " + string.Join(", ",  possibleStarts));
        return possibleStarts[UnityEngine.Random.Range(0, possibleStarts.Count-1)];
    }

    private string determineStringTableFromKeyString(string keyString){
        if (keyString.StartsWith("Robot_Script")){
            return robotStringTableName;
        }
        throw new Exception("Key doesn't belong to any string table");
        return null;
    }

    private Speech_Object buildSpeechObjectWithStartKey(bool isBlocker, string keyString, Func<string, string> formatFunc=null){
        string stringTable = determineStringTableFromKeyString(keyString);
        
        
        
        
        List<string> Speech_Strings_List = new List<string>();
        List<UI_Characters.Characters> Characters_List = new List<UI_Characters.Characters>();
        List<UI_Characters.Emotions> Emotions_List = new List<UI_Characters.Emotions>();
        List<UI_Characters.Emotions> Post_Emotions_List = new List<UI_Characters.Emotions>();
        bool stringsRemaining = true;
        string curString = "";
        List<string> splitKeyString = new List<string>();
        int curKeyStringConversationIndex = 0;
        int count = 1000; // Don't think we'll ever need to load a convo 1000 strings long
        while (stringsRemaining && count != 0){
            curString = localizationManager.GetLocalizedString(stringTable, keyString);
            if (formatFunc != null){
                curString = formatFunc(curString);
            }
            Debug.Log("CUR STRING: " + curString);
            if (!curString.StartsWith("No translation found")){
                Speech_Strings_List.Add(curString);
                Characters_List.Add(script2Character[keyString]);
                Emotions_List.Add(script2Emotion[keyString]);
                Post_Emotions_List.Add(script2PostEmotion[keyString]);
            }
            else{
                stringsRemaining = false;
            }
            Debug.Log("WAS: " + keyString);
            splitKeyString = new List<string>(keyString.Split("."));
            curKeyStringConversationIndex = Int32.Parse(splitKeyString[splitKeyString.Count-1]) + 1;
            keyString = string.Join(".", splitKeyString.GetRange(0, splitKeyString.Count-1)) + "." + curKeyStringConversationIndex.ToString();
            Debug.Log("NOW: " + keyString);
            count--;
        }
        return new Speech_Object(isBlocker, Speech_Strings_List, Characters_List, Emotions_List, Post_Emotions_List);
    }


    public Speech_Object GenerateAutopilotResultSpeech(AutopilotReturnState autopilotReturnState, double autopilotHeight, int autopilotGems){
        
        string AutopilotResultFormatFunc(string inputStr){
            inputStr = inputStr.Replace("{height}", Number_String_Formatter.formatHeightForSpeechObject(autopilotHeight));
            inputStr = inputStr.Replace("{gems}", autopilotGems.ToString());
            return inputStr;
        }

        string keyStringBase = "Robot_Script.Autopilot_Return";
        switch(autopilotReturnState){
            case AutopilotReturnState.Normal:
                keyStringBase += ".Normal";
                break;
            default:
                break;
        }
        
        if(autopilotGems == 0){
            keyStringBase += ".No_Gems";
        }
        else if(autopilotGems == 1){
            keyStringBase += ".Single_Gem";
        }
        else{
            keyStringBase += ".Multiple_Gems";
        }

        string keyString = SelectRandomStartKey(keyStringBase);
        return buildSpeechObjectWithStartKey(isBlocker:true, keyString:keyString, formatFunc:AutopilotResultFormatFunc);
    }

    

}
