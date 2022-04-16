using System.Collections;
using System.Collections.Generic;

using System;
using UI_Characters;

public class Speech_Object
{
    public bool is_blocker {get; private set;} // Will all touch be disabled while this speech is displayed??
    public List<string> speech_strings {get; private set;}
    public List<Characters> characters {get; private set;}
    public List<Emotions> emotions {get; private set;}
    public List<Emotions> postEmotions {get; private set;}
    

    public Speech_Object(bool IsBlocker, List<string> Speech_Strings_List, List<Characters> Characters_List, List<Emotions> Emotions_List, List<Emotions> Post_Emotions_List){
        is_blocker = IsBlocker;
        speech_strings = Speech_Strings_List;
        characters = Characters_List;
        emotions = Emotions_List;
        postEmotions = Post_Emotions_List;

        if (!(speech_strings.Count == characters.Count && characters.Count == emotions.Count && emotions.Count == postEmotions.Count)){
            throw new ArgumentException("Lists Used To Build Speech_Object must be of all same length.");
        }

        //TODO: CHECK TO MAKE SURE THAT A VALID LIST OF CHARACTERS AND EMOTIONS WAS PASSED.
    }
}
