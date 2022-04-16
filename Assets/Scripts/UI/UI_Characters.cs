using System.Collections;
using System.Collections.Generic;

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


        public Characters2Names(){
            RobotName = "Robo McTwist";
            GuyName = "Sir Stonkman";
            DogName = "Dogerino";
            GorillaName = "Baramhay";
        }

        private string getCharacterNames(Characters c){
            switch (c)
            {
                case Characters.Robot:
                return RobotName;
                break;

                case Characters.Guy:
                return GuyName;
                break;

                case Characters.Dog:
                return DogName;
                break;

                case Characters.Gorilla:
                return GorillaName;
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
    
}
