using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Localization.Settings;
 
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
                    priceText.SetText(product.metadata.localizedPriceString);
            }
            catch(System.Exception e){
                passed = false;
            }

            try{
                if (titleText != null)
                    titleText.SetText(product.metadata.localizedTitle);
            }
            catch(System.Exception e){
                passed = false;
            }

            try{
                if (descriptionText != null)
                    descriptionText.SetText(product.metadata.localizedDescription);
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
}