using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

using System.IO;
using System.Linq;

public class BadwordsFilter : MonoBehaviour
{


    public static BadwordsFilter instance;


    void Awake(){
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            //Debug.Log("YEET CONTAINS BAD WORDS? " + checkWordContainsBadWords("YEET"));
            //Debug.Log("Bags of bDIcks Sandwiches CONTAINS BAD WORDS? " + checkWordContainsBadWords("Bags of bDIcks Sandwiches"));
        }
        else{
            Destroy(this.gameObject);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool checkWordContainsBadWords(string word){


        List<string> getBadWordsList(Languages language){

            string languageFile = "";
            if(language == Languages.English){
                languageFile = "Assets/Localization/Bad_Words_Lists/BadWordsEnglish.csv";
            }
            else if(language == Languages.Spanish){
                Debug.Log("Spanish Not Yet Implemented");
                return new List<string>();
            }
            else{
                Debug.Log("Couldn't get bad words for " + language);
                return new List<string>();
            }
            
            
            List<string> badwords = new List<string>();
            using(var reader = new StreamReader(languageFile))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    badwords.Add(values[1]);
                }
            }
            badwords.RemoveAt(0);
            return badwords;
        }
        Languages language = Localization_Manager.instance.currentLanguage;
        List<string> badwords = getBadWordsList(language);
        //Debug.Log(string.Join(", ", badwords));

        return badwords.ToArray().Select(badword => word.ToLower().Contains(badword.ToLower())).ToArray().Any(result => result == true);
    }

    //speechStrs.Select(str => Audio_Manager.instance.GetSound(str)).ToArray();
}
