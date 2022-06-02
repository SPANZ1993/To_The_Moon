using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement;
 

public enum Languages {
    English = 0,
    Spanish = 1
}

public class Localization_Manager : MonoBehaviour
{
    //TODO: load localized string
    LocalizedString myLocalizedString = new LocalizedString();
    [SerializeField]
    private List<Locale> localesList = new List<Locale>();

    public Languages currentLanguage {get; private set;}

    [SerializeField]
    private bool setEnglishTmp, setSpanishTmp = false; // REMOVE



    public static Localization_Manager instance;


    public delegate void LocaleChanged();
    public static event LocaleChanged LocaleChangedInfo;


    void Awake(){
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            setCurrentLanguage();
        }
        else{
            Destroy(this.gameObject);
        }
    }

    private void setCurrentLanguage(){
        for(int i=0; i<localesList.Count; i++){
            if(LocalizationSettings.SelectedLocale == localesList[i]){
                currentLanguage = (Languages)i;
            }
        }
    }


    private void ChangeLanguage(Languages language)
    {
        LocalizationSettings.SelectedLocale = localesList[(int)language];
        if (LocaleChangedInfo != null){
            LocaleChangedInfo();
        }
        setCurrentLanguage();
    }

    // private IEnumerator _changeLanguageTest(){
    //     yield return new WaitForSeconds(5);
    //     ChangeLanguage(Languages.Spanish);
    // }

    public string GetLocalizedString(string table, string entryref){
        myLocalizedString.TableReference = table;
        myLocalizedString.TableEntryReference = entryref;
        return myLocalizedString.GetLocalizedString();
    }
    

    void Start(){
        // StartCoroutine(_changeLanguageTest());
    }

    void Update(){
        if (setEnglishTmp){ // REMOVE
            if (LocalizationSettings.SelectedLocale != localesList[(int)Languages.English]){
                ChangeLanguage(Languages.English);
            }
            setEnglishTmp = false;
        }
        else if (setSpanishTmp){
            if (LocalizationSettings.SelectedLocale != localesList[(int)Languages.Spanish]){
                ChangeLanguage(Languages.Spanish);
            }
        }
        setSpanishTmp = false;
    }
}
