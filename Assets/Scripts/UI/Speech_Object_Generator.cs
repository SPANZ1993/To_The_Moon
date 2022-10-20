using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement;


using Newtonsoft.Json;


public class Speech_Object_Generator : MonoBehaviour
{

    public static Speech_Object_Generator instance;

    private UI_Characters.Characters2Names characters2Names;
    private UI_Characters.Characters2DisplayNames characters2DisplayNames;
    private UI_Characters.Characters2Emotions characters2Emotions;
    
    private List<string> allKeysList; // All possible keys for loading strings
    private Dictionary<string, UI_Characters.Characters> script2Character;
    private Dictionary<string, UI_Characters.Emotions> script2Emotion;
    private Dictionary<string, UI_Characters.Emotions> script2PostEmotion;

    string robotStringTableName = "Robot_Script_Table";
    string eventsScriptTableName = "Events_Script";
    string main_area_ui_table_name = "UI_Banner";

    void Awake(){

        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            characters2Emotions = new UI_Characters.Characters2Emotions();
            characters2Names = new UI_Characters.Characters2Names();
            characters2DisplayNames = new UI_Characters.Characters2DisplayNames();
        }
        else{
            Destroy(this.gameObject);
        }
    }

    void Start(){
        



        script2Character = new Dictionary<string, UI_Characters.Characters>(){
            // Robot
            {"Robot_Script.Autopilot_Return.Normal.No_Gems.1.1", UI_Characters.Characters.Robot}, // 1
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.1", UI_Characters.Characters.Robot}, // 2
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.2", UI_Characters.Characters.Robot}, // 3
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.1", UI_Characters.Characters.Robot }, // 4
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.2", UI_Characters.Characters.Robot}, // 5
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.3", UI_Characters.Characters.Robot}, //5a
            // End Robot

            // Onboarding
            {"Events_Script.Onboarding.1.1", UI_Characters.Characters.Robot}, // 6
            {"Events_Script.Onboarding.1.2", UI_Characters.Characters.Robot}, // 7
            {"Events_Script.Onboarding.1.3", UI_Characters.Characters.Robot}, // 8
            {"Events_Script.Onboarding.1.4", UI_Characters.Characters.Robot}, // 9
            {"Events_Script.Onboarding.1.5", UI_Characters.Characters.Robot}, // 10
            {"Events_Script.Onboarding.1.6", UI_Characters.Characters.Robot}, // 11

            {"Events_Script.Onboarding.2.1", UI_Characters.Characters.Robot}, // 12
            {"Events_Script.Onboarding.2.2", UI_Characters.Characters.Robot}, // 13
            {"Events_Script.Onboarding.2.3", UI_Characters.Characters.Robot}, // 14
            
            {"Events_Script.Onboarding.3.1", UI_Characters.Characters.Robot}, // 15
            {"Events_Script.Onboarding.3.2", UI_Characters.Characters.Robot}, // 16
            {"Events_Script.Onboarding.3.3", UI_Characters.Characters.Robot}, // 17
            {"Events_Script.Onboarding.3.4", UI_Characters.Characters.Robot}, // 18

            {"Events_Script.Onboarding.4.1", UI_Characters.Characters.Robot}, // 19
            {"Events_Script.Onboarding.4.2", UI_Characters.Characters.Robot}, // 20
            {"Events_Script.Onboarding.4.3", UI_Characters.Characters.Robot}, // 21
            {"Events_Script.Onboarding.4.4", UI_Characters.Characters.Robot}, // 22

            {"Events_Script.Onboarding.5.1", UI_Characters.Characters.Robot}, // 23

            {"Events_Script.Onboarding.6.1", UI_Characters.Characters.Robot}, // 24
            {"Events_Script.Onboarding.6.2", UI_Characters.Characters.Robot}, // 25
            {"Events_Script.Onboarding.6.3", UI_Characters.Characters.Robot}, // 26


            {"Events_Script.Onboarding.7.1", UI_Characters.Characters.Robot}, // 27

            {"Events_Script.Onboarding.8.1", UI_Characters.Characters.Robot}, // 28

            {"Events_Script.Onboarding.9.1", UI_Characters.Characters.Robot}, // 29
            {"Events_Script.Onboarding.9.2", UI_Characters.Characters.Robot}, // 30
            {"Events_Script.Onboarding.9.3", UI_Characters.Characters.Robot}, // 30b

            {"Events_Script.Onboarding.10.1", UI_Characters.Characters.Robot}, // 31
            {"Events_Script.Onboarding.10.2", UI_Characters.Characters.Robot}, // 32
            {"Events_Script.Onboarding.10.3", UI_Characters.Characters.Robot}, // 32b

            {"Events_Script.Onboarding.11.1", UI_Characters.Characters.Robot}, // 33
            {"Events_Script.Onboarding.11.2", UI_Characters.Characters.Robot}, // 34

            {"Events_Script.Onboarding.12.1", UI_Characters.Characters.Robot}, // 35

            {"Events_Script.Onboarding.13.1", UI_Characters.Characters.Dog}, // 36
            {"Events_Script.Onboarding.13.2", UI_Characters.Characters.Robot}, // 37
            {"Events_Script.Onboarding.13.3", UI_Characters.Characters.Robot}, // 38
            {"Events_Script.Onboarding.13.4", UI_Characters.Characters.Robot}, // 39
            {"Events_Script.Onboarding.13.5", UI_Characters.Characters.Robot}, // 40
            {"Events_Script.Onboarding.13.6", UI_Characters.Characters.Dog}, // 41
            {"Events_Script.Onboarding.13.7", UI_Characters.Characters.Dog}, // 42
            {"Events_Script.Onboarding.13.8", UI_Characters.Characters.Dog}, // 42b
            {"Events_Script.Onboarding.13.9", UI_Characters.Characters.Dog}, // 43
            {"Events_Script.Onboarding.13.10", UI_Characters.Characters.Dog}, // 44
            {"Events_Script.Onboarding.13.11", UI_Characters.Characters.Robot}, // 45
            {"Events_Script.Onboarding.13.12", UI_Characters.Characters.Dog}, // 46
            {"Events_Script.Onboarding.13.13", UI_Characters.Characters.Dog}, // 46b
            {"Events_Script.Onboarding.13.14", UI_Characters.Characters.Dog}, // 47
            {"Events_Script.Onboarding.13.15", UI_Characters.Characters.Dog}, // 48
            {"Events_Script.Onboarding.13.16", UI_Characters.Characters.Dog}, // 49
            {"Events_Script.Onboarding.13.17", UI_Characters.Characters.Dog}, // 50
            {"Events_Script.Onboarding.13.18", UI_Characters.Characters.Dog}, // 51
            {"Events_Script.Onboarding.13.19", UI_Characters.Characters.Dog}, // 52
            {"Events_Script.Onboarding.13.20", UI_Characters.Characters.Robot}, // 53
            {"Events_Script.Onboarding.13.21", UI_Characters.Characters.Dog}, // 54
            {"Events_Script.Onboarding.13.22", UI_Characters.Characters.Dog}, // 55
            {"Events_Script.Onboarding.13.23", UI_Characters.Characters.Dog}, // 56
            {"Events_Script.Onboarding.13.24", UI_Characters.Characters.Dog}, // 57
            {"Events_Script.Onboarding.13.25", UI_Characters.Characters.Dog}, // 58
            {"Events_Script.Onboarding.13.26", UI_Characters.Characters.Dog}, // 59
            {"Events_Script.Onboarding.13.27", UI_Characters.Characters.Robot}, // 60
            {"Events_Script.Onboarding.13.28", UI_Characters.Characters.Dog}, // 61
            {"Events_Script.Onboarding.13.29", UI_Characters.Characters.Robot}, // 62
            {"Events_Script.Onboarding.13.30", UI_Characters.Characters.Dog}, // 63
            {"Events_Script.Onboarding.13.31", UI_Characters.Characters.Dog}, // 63b
            {"Events_Script.Onboarding.13.32", UI_Characters.Characters.Robot}, // 64
            {"Events_Script.Onboarding.13.33", UI_Characters.Characters.Dog}, // 65
            {"Events_Script.Onboarding.13.34", UI_Characters.Characters.Dog}, // 66
            {"Events_Script.Onboarding.13.35", UI_Characters.Characters.Robot}, // 67
            {"Events_Script.Onboarding.13.36", UI_Characters.Characters.Robot}, // 68
            {"Events_Script.Onboarding.13.37", UI_Characters.Characters.Robot}, // 69
            {"Events_Script.Onboarding.13.38", UI_Characters.Characters.Robot}, // 70

            {"Events_Script.Onboarding.14.1", UI_Characters.Characters.Robot}, // 71
            {"Events_Script.Onboarding.14.2", UI_Characters.Characters.Robot}, // 72

            {"Events_Script.Onboarding.15.1", UI_Characters.Characters.Robot}, // 73
            {"Events_Script.Onboarding.15.2", UI_Characters.Characters.Robot}, // 74
            {"Events_Script.Onboarding.15.3", UI_Characters.Characters.Robot}, // 75
            {"Events_Script.Onboarding.15.4", UI_Characters.Characters.Robot}, // 76
            {"Events_Script.Onboarding.15.5", UI_Characters.Characters.Robot}, // 77
            {"Events_Script.Onboarding.15.6", UI_Characters.Characters.Robot}, // 78

            {"Events_Script.Onboarding.16.1", UI_Characters.Characters.Robot}, // 79
            {"Events_Script.Onboarding.16.2", UI_Characters.Characters.Robot}, // 80
            {"Events_Script.Onboarding.16.3", UI_Characters.Characters.Robot}, // 81
            {"Events_Script.Onboarding.16.4", UI_Characters.Characters.Robot}, // 82
            {"Events_Script.Onboarding.16.5", UI_Characters.Characters.Robot}, // 83
            {"Events_Script.Onboarding.16.6", UI_Characters.Characters.Robot}, // 84
            {"Events_Script.Onboarding.16.7", UI_Characters.Characters.Robot}, // 85
            {"Events_Script.Onboarding.16.8", UI_Characters.Characters.Robot}, // 86
            {"Events_Script.Onboarding.16.9", UI_Characters.Characters.Robot}, // 87
            {"Events_Script.Onboarding.16.10", UI_Characters.Characters.Robot}, // 88
            {"Events_Script.Onboarding.16.11", UI_Characters.Characters.Robot}, // 89
            {"Events_Script.Onboarding.16.12", UI_Characters.Characters.Robot}, // 90
            {"Events_Script.Onboarding.16.13", UI_Characters.Characters.Robot}, // 90

            {"Events_Script.Onboarding.17.1", UI_Characters.Characters.Robot}, // 91

            {"Events_Script.Onboarding.18.1", UI_Characters.Characters.Robot}, // 92
            {"Events_Script.Onboarding.18.2", UI_Characters.Characters.Robot}, // 93
            {"Events_Script.Onboarding.18.3", UI_Characters.Characters.Robot}, // 94

            {"Events_Script.Onboarding.19.1", UI_Characters.Characters.Robot}, // 95
            {"Events_Script.Onboarding.19.2", UI_Characters.Characters.Robot}, // 96

            {"Events_Script.Onboarding.20.1", UI_Characters.Characters.Robot}, // 97
            {"Events_Script.Onboarding.20.2", UI_Characters.Characters.Robot}, // 98

            {"Events_Script.Onboarding.21.1", UI_Characters.Characters.Robot}, // 99
            {"Events_Script.Onboarding.21.2", UI_Characters.Characters.Robot}, // 100
            {"Events_Script.Onboarding.21.3", UI_Characters.Characters.Robot}, // 101
            {"Events_Script.Onboarding.21.4", UI_Characters.Characters.Robot}, // 102
            {"Events_Script.Onboarding.21.5", UI_Characters.Characters.Robot}, // 103
            {"Events_Script.Onboarding.21.6", UI_Characters.Characters.Robot}, // 104
            {"Events_Script.Onboarding.21.7", UI_Characters.Characters.Robot}, // 105
            // End Onboarding


            // NextLevelNotReadyEarth
            {"Events_Script.NextLevelNotReady.Earth.1.1", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.1.2", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.1.3", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.1.4", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.1.5", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.1.6", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.2.1", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.2", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.3", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.4", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.2.5", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.2.6", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.7", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.8", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.9", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.10", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.2.11", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.12", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.2.13", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.2.14", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.15", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.16", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.17", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.18", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelNotReady.Earth.2.19", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.2.20", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Earth.2.21", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.2.22", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Earth.2.23", UI_Characters.Characters.Dog},
            // End NextLevelNotReadyEarth
            
            // NextlevelReadyEarth
            {"Events_Script.NextLevelReady.Earth.1.1", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.2", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.3", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelReady.Earth.1.4", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelReady.Earth.1.5", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.6", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.7", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelReady.Earth.1.8", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.9", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.10", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.11", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.12", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.13", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.14", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.15", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.16", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.17", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.18", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.19", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.20", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.21", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.22", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelReady.Earth.1.23", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.24", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.25", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.26", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.27", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.28", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.29", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.30", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.31", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.32", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.33", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.34", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.35", UI_Characters.Characters.Guy},
            {"Events_Script.NextLevelReady.Earth.1.36", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.37", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.38", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelReady.Earth.1.39", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.40", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.41", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.42", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.43", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.44", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.45", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.46", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.47", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.48", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelReady.Earth.1.49", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelReady.Earth.1.50", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelReady.Earth.1.51", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelReady.Earth.1.52", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelReady.Earth.1.53", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelReady.Earth.1.54", UI_Characters.Characters.Dog},
            // End NextlevelReadyEarth


            // Start NextLevelNotReadyMoon
            {"Events_Script.NextLevelNotReady.Moon.1.1", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Moon.1.2", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.3", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.4", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Moon.1.5", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.6", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Moon.1.7", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.8", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Moon.1.9", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.10", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Moon.1.11", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.12", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.13", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.14", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Moon.1.15", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Moon.1.16", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Moon.1.17", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.18", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.19", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.20", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Moon.1.21", UI_Characters.Characters.Robot},
            {"Events_Script.NextLevelNotReady.Moon.1.22", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Moon.1.23", UI_Characters.Characters.Dog},
            {"Events_Script.NextLevelNotReady.Moon.1.24", UI_Characters.Characters.Robot},
            // End NextLevelNotReadyMoon

            // Start NextLevelReadyMoon
            {"Events_Script.NextLevelReady.Moon.1.1", UI_Characters.Characters.Dog},
            // End NextLevelReadyMoon

            // Robot Dark Matter Hint
            {"Robot_Script.Hint_Dark_Matter.1.1", UI_Characters.Characters.Robot},
            {"Robot_Script.Hint_Dark_Matter.1.2", UI_Characters.Characters.Robot},
            {"Robot_Script.Hint_Dark_Matter.1.3", UI_Characters.Characters.Robot},
            // End Robot Dark Matter Hint

        };


        script2Emotion = new Dictionary<string, UI_Characters.Emotions>(){
            // Robot
            {"Robot_Script.Autopilot_Return.Normal.No_Gems.1.1", UI_Characters.Emotions.Talking}, // 1
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.1", UI_Characters.Emotions.Talking}, // 2
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.2", UI_Characters.Emotions.Talking}, // 3
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.1", UI_Characters.Emotions.Talking}, // 4
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.2", UI_Characters.Emotions.Talking}, // 5
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.3", UI_Characters.Emotions.Talking}, //5a
            // End Robot

            // Onboarding
            {"Events_Script.Onboarding.1.1", UI_Characters.Emotions.Talking}, // 6
            {"Events_Script.Onboarding.1.2", UI_Characters.Emotions.Talking}, // 7
            {"Events_Script.Onboarding.1.3", UI_Characters.Emotions.Talking}, // 8
            {"Events_Script.Onboarding.1.4", UI_Characters.Emotions.Talking}, // 9
            {"Events_Script.Onboarding.1.5", UI_Characters.Emotions.Talking}, // 10
            {"Events_Script.Onboarding.1.6", UI_Characters.Emotions.Talking}, // 11

            {"Events_Script.Onboarding.2.1", UI_Characters.Emotions.Talking}, // 12
            {"Events_Script.Onboarding.2.2", UI_Characters.Emotions.Talking}, // 13
            {"Events_Script.Onboarding.2.3", UI_Characters.Emotions.Talking}, // 14

            {"Events_Script.Onboarding.3.1", UI_Characters.Emotions.Talking}, // 15
            {"Events_Script.Onboarding.3.2", UI_Characters.Emotions.Talking}, // 16
            {"Events_Script.Onboarding.3.3", UI_Characters.Emotions.Talking}, // 17
            {"Events_Script.Onboarding.3.4", UI_Characters.Emotions.Talking}, // 18

            {"Events_Script.Onboarding.4.1", UI_Characters.Emotions.Talking}, // 19
            {"Events_Script.Onboarding.4.2", UI_Characters.Emotions.Talking}, // 20
            {"Events_Script.Onboarding.4.3", UI_Characters.Emotions.Talking}, // 21
            {"Events_Script.Onboarding.4.4", UI_Characters.Emotions.Talking}, // 22

            {"Events_Script.Onboarding.5.1", UI_Characters.Emotions.Talking}, // 23

            {"Events_Script.Onboarding.6.1", UI_Characters.Emotions.Talking}, // 24
            {"Events_Script.Onboarding.6.2", UI_Characters.Emotions.Talking}, // 25
            {"Events_Script.Onboarding.6.3", UI_Characters.Emotions.Talking}, // 26

            {"Events_Script.Onboarding.7.1", UI_Characters.Emotions.Talking}, // 27

            {"Events_Script.Onboarding.8.1", UI_Characters.Emotions.Talking}, // 28

            {"Events_Script.Onboarding.9.1", UI_Characters.Emotions.Talking}, // 29
            {"Events_Script.Onboarding.9.2", UI_Characters.Emotions.Talking}, // 30
            {"Events_Script.Onboarding.9.3", UI_Characters.Emotions.Talking}, // 30b

            {"Events_Script.Onboarding.10.1", UI_Characters.Emotions.Talking}, // 31
            {"Events_Script.Onboarding.10.2", UI_Characters.Emotions.Talking}, // 32
            {"Events_Script.Onboarding.10.3", UI_Characters.Emotions.Talking}, // 32b

            {"Events_Script.Onboarding.11.1", UI_Characters.Emotions.Talking}, // 33
            {"Events_Script.Onboarding.11.2", UI_Characters.Emotions.Talking}, // 34

            {"Events_Script.Onboarding.12.1", UI_Characters.Emotions.Talking}, // 35

            {"Events_Script.Onboarding.13.1", UI_Characters.Emotions.Talking}, // 36
            {"Events_Script.Onboarding.13.2", UI_Characters.Emotions.Talking}, // 37
            {"Events_Script.Onboarding.13.3", UI_Characters.Emotions.Talking}, // 38
            {"Events_Script.Onboarding.13.4", UI_Characters.Emotions.Talking}, // 39
            {"Events_Script.Onboarding.13.5", UI_Characters.Emotions.Talking}, // 40
            {"Events_Script.Onboarding.13.6", UI_Characters.Emotions.Talking}, // 41
            {"Events_Script.Onboarding.13.7", UI_Characters.Emotions.Talking}, // 42
            {"Events_Script.Onboarding.13.8", UI_Characters.Emotions.Talking}, // 42b
            {"Events_Script.Onboarding.13.9", UI_Characters.Emotions.Talking}, // 43
            {"Events_Script.Onboarding.13.10", UI_Characters.Emotions.Talking}, // 44
            {"Events_Script.Onboarding.13.11", UI_Characters.Emotions.Talking}, // 45
            {"Events_Script.Onboarding.13.12", UI_Characters.Emotions.Talking}, // 46
            {"Events_Script.Onboarding.13.13", UI_Characters.Emotions.Talking}, // 46b
            {"Events_Script.Onboarding.13.14", UI_Characters.Emotions.Talking}, // 47
            {"Events_Script.Onboarding.13.15", UI_Characters.Emotions.Talking}, // 48
            {"Events_Script.Onboarding.13.16", UI_Characters.Emotions.Talking}, // 49
            {"Events_Script.Onboarding.13.17", UI_Characters.Emotions.Talking}, // 50
            {"Events_Script.Onboarding.13.18", UI_Characters.Emotions.Talking}, // 51
            {"Events_Script.Onboarding.13.19", UI_Characters.Emotions.Talking}, // 52
            {"Events_Script.Onboarding.13.20", UI_Characters.Emotions.Talking}, // 53
            {"Events_Script.Onboarding.13.21", UI_Characters.Emotions.Talking}, // 54
            {"Events_Script.Onboarding.13.22", UI_Characters.Emotions.Talking}, // 55
            {"Events_Script.Onboarding.13.23", UI_Characters.Emotions.Talking}, // 56
            {"Events_Script.Onboarding.13.24", UI_Characters.Emotions.Talking}, // 57
            {"Events_Script.Onboarding.13.25", UI_Characters.Emotions.Talking}, // 58
            {"Events_Script.Onboarding.13.26", UI_Characters.Emotions.Talking}, // 59
            {"Events_Script.Onboarding.13.27", UI_Characters.Emotions.Talking}, // 60
            {"Events_Script.Onboarding.13.28", UI_Characters.Emotions.Talking}, // 61
            {"Events_Script.Onboarding.13.29", UI_Characters.Emotions.Talking}, // 62
            {"Events_Script.Onboarding.13.30", UI_Characters.Emotions.Talking}, // 63
            {"Events_Script.Onboarding.13.31", UI_Characters.Emotions.Talking}, // 63b
            {"Events_Script.Onboarding.13.32", UI_Characters.Emotions.Talking}, // 64
            {"Events_Script.Onboarding.13.33", UI_Characters.Emotions.Talking}, // 65
            {"Events_Script.Onboarding.13.34", UI_Characters.Emotions.Talking}, // 66
            {"Events_Script.Onboarding.13.35", UI_Characters.Emotions.Talking}, // 67
            {"Events_Script.Onboarding.13.36", UI_Characters.Emotions.Talking}, // 68
            {"Events_Script.Onboarding.13.37", UI_Characters.Emotions.Talking}, // 69
            {"Events_Script.Onboarding.13.38", UI_Characters.Emotions.Talking}, // 70

            {"Events_Script.Onboarding.14.1", UI_Characters.Emotions.Talking}, // 71
            {"Events_Script.Onboarding.14.2", UI_Characters.Emotions.Talking}, // 72

            {"Events_Script.Onboarding.15.1", UI_Characters.Emotions.Talking}, // 73
            {"Events_Script.Onboarding.15.2", UI_Characters.Emotions.Talking}, // 74
            {"Events_Script.Onboarding.15.3", UI_Characters.Emotions.Talking}, // 75
            {"Events_Script.Onboarding.15.4", UI_Characters.Emotions.Talking}, // 76
            {"Events_Script.Onboarding.15.5", UI_Characters.Emotions.Talking}, // 77
            {"Events_Script.Onboarding.15.6", UI_Characters.Emotions.Talking}, // 78

            {"Events_Script.Onboarding.16.1", UI_Characters.Emotions.Talking}, // 79
            {"Events_Script.Onboarding.16.2", UI_Characters.Emotions.Talking}, // 80
            {"Events_Script.Onboarding.16.3", UI_Characters.Emotions.Talking}, // 81
            {"Events_Script.Onboarding.16.4", UI_Characters.Emotions.Talking}, // 82
            {"Events_Script.Onboarding.16.5", UI_Characters.Emotions.Talking}, // 83
            {"Events_Script.Onboarding.16.6", UI_Characters.Emotions.Talking}, // 84
            {"Events_Script.Onboarding.16.7", UI_Characters.Emotions.Talking}, // 85
            {"Events_Script.Onboarding.16.8", UI_Characters.Emotions.Talking}, // 86
            {"Events_Script.Onboarding.16.9", UI_Characters.Emotions.Talking}, // 87
            {"Events_Script.Onboarding.16.10", UI_Characters.Emotions.Talking}, // 88
            {"Events_Script.Onboarding.16.11", UI_Characters.Emotions.Talking}, // 89
            {"Events_Script.Onboarding.16.12", UI_Characters.Emotions.Talking}, // 90
            {"Events_Script.Onboarding.16.13", UI_Characters.Emotions.Talking}, // 90b

            {"Events_Script.Onboarding.17.1", UI_Characters.Emotions.Talking}, // 91

            {"Events_Script.Onboarding.18.1", UI_Characters.Emotions.Talking}, // 92
            {"Events_Script.Onboarding.18.2", UI_Characters.Emotions.Talking}, // 93
            {"Events_Script.Onboarding.18.3", UI_Characters.Emotions.Talking}, // 94
            
            {"Events_Script.Onboarding.19.1", UI_Characters.Emotions.Talking}, // 95
            {"Events_Script.Onboarding.19.2", UI_Characters.Emotions.Talking}, // 96

            {"Events_Script.Onboarding.20.1", UI_Characters.Emotions.Talking}, // 97
            {"Events_Script.Onboarding.20.2", UI_Characters.Emotions.Talking}, // 98

            {"Events_Script.Onboarding.21.1", UI_Characters.Emotions.Talking}, // 99
            {"Events_Script.Onboarding.21.2", UI_Characters.Emotions.Talking}, // 100
            {"Events_Script.Onboarding.21.3", UI_Characters.Emotions.Talking}, // 101
            {"Events_Script.Onboarding.21.4", UI_Characters.Emotions.Talking}, // 102
            {"Events_Script.Onboarding.21.5", UI_Characters.Emotions.Talking}, // 103
            {"Events_Script.Onboarding.21.6", UI_Characters.Emotions.Talking}, // 104
            {"Events_Script.Onboarding.21.7", UI_Characters.Emotions.Talking}, // 105
            // End Onboarding


            // NextLevelNotReadyEarth
            {"Events_Script.NextLevelNotReady.Earth.1.1", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.1.2", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.1.3", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.1.4", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.1.5", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.1.6", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.1", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.2", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.3", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.4", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.5", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.6", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.7", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.8", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.9", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.10", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.11", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.12", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.13", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.14", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.15", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.16", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.17", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.18", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.19", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.20", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.21", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.22", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.23", UI_Characters.Emotions.Talking},
            // End NextLevelNotReadyEarth

            // NextlevelReadyEarth
            {"Events_Script.NextLevelReady.Earth.1.1", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.2", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.3", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.4", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.5", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.6", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.7", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelReady.Earth.1.8", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.9", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.10", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.11", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.12", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.13", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.14", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.15", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.16", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.17", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.18", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.19", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.20", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.21", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.22", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.23", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.24", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.25", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.26", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.27", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.28", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.29", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.30", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.31", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.32", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.33", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.34", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.35", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.36", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.37", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.38", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.39", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.40", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.41", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.42", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.43", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.44", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.45", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.46", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.47", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.48", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.49", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.50", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.51", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.52", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.53", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.54", UI_Characters.Emotions.Talking},
            // End NextlevelReadyEarth

            // Start NextLevelNotReadyMoon
            {"Events_Script.NextLevelNotReady.Moon.1.1", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.2", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.3", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.4", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.5", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.6", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.7", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.8", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.9", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.10", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.11", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.12", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.13", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.14", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.15", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.16", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.17", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.18", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.19", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.20", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.21", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.22", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.23", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.24", UI_Characters.Emotions.Talking},
            // End NextLevelNotReadyMoon

            // Start NextLevelReadyMoon
            {"Events_Script.NextLevelReady.Moon.1.1", UI_Characters.Emotions.Talking},
            // End NextLevelReadyMoon

            // Robot Dark Matter Hint
            {"Robot_Script.Hint_Dark_Matter.1.1", UI_Characters.Emotions.Talking},
            {"Robot_Script.Hint_Dark_Matter.1.2", UI_Characters.Emotions.Talking},
            {"Robot_Script.Hint_Dark_Matter.1.3", UI_Characters.Emotions.Talking},
            // End Robot Dark Matter Hint
        };
        script2PostEmotion = new Dictionary<string, UI_Characters.Emotions>(){
            // Robot
            {"Robot_Script.Autopilot_Return.Normal.No_Gems.1.1", UI_Characters.Emotions.Idle}, // 1
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.1", UI_Characters.Emotions.Idle}, // 2
            {"Robot_Script.Autopilot_Return.Normal.Single_Gem.1.2", UI_Characters.Emotions.Idle}, // 3
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.1", UI_Characters.Emotions.Idle}, // 4
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.2", UI_Characters.Emotions.Idle}, // 5
            {"Robot_Script.Autopilot_Return.Normal.Multiple_Gems.1.3", UI_Characters.Emotions.Sad}, //5a

            // Onboarding
            {"Events_Script.Onboarding.1.1", UI_Characters.Emotions.Idle}, // 6
            {"Events_Script.Onboarding.1.2", UI_Characters.Emotions.Idle}, // 7
            {"Events_Script.Onboarding.1.3", UI_Characters.Emotions.Idle}, // 8
            {"Events_Script.Onboarding.1.4", UI_Characters.Emotions.Idle}, // 9
            {"Events_Script.Onboarding.1.5", UI_Characters.Emotions.Idle}, // 10
            {"Events_Script.Onboarding.1.6", UI_Characters.Emotions.Idle}, // 11

            {"Events_Script.Onboarding.2.1", UI_Characters.Emotions.Idle}, // 12
            {"Events_Script.Onboarding.2.2", UI_Characters.Emotions.Idle}, // 13
            {"Events_Script.Onboarding.2.3", UI_Characters.Emotions.Idle}, // 14

            {"Events_Script.Onboarding.3.1", UI_Characters.Emotions.Idle}, // 15
            {"Events_Script.Onboarding.3.2", UI_Characters.Emotions.Idle}, // 16
            {"Events_Script.Onboarding.3.3", UI_Characters.Emotions.Idle}, // 17
            {"Events_Script.Onboarding.3.4", UI_Characters.Emotions.Idle}, // 18

            {"Events_Script.Onboarding.4.1", UI_Characters.Emotions.Idle}, // 19
            {"Events_Script.Onboarding.4.2", UI_Characters.Emotions.Idle}, // 20
            {"Events_Script.Onboarding.4.3", UI_Characters.Emotions.Idle}, // 21
            {"Events_Script.Onboarding.4.4", UI_Characters.Emotions.Idle}, // 22

            {"Events_Script.Onboarding.5.1", UI_Characters.Emotions.Idle}, // 23

            {"Events_Script.Onboarding.6.1", UI_Characters.Emotions.Idle}, // 24
            {"Events_Script.Onboarding.6.2", UI_Characters.Emotions.Idle}, // 25
            {"Events_Script.Onboarding.6.3", UI_Characters.Emotions.Idle}, // 26

            {"Events_Script.Onboarding.7.1", UI_Characters.Emotions.Idle}, // 27
            
            {"Events_Script.Onboarding.8.1", UI_Characters.Emotions.Idle}, // 28

            {"Events_Script.Onboarding.9.1", UI_Characters.Emotions.Idle}, // 29
            {"Events_Script.Onboarding.9.2", UI_Characters.Emotions.Idle}, // 30
            {"Events_Script.Onboarding.9.3", UI_Characters.Emotions.Idle}, // 30b

            {"Events_Script.Onboarding.10.1", UI_Characters.Emotions.Idle}, // 31
            {"Events_Script.Onboarding.10.2", UI_Characters.Emotions.Idle}, // 32
            {"Events_Script.Onboarding.10.3", UI_Characters.Emotions.Idle}, // 32b

            {"Events_Script.Onboarding.11.1", UI_Characters.Emotions.Idle}, // 33
            {"Events_Script.Onboarding.11.2", UI_Characters.Emotions.Idle}, // 34

            {"Events_Script.Onboarding.12.1", UI_Characters.Emotions.Idle}, // 35

            {"Events_Script.Onboarding.13.1", UI_Characters.Emotions.Happy}, // 36
            {"Events_Script.Onboarding.13.2", UI_Characters.Emotions.Idle}, // 37
            {"Events_Script.Onboarding.13.3", UI_Characters.Emotions.Idle}, // 38
            {"Events_Script.Onboarding.13.4", UI_Characters.Emotions.Idle}, // 39
            {"Events_Script.Onboarding.13.5", UI_Characters.Emotions.Idle}, // 40
            {"Events_Script.Onboarding.13.6", UI_Characters.Emotions.Talking}, // 41
            {"Events_Script.Onboarding.13.7", UI_Characters.Emotions.Talking}, // 42
            {"Events_Script.Onboarding.13.8", UI_Characters.Emotions.Talking}, // 42b
            {"Events_Script.Onboarding.13.9", UI_Characters.Emotions.Talking}, // 43
            {"Events_Script.Onboarding.13.10", UI_Characters.Emotions.Talking}, // 44
            {"Events_Script.Onboarding.13.11", UI_Characters.Emotions.Idle}, // 45
            {"Events_Script.Onboarding.13.12", UI_Characters.Emotions.Happy}, // 46
            {"Events_Script.Onboarding.13.13", UI_Characters.Emotions.Talking}, // 46b
            {"Events_Script.Onboarding.13.14", UI_Characters.Emotions.Talking}, // 47
            {"Events_Script.Onboarding.13.15", UI_Characters.Emotions.Talking}, // 48
            {"Events_Script.Onboarding.13.16", UI_Characters.Emotions.Talking}, // 49
            {"Events_Script.Onboarding.13.17", UI_Characters.Emotions.Happy}, // 50
            {"Events_Script.Onboarding.13.18", UI_Characters.Emotions.Talking}, // 51
            {"Events_Script.Onboarding.13.19", UI_Characters.Emotions.Sad}, // 52
            {"Events_Script.Onboarding.13.20", UI_Characters.Emotions.Idle}, // 53
            {"Events_Script.Onboarding.13.21", UI_Characters.Emotions.Talking}, // 54
            {"Events_Script.Onboarding.13.22", UI_Characters.Emotions.Sad}, // 55
            {"Events_Script.Onboarding.13.23", UI_Characters.Emotions.Talking}, // 56
            {"Events_Script.Onboarding.13.24", UI_Characters.Emotions.Talking}, // 57
            {"Events_Script.Onboarding.13.25", UI_Characters.Emotions.Talking}, // 58
            {"Events_Script.Onboarding.13.26", UI_Characters.Emotions.Talking}, // 59
            {"Events_Script.Onboarding.13.27", UI_Characters.Emotions.Idle}, // 60
            {"Events_Script.Onboarding.13.28", UI_Characters.Emotions.Happy}, // 61
            {"Events_Script.Onboarding.13.29", UI_Characters.Emotions.Idle}, // 62
            {"Events_Script.Onboarding.13.30", UI_Characters.Emotions.Talking}, // 63
            {"Events_Script.Onboarding.13.31", UI_Characters.Emotions.Talking}, // 63b
            {"Events_Script.Onboarding.13.32", UI_Characters.Emotions.Idle}, // 64
            {"Events_Script.Onboarding.13.33", UI_Characters.Emotions.Talking}, // 65
            {"Events_Script.Onboarding.13.34", UI_Characters.Emotions.Talking}, // 66
            {"Events_Script.Onboarding.13.35", UI_Characters.Emotions.Idle}, // 67
            {"Events_Script.Onboarding.13.36", UI_Characters.Emotions.Idle}, // 68
            {"Events_Script.Onboarding.13.37", UI_Characters.Emotions.Idle}, // 69
            {"Events_Script.Onboarding.13.38", UI_Characters.Emotions.Idle}, // 70

            {"Events_Script.Onboarding.14.1", UI_Characters.Emotions.Idle}, // 71
            {"Events_Script.Onboarding.14.2", UI_Characters.Emotions.Idle}, // 72

            {"Events_Script.Onboarding.15.1", UI_Characters.Emotions.Idle}, // 73
            {"Events_Script.Onboarding.15.2", UI_Characters.Emotions.Idle}, // 74 
            {"Events_Script.Onboarding.15.3", UI_Characters.Emotions.Idle}, // 75
            {"Events_Script.Onboarding.15.4", UI_Characters.Emotions.Idle}, // 76
            {"Events_Script.Onboarding.15.5", UI_Characters.Emotions.Idle}, // 77
            {"Events_Script.Onboarding.15.6", UI_Characters.Emotions.Idle}, // 78

            {"Events_Script.Onboarding.16.1", UI_Characters.Emotions.Idle}, // 79
            {"Events_Script.Onboarding.16.2", UI_Characters.Emotions.Idle}, // 80
            {"Events_Script.Onboarding.16.3", UI_Characters.Emotions.Idle}, // 81
            {"Events_Script.Onboarding.16.4", UI_Characters.Emotions.Idle}, // 82
            {"Events_Script.Onboarding.16.5", UI_Characters.Emotions.Idle}, // 83
            {"Events_Script.Onboarding.16.6", UI_Characters.Emotions.Idle}, // 84
            {"Events_Script.Onboarding.16.7", UI_Characters.Emotions.Idle}, // 85
            {"Events_Script.Onboarding.16.8", UI_Characters.Emotions.Idle}, // 86
            {"Events_Script.Onboarding.16.9", UI_Characters.Emotions.Idle}, // 87
            {"Events_Script.Onboarding.16.10", UI_Characters.Emotions.Idle}, // 88
            {"Events_Script.Onboarding.16.11", UI_Characters.Emotions.Idle}, // 89
            {"Events_Script.Onboarding.16.12", UI_Characters.Emotions.Idle}, // 90
            {"Events_Script.Onboarding.16.13", UI_Characters.Emotions.Idle}, // 90b

            {"Events_Script.Onboarding.17.1", UI_Characters.Emotions.Idle}, // 91
            
            {"Events_Script.Onboarding.18.1", UI_Characters.Emotions.Idle}, // 92
            {"Events_Script.Onboarding.18.2", UI_Characters.Emotions.Idle}, // 93
            {"Events_Script.Onboarding.18.3", UI_Characters.Emotions.Idle}, // 94

            {"Events_Script.Onboarding.19.1", UI_Characters.Emotions.Idle}, // 95
            {"Events_Script.Onboarding.19.2", UI_Characters.Emotions.Idle}, // 96

            {"Events_Script.Onboarding.20.1", UI_Characters.Emotions.Idle}, // 97
            {"Events_Script.Onboarding.20.2", UI_Characters.Emotions.Idle}, // 98

            {"Events_Script.Onboarding.21.1", UI_Characters.Emotions.Idle}, // 99
            {"Events_Script.Onboarding.21.2", UI_Characters.Emotions.Idle}, // 100
            {"Events_Script.Onboarding.21.3", UI_Characters.Emotions.Idle}, // 101
            {"Events_Script.Onboarding.21.4", UI_Characters.Emotions.Idle}, // 102
            {"Events_Script.Onboarding.21.5", UI_Characters.Emotions.Idle}, // 103
            {"Events_Script.Onboarding.21.6", UI_Characters.Emotions.Idle}, // 104
            {"Events_Script.Onboarding.21.7", UI_Characters.Emotions.Idle}, // 105
            // End Onboarding
        
            // NextLevelNotReadyEarth
            {"Events_Script.NextLevelNotReady.Earth.1.1", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelNotReady.Earth.1.2", UI_Characters.Emotions.Sad},
            {"Events_Script.NextLevelNotReady.Earth.1.3", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.1.4", UI_Characters.Emotions.Sad},
            {"Events_Script.NextLevelNotReady.Earth.1.5", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.1.6", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.1", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelNotReady.Earth.2.2", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelNotReady.Earth.2.3", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelNotReady.Earth.2.4", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.5", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.6", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.7", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.8", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.9", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelNotReady.Earth.2.10", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.11", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.12", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.13", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelNotReady.Earth.2.14", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.15", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.16", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.17", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelNotReady.Earth.2.18", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.19", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.20", UI_Characters.Emotions.Sad},
            {"Events_Script.NextLevelNotReady.Earth.2.21", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.22", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Earth.2.23", UI_Characters.Emotions.Happy},
            // End NextLevelNotReadyEarth

            // NextlevelReadyEarth
            {"Events_Script.NextLevelReady.Earth.1.1", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelReady.Earth.1.2", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.3", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelReady.Earth.1.4", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelReady.Earth.1.5", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.6", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.7", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelReady.Earth.1.8", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelReady.Earth.1.9", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.10", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.11", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelReady.Earth.1.12", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelReady.Earth.1.13", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.14", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.15", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.16", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.17", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.18", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.19", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.20", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.21", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.22", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelReady.Earth.1.23", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.24", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.25", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelReady.Earth.1.26", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.27", UI_Characters.Emotions.Sad},
            {"Events_Script.NextLevelReady.Earth.1.28", UI_Characters.Emotions.Sad},
            {"Events_Script.NextLevelReady.Earth.1.29", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.30", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.31", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelReady.Earth.1.32", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelReady.Earth.1.33", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelReady.Earth.1.34", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.35", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelReady.Earth.1.36", UI_Characters.Emotions.Sad},
            {"Events_Script.NextLevelReady.Earth.1.37", UI_Characters.Emotions.Sad},
            {"Events_Script.NextLevelReady.Earth.1.38", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelReady.Earth.1.39", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.40", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.41", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.42", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.43", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.44", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.45", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.46", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.47", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.48", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelReady.Earth.1.49", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelReady.Earth.1.50", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelReady.Earth.1.51", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelReady.Earth.1.52", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelReady.Earth.1.53", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelReady.Earth.1.54", UI_Characters.Emotions.Happy},
            // End NextlevelReadyEarth


            // Start NextLevelNotReadyMoon
            {"Events_Script.NextLevelNotReady.Moon.1.1", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelNotReady.Moon.1.2", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.3", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelNotReady.Moon.1.4", UI_Characters.Emotions.Sad},
            {"Events_Script.NextLevelNotReady.Moon.1.5", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelNotReady.Moon.1.6", UI_Characters.Emotions.Sad},
            {"Events_Script.NextLevelNotReady.Moon.1.7", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelNotReady.Moon.1.8", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.9", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelNotReady.Moon.1.10", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.11", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.12", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.13", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelNotReady.Moon.1.14", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.15", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.16", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.17", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelNotReady.Moon.1.18", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.19", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelNotReady.Moon.1.20", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.21", UI_Characters.Emotions.Idle},
            {"Events_Script.NextLevelNotReady.Moon.1.22", UI_Characters.Emotions.Talking},
            {"Events_Script.NextLevelNotReady.Moon.1.23", UI_Characters.Emotions.Happy},
            {"Events_Script.NextLevelNotReady.Moon.1.24", UI_Characters.Emotions.Idle},
            // End NextLevelNotReadyMoon

            // Start NextLevelReadyMoon
            {"Events_Script.NextLevelReady.Moon.1.1", UI_Characters.Emotions.Happy},
            // End NextLevelReadyMoon

        
            // Robot Dark Matter Hint
            {"Robot_Script.Hint_Dark_Matter.1.1", UI_Characters.Emotions.Idle},
            {"Robot_Script.Hint_Dark_Matter.1.2", UI_Characters.Emotions.Idle},
            {"Robot_Script.Hint_Dark_Matter.1.3", UI_Characters.Emotions.Idle},
            // End Robot Dark Matter Hint
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
                localizedStrTemp = Localization_Manager.instance.GetLocalizedString(robotStringTableName, key);
                curStringTableName = robotStringTableName;
            }
            else if(key.StartsWith("Events_Script")){
                localizedStrTemp = Localization_Manager.instance.GetLocalizedString(eventsScriptTableName, key);
                curStringTableName = eventsScriptTableName;
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
            //Debug.Log("CHECKING KEY: " + key + " against " + keyStringBase);
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
        else if (keyString.StartsWith("Events_Script")){
            return eventsScriptTableName;
        }
        throw new Exception("Key doesn't belong to any string table");
        return null;
    }

    public string defaultStringFormatFunc(string inputStr){
        inputStr = inputStr.Replace("{gamename}", Localization_Manager.instance.GetLocalizedString(main_area_ui_table_name, "UI.General.Game_Name"));
        inputStr = inputStr.Replace("{playername}", Game_Manager.instance.userDisplayName);
        inputStr = inputStr.Replace("{coinname}", Game_Manager.instance.coinName);
        inputStr = inputStr.Replace("{gemname_singular}", Localization_Manager.instance.GetLocalizedString(main_area_ui_table_name, "UI.General.Gem_Name_Singular"));
        inputStr = inputStr.Replace("{gemname_plural}", Localization_Manager.instance.GetLocalizedString(main_area_ui_table_name, "UI.General.Gem_Name_Plural"));
        inputStr = inputStr.Replace("{robot}", characters2Names[UI_Characters.Characters.Robot]);
        inputStr = inputStr.Replace("{robotDisplay}", characters2DisplayNames[UI_Characters.Characters.Robot]);
        inputStr = inputStr.Replace("{stonks}", characters2Names[UI_Characters.Characters.Guy]);
        inputStr = inputStr.Replace("{stonksDisplay}", characters2DisplayNames[UI_Characters.Characters.Guy]);
        inputStr = inputStr.Replace("{dog}", characters2Names[UI_Characters.Characters.Dog]);
        inputStr = inputStr.Replace("{dogDisplay}", characters2DisplayNames[UI_Characters.Characters.Dog]);
        inputStr = inputStr.Replace("{gorilla}", characters2Names[UI_Characters.Characters.Gorilla]);
        inputStr = inputStr.Replace("{gorillaDisplay}", characters2DisplayNames[UI_Characters.Characters.Gorilla]);

        return inputStr;
    }





    public Speech_Object buildSpeechObjectWithStartKey(bool isBlocker, string keyString, Func<string, string> formatFunc=null){
        string stringTable = determineStringTableFromKeyString(keyString);
        
        if(formatFunc==null){
            formatFunc = defaultStringFormatFunc;
        }
        
        
        
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
            curString = Localization_Manager.instance.GetLocalizedString(stringTable, keyString);
            if (formatFunc != null){
                curString = formatFunc(curString);
            }
            //Debug.Log("CUR STRING: " + curString);
            if (!curString.StartsWith("No translation found")){
                Speech_Strings_List.Add(curString);
                Characters_List.Add(script2Character[keyString]);
                Emotions_List.Add(script2Emotion[keyString]);
                Post_Emotions_List.Add(script2PostEmotion[keyString]);
            }
            else{
                stringsRemaining = false;
            }
            //Debug.Log("WAS: " + keyString);
            splitKeyString = new List<string>(keyString.Split("."));
            curKeyStringConversationIndex = Int32.Parse(splitKeyString[splitKeyString.Count-1]) + 1;
            keyString = string.Join(".", splitKeyString.GetRange(0, splitKeyString.Count-1)) + "." + curKeyStringConversationIndex.ToString();
            //Debug.Log("NOW: " + keyString);
            count--;
        }
        return new Speech_Object(isBlocker, Speech_Strings_List, Characters_List, Emotions_List, Post_Emotions_List);
    }







    public Speech_Object GenerateAutopilotResultSpeech(AutopilotReturnState autopilotReturnState, double autopilotHeight, int autopilotGems){
        
        string AutopilotResultFormatFunc(string inputStr){
            inputStr = defaultStringFormatFunc(inputStr);
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

    


    private UI_Characters.Characters jsonCharString2UI_Character(char c){
        if(c == 'R'){
            return UI_Characters.Characters.Robot;
        }
        else if(c == 'S'){
            return UI_Characters.Characters.Guy;
        }
        else if(c == 'D'){
            return UI_Characters.Characters.Dog;
        }
        else if (c == 'G'){
            return UI_Characters.Characters.Gorilla;
        }
        return UI_Characters.Characters.Robot;
    }

    private UI_Characters.Emotions jsonEmotString2UI_Emotion(char c){
        if(c == 'T'){
            return UI_Characters.Emotions.Talking;
        }
        else if(c == 'H'){
            return UI_Characters.Emotions.Happy;
        }
        else if(c == 'I'){
            return UI_Characters.Emotions.Idle;
        }
        else if (c == 'S'){
            return UI_Characters.Emotions.Sad;
        }
        return UI_Characters.Emotions.Talking;
    }




    public Speech_Object BuildSpeechObjectFromJSON(string json){
        // #########HOW THIS WORKS###########
        // ACCEPTS SOME JSON CONTAINING DATA TO BUILD A SPEECH OBJECT.. SHOULD HAVE A KEY "B" with Value 0 or 1 Denoting whether blocker or not
        // The other keys should be of the form Sn and SCn, where n is the order of the text element in the speech object
        // The value of the Sn keys is the actual speech text
        // The value of the SCn key is a three letter code denoting the character, the emotion, and the post-emotion
        // Possible characters are R, S, D, or G... Denoting Robot, Stonks, Dog, Gorilla
        // Possible emotions are T, H, I, S... Denoting Talking, Happy, Idle, Sad
        // Validation is done to ensure Sn/SCn keys go up in numerical order, and that the character information is valid (I.E. the given character has the given emotions)
        // If a speech object can be built (i.e. the json is valid) it is returned, otherwise null is returned
       
        
        if(ValidateSpeechObjectJson(json)){
            try{
                //return new Speech_Object();
                Dictionary<string, string> speechJsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                bool isBlocker = speechJsonDict["B"] == "1";
                List<string> Speech_Strings_List = new List<string>();
                List<UI_Characters.Characters> Characters_List = new List<UI_Characters.Characters>();
                List<UI_Characters.Emotions> Emotions_List = new List<UI_Characters.Emotions>();
                List<UI_Characters.Emotions> Post_Emotions_List = new List<UI_Characters.Emotions>();

                int curI = 1;
                string curSpeech;
                UI_Characters.Characters curCharacter;
                UI_Characters.Emotions curEmotion;
                UI_Characters.Emotions curPostEmotion;
                string curCharStr;
                while(speechJsonDict.Keys.Contains("S"+curI.ToString()) && speechJsonDict.Keys.Contains("SC"+curI.ToString())){
                    curSpeech = speechJsonDict["S"+curI.ToString()];
                    curCharStr = speechJsonDict["SC"+curI.ToString()];

                    Speech_Strings_List.Add(defaultStringFormatFunc(curSpeech));
                    Characters_List.Add(jsonCharString2UI_Character(curCharStr[0]));
                    Emotions_List.Add(jsonEmotString2UI_Emotion(curCharStr[1]));
                    Post_Emotions_List.Add(jsonEmotString2UI_Emotion(curCharStr[2]));
                    
                    
                    curI++;
                }

                return new Speech_Object(isBlocker, Speech_Strings_List, Characters_List, Emotions_List, Post_Emotions_List);
            }
            catch(Exception e){
                Debug.Log(e);
                return null;
            }
        }
        return null;
    }

    public bool ValidateSpeechObjectJson(string json){
        Dictionary<string, string> speechJsonDict = new Dictionary<string, string>();
        try{
            speechJsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
        catch(Exception e){
            Debug.Log("SOMETHING IS WRONG WITH THE FORMATTING OF OUR SPEECH JSON");
            return false;
        }

        if(speechJsonDict.Keys.Count < 3){
            Debug.Log("NOT ENOUGH KEYS IN OUR SPEECH DICT");
            return false;
        }


        bool foundB = false;
        foreach(string k in speechJsonDict.Keys){
            if(k == "B"){
                if(speechJsonDict[k] != "0" && speechJsonDict[k] != "1"){
                    Debug.Log("GOT IMPROPER VALUE FOR BLOCKER \"B\" Key " + speechJsonDict[k] + " OPTIONS ARE \"0\" and \"1\"");
                    return false;
                }
                foundB = true;
            }
            else if(k.StartsWith("S") && !k.StartsWith("SC")){
                if(k.Count(char.IsLetter) != 1 && !k.Select(c => c != 'S' && char.IsNumber(c)).All(r => r)){
                    Debug.Log("GOT IMPROPER KEY IN OUR SPEECH DICT: " + k);
                    return false;
                }
            }
            else if(k.StartsWith("SC")){
                if(speechJsonDict[k].Length != 3 || 
                !"RSDG".Contains(speechJsonDict[k][0]) || 
                !"THIS".Contains(speechJsonDict[k][1]) || 
                !"THIS".Contains(speechJsonDict[k][2]) ||
                !k.Replace("SC", "").Select(c => char.IsNumber(c)).All(r => r)){
                    Debug.Log("IMPROPER VALUE FOR KEY " + k + " IN OUR SPEECH DICT: " + speechJsonDict[k]);
                    return false;
                }
                else{
                    UI_Characters.Characters curCharacter = jsonCharString2UI_Character(speechJsonDict[k][0]);
                    UI_Characters.Emotions curEmotion = jsonEmotString2UI_Emotion(speechJsonDict[k][1]);
                    UI_Characters.Emotions curPostEmotion = jsonEmotString2UI_Emotion(speechJsonDict[k][2]);
                    if(curCharacter == null || curEmotion == null || curPostEmotion == null){
                        Debug.Log("IMPROPER VALUE FOR KEY " + k + " IN OUR SPEECH DICT: " + speechJsonDict[k]);
                        return false;
                    }
                    else{
                        try{
                            if(!characters2Emotions[curCharacter].Contains(curEmotion) || !characters2Emotions[curCharacter].Contains(curPostEmotion)){
                                Debug.Log("IMPROPER CHARACTER EMOTIONS FOR KEY: " + k);
                                return false;
                            }
                        }
                        catch(System.Exception e){
                            Debug.Log("WTF: " + curCharacter);
                            Debug.Log("WTF: " + curEmotion);
                            Debug.Log("WTF: " + curPostEmotion); 
                            throw e;
                        }
                    }
                }
            }
            else{
                Debug.Log("GOT IMPROPER KEY IN OUR SPEECH DICT: " + k);
                return false;
            }
        }

        if(!foundB){
            Debug.Log("SPEECH DICTIONARY DOESN'T CONTAIN AN IS BLOCKER \"B\" Key");
            return false;
        }

        foreach(string k in speechJsonDict.Keys.Where(k => k.StartsWith("S") && !k.StartsWith("SC"))){
            if(!speechJsonDict.Keys.Contains("SC" + k.Replace("S", ""))){
                Debug.Log("SPEECH KEY " + k + " HAS NO CORRESPONDING CHARACTER INFORMATION KEY IN OUR SPEECH DICT");
                return false;
            }
        }

        foreach(string k in speechJsonDict.Keys.Where(k => k.StartsWith("SC"))){
            if(!speechJsonDict.Keys.Contains("S" + k.Replace("SC", ""))){
                Debug.Log("CHARACTER INFORMATION KEY " + k + " HAS NO CORRESPONDING SPEECH KEY IN OUR SPEECH DICT");
                return false;
            }
        }

        // Going to count up from 1 -> n until we no longer see Sn or SCn keys... if there are other keys in the dictionary that haven't been accounted
        // for (other than "B"), then something is messed up
        Dictionary<string, bool> speechKeyAccountingDict = new Dictionary<string, bool>();
        foreach(string k in speechJsonDict.Keys){
            if(k == "B"){
                speechKeyAccountingDict[k] = true;
            }
            else{
                speechKeyAccountingDict[k] = false;
            }
        }

        int curI = 1;
        while(speechJsonDict.Keys.Contains("S"+curI.ToString()) || speechJsonDict.Keys.Contains("SC"+curI.ToString())){
            if(speechJsonDict.Keys.Contains("S"+curI.ToString())){
                speechKeyAccountingDict["S"+curI.ToString()] = true;
            }
            else{
                Debug.Log("SPEECH DICT MISSING KEY " + "S"+curI.ToString());
                return false;
            }
            
            if(speechJsonDict.Keys.Contains("SC"+curI.ToString())){
                speechKeyAccountingDict["SC"+curI.ToString()] = true;
            }
            else{
                Debug.Log("SPEECH DICT MISSING KEY " + "SC"+curI.ToString());
                return false;
            }
            curI++;
        }

        if(!speechKeyAccountingDict.Values.All(v => v)){
            Debug.Log("SOMETHING IS WRONG WITH THE KEY ORDERING IN THE SPEECH DICT.");
            return false;
        }

        return true;
    }
}
