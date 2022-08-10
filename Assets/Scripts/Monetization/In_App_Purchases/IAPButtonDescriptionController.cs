using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Localization.Settings;
 
using System;
using System.Linq;

[RequireComponent(typeof(IAPButton))]
public class IAPButtonDescriptionController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
 
    private IAPButton attachedButton;
 
    private void Awake()
    {
        attachedButton = GetComponent<IAPButton>();
        //Initialize();
    }

    public void Clear(){
        priceText = null;
        titleText = null;
        descriptionText = null;
    }
 
    public bool Initialize()
    {
        bool passed = true;
        Product product = null;
        try{
            //Debug.Log("INITIALIZING HERE " + attachedButton.productId);
            try{
                product = CodelessIAPStoreListener.Instance.GetProduct(attachedButton.productId);
            }
            catch(System.Exception e){
                passed = false;
            }

            try{
                if (priceText != null)
                    priceText.SetText(trimStoreString(product.metadata.localizedPriceString));
            }
            catch(System.Exception e){
                passed = false;
            }

            try{
                if (titleText != null){
                    titleText.SetText(trimStoreString(product.metadata.localizedTitle));
                    if(GameObject.Find("App_State_Text")!=null){
                         GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text += "\n<--" + product.metadata.localizedTitle + " --- " + trimStoreString(product.metadata.localizedTitle) + "-->";
                    }

                }
            }
            catch(System.Exception e){
                passed = false;
            }

            try{
                if (descriptionText != null)
                    descriptionText.SetText(trimStoreString(product.metadata.localizedDescription));
            }
            catch(System.Exception e){
                passed = false;
            }
        }
        catch(System.Exception e){
            // if(GameObject.Find("App_State_Text")!=null){
            //     GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text += e.ToString();
            // }
            passed = false;
        }
        return passed;
    }

    public bool Initialize(TextMeshProUGUI PriceText, TextMeshProUGUI TitleText, TextMeshProUGUI DescriptionText){
        priceText = PriceText;
        titleText = TitleText;
        descriptionText = DescriptionText;
        return Initialize();
    }




    // On some platforms (Android) the product.metadata values has extraneous information.. This should trim that.
    public static string trimStoreString(string s, string breakstr = "com.EggKidGames"){
        // Return the part of s that is before breakstr and is not all punctuation
        
        string[] breakstrs = new string[]{breakstr};
        s = s.Split(breakstrs, StringSplitOptions.None)[0];
        
        string[] tmps = s.Split(new string[] {" " , "\n", "\r"}, StringSplitOptions.None);
        while(tmps[tmps.Length-1].Select(c => Char.IsPunctuation(c) || char.IsSymbol(c)).All(b => b)){
            tmps = tmps.Take(tmps.Length - 1).ToArray();
        }
        
        return String.Join(" ", tmps).Trim();
    }


}