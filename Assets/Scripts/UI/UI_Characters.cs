using System.Collections;
using System.Collections.Generic;

using System;
using System.Linq;

namespace UI_Characters
{

    public enum Characters
    {
        Robot = 0,
        Guy = 1,
        Dog = 2,
        Gorilla = 3
    }

    public enum Emotions
    {
        Idle = 0,
        Talking = 1,
        Happy = 2,
        Sad = 3
    }


    public class Characters2Emotions{

        Emotions[] RobotArr = new Emotions[] {Emotions.Idle, Emotions.Talking, Emotions.Sad};
        Emotions[] GuyArr = new Emotions[] {Emotions.Talking, Emotions.Happy, Emotions.Sad};
        Emotions[] DogArr = new Emotions[] {Emotions.Talking, Emotions.Happy, Emotions.Sad};
        Emotions[] GorillaArr = new Emotions [] {Emotions.Talking, Emotions.Happy, Emotions.Sad};


        private Emotions[] getCharacterEmotions(Characters c){
            switch (c)
            {
                case Characters.Robot:
                    return RobotArr;
                    break;

                case Characters.Guy:
                    return GuyArr;
                    break;

                case Characters.Dog:
                    return DogArr;
                    break;

                case Characters.Gorilla:
                    return GorillaArr;
                    break;

                default:
                    return new Emotions[] {};
                    break;
            }
        }

        public bool characterHasEmotion(Characters c, Emotions e){
            return this[c].Contains(e);
        }
        
        public Emotions[] this[Characters c]{
            get => getCharacterEmotions(c);
        }
    }






    public class Characters2Names{

        string RobotName;
        string GuyName;
        string DogName;
        string GorillaName;


        private string getCharacterNames(Characters c){
            string characters_table = "Characters";
            switch (c)
            {
                case Characters.Robot:
                    return Localization_Manager.instance.GetLocalizedString(characters_table, "Character.Robot.Name");
                    break;

                case Characters.Guy:
                    return Localization_Manager.instance.GetLocalizedString(characters_table, "Character.Stonks.Name");
                    break;

                case Characters.Dog:
                    return Localization_Manager.instance.GetLocalizedString(characters_table, "Character.Dog.Name");
                    break;

                case Characters.Gorilla:
                    return Localization_Manager.instance.GetLocalizedString(characters_table, "Character.Gorilla.Name");
                    break;

                default:
                    return "";
                    break;
            }
        }
        
        public string this[Characters c]{
            get => getCharacterNames(c);
        }
    }
    
    public class Characters2DisplayNames{

        string RobotName;
        string GuyName;
        string DogName;
        string GorillaName;


        private string getCharacterNames(Characters c){
            string characters_table = "Characters";
            switch (c)
            {
                case Characters.Robot:
                    return Localization_Manager.instance.GetLocalizedString(characters_table, "Character.Robot.DisplayName");
                    break;

                case Characters.Guy:
                    return Localization_Manager.instance.GetLocalizedString(characters_table, "Character.Stonks.DisplayName");
                    break;

                case Characters.Dog:
                    return Localization_Manager.instance.GetLocalizedString(characters_table, "Character.Dog.DisplayName");
                    break;

                case Characters.Gorilla:
                    return Localization_Manager.instance.GetLocalizedString(characters_table, "Character.Gorilla.DisplayName");
                    break;

                default:
                    return "";
                    break;
            }
        }
        
        public string this[Characters c]{
            get => getCharacterNames(c);
        }
    }


    
    public class CharacterEmotions2SpeechSounds{

        public static Sound[] getSounds(Characters c, Emotions e){
            string[] speechStrs = new string[0];
            switch (c)
            {
                case Characters.Robot:
                    if(e == Emotions.Idle || e == Emotions.Talking){
                        speechStrs = new string[]{
                            "Speech_Beep_Robot_Neutral_0",
                            "Speech_Beep_Robot_Neutral_1",
                            "Speech_Beep_Robot_Neutral_2",
                            "Speech_Beep_Robot_Neutral_3",
                            "Speech_Beep_Robot_Neutral_4",
                            "Speech_Beep_Robot_Neutral_5",
                            "Speech_Beep_Robot_Neutral_6",
                            "Speech_Beep_Robot_Neutral_7",
                            "Speech_Beep_Robot_Neutral_8",
                            "Speech_Beep_Robot_Neutral_9"
                        };
                    }
                    else if(e == Emotions.Happy){
                        speechStrs = new string[]{
                            "Speech_Beep_Robot_Happy_0",
                            "Speech_Beep_Robot_Happy_1",
                            "Speech_Beep_Robot_Happy_2",
                            "Speech_Beep_Robot_Happy_3",
                            "Speech_Beep_Robot_Happy_4",
                            "Speech_Beep_Robot_Happy_5",
                            "Speech_Beep_Robot_Happy_6",
                            "Speech_Beep_Robot_Happy_7",
                            "Speech_Beep_Robot_Happy_8",
                            "Speech_Beep_Robot_Happy_9"
                        };
                    }
                    else if(e == Emotions.Sad){
                        speechStrs = new string[]{
                            "Speech_Beep_Robot_Sad_0",
                            "Speech_Beep_Robot_Sad_1",
                            "Speech_Beep_Robot_Sad_2",
                            "Speech_Beep_Robot_Sad_3",
                            "Speech_Beep_Robot_Sad_4",
                            "Speech_Beep_Robot_Sad_5",
                            "Speech_Beep_Robot_Sad_6",
                            "Speech_Beep_Robot_Sad_7",
                            "Speech_Beep_Robot_Sad_8",
                            "Speech_Beep_Robot_Sad_9"
                        };
                    }
                    break;

                case Characters.Guy:
                    if(e == Emotions.Talking){
                        speechStrs = new string[]{
                            "Speech_Beep_Stonks_Neutral_0",
                            "Speech_Beep_Stonks_Neutral_1",
                            "Speech_Beep_Stonks_Neutral_2",
                            "Speech_Beep_Stonks_Neutral_3",
                            "Speech_Beep_Stonks_Neutral_4",
                            "Speech_Beep_Stonks_Neutral_5",
                            "Speech_Beep_Stonks_Neutral_6",
                            "Speech_Beep_Stonks_Neutral_7",
                            "Speech_Beep_Stonks_Neutral_8",
                            "Speech_Beep_Stonks_Neutral_9"
                        };
                    }
                    else if(e == Emotions.Happy){
                        speechStrs = new string[]{
                            "Speech_Beep_Stonks_Happy_0",
                            "Speech_Beep_Stonks_Happy_1",
                            "Speech_Beep_Stonks_Happy_2",
                            "Speech_Beep_Stonks_Happy_3",
                            "Speech_Beep_Stonks_Happy_4",
                            "Speech_Beep_Stonks_Happy_5",
                            "Speech_Beep_Stonks_Happy_6",
                            "Speech_Beep_Stonks_Happy_7",
                            "Speech_Beep_Stonks_Happy_8",
                            "Speech_Beep_Stonks_Happy_9"
                        };
                    }
                    else if(e == Emotions.Sad){
                        speechStrs = new string[]{
                            "Speech_Beep_Stonks_Sad_0",
                            "Speech_Beep_Stonks_Sad_1",
                            "Speech_Beep_Stonks_Sad_2",
                            "Speech_Beep_Stonks_Sad_3",
                            "Speech_Beep_Stonks_Sad_4",
                            "Speech_Beep_Stonks_Sad_5",
                            "Speech_Beep_Stonks_Sad_6",
                            "Speech_Beep_Stonks_Sad_7",
                            "Speech_Beep_Stonks_Sad_8",
                            "Speech_Beep_Stonks_Sad_9"
                        };
                    }
                    break;

                case Characters.Dog:
                    if(e == Emotions.Talking){
                        speechStrs = new string[]{
                            "Speech_Beep_Dog_Neutral_0",
                            "Speech_Beep_Dog_Neutral_1",
                            "Speech_Beep_Dog_Neutral_2",
                            "Speech_Beep_Dog_Neutral_3",
                            "Speech_Beep_Dog_Neutral_4",
                            "Speech_Beep_Dog_Neutral_5",
                            "Speech_Beep_Dog_Neutral_6",
                            "Speech_Beep_Dog_Neutral_7",
                            "Speech_Beep_Dog_Neutral_8",
                            "Speech_Beep_Dog_Neutral_9"
                        };
                    }
                    else if(e == Emotions.Happy){
                        speechStrs = new string[]{
                            "Speech_Beep_Dog_Happy_0",
                            "Speech_Beep_Dog_Happy_1",
                            "Speech_Beep_Dog_Happy_2",
                            "Speech_Beep_Dog_Happy_3",
                            "Speech_Beep_Dog_Happy_4",
                            "Speech_Beep_Dog_Happy_5",
                            "Speech_Beep_Dog_Happy_6",
                            "Speech_Beep_Dog_Happy_7",
                            "Speech_Beep_Dog_Happy_8",
                            "Speech_Beep_Dog_Happy_9"
                        };
                    }
                    else if(e == Emotions.Sad){
                        speechStrs = new string[]{
                            "Speech_Beep_Dog_Sad_0",
                            "Speech_Beep_Dog_Sad_1",
                            "Speech_Beep_Dog_Sad_2",
                            "Speech_Beep_Dog_Sad_3",
                            "Speech_Beep_Dog_Sad_4",
                            "Speech_Beep_Dog_Sad_5",
                            "Speech_Beep_Dog_Sad_6",
                            "Speech_Beep_Dog_Sad_7",
                            "Speech_Beep_Dog_Sad_8",
                            "Speech_Beep_Dog_Sad_9"
                        };
                    }
                    break;

                case Characters.Gorilla:
                    if(e == Emotions.Talking){
                        speechStrs = new string[]{
                            "Speech_Beep_Gorilla_Neutral_0",
                            "Speech_Beep_Gorilla_Neutral_1",
                            "Speech_Beep_Gorilla_Neutral_2",
                            "Speech_Beep_Gorilla_Neutral_3",
                            "Speech_Beep_Gorilla_Neutral_4",
                            "Speech_Beep_Gorilla_Neutral_5",
                            "Speech_Beep_Gorilla_Neutral_6",
                            "Speech_Beep_Gorilla_Neutral_7",
                            "Speech_Beep_Gorilla_Neutral_8",
                            "Speech_Beep_Gorilla_Neutral_9"
                        };
                    }
                    else if(e == Emotions.Happy){
                        speechStrs = new string[]{
                            "Speech_Beep_Gorilla_Happy_0",
                            "Speech_Beep_Gorilla_Happy_1",
                            "Speech_Beep_Gorilla_Happy_2",
                            "Speech_Beep_Gorilla_Happy_3",
                            "Speech_Beep_Gorilla_Happy_4",
                            "Speech_Beep_Gorilla_Happy_5",
                            "Speech_Beep_Gorilla_Happy_6",
                            "Speech_Beep_Gorilla_Happy_7",
                            "Speech_Beep_Gorilla_Happy_8",
                            "Speech_Beep_Gorilla_Happy_9"
                        };
                    }
                    else if(e == Emotions.Sad){
                        speechStrs = new string[]{
                            "Speech_Beep_Gorilla_Sad_0",
                            "Speech_Beep_Gorilla_Sad_1",
                            "Speech_Beep_Gorilla_Sad_2",
                            "Speech_Beep_Gorilla_Sad_3",
                            "Speech_Beep_Gorilla_Sad_4",
                            "Speech_Beep_Gorilla_Sad_5",
                            "Speech_Beep_Gorilla_Sad_6",
                            "Speech_Beep_Gorilla_Sad_7",
                            "Speech_Beep_Gorilla_Sad_8",
                            "Speech_Beep_Gorilla_Sad_9"
                        };
                    }
                    else{
                        //print("COULD'NT GET SPEECH SOUNDS FOR " + c + " --- " + e);
                        return new Sound[0];
                    }
                    break;

                default:
                    //Debug.Log("COULD'NT GET SPEECH SOUNDS FOR " + c + " --- " + e);
                    return new Sound[0];
                    break;
            }
            return speechStrs.Select(str => Audio_Manager.instance.GetSound(str)).ToArray();
        }
    }

}
