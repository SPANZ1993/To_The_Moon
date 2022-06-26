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
 
    public void Initialize()
    {
        //Debug.Log("INITIALIZING HERE " + attachedButton.productId);
        var product = CodelessIAPStoreListener.Instance.GetProduct(attachedButton.productId);
 
        if (priceText != null)
            priceText.SetText(product.metadata.localizedPriceString);

        if (titleText != null)
            titleText.SetText(product.metadata.localizedTitle);
 
        if (descriptionText != null)
            descriptionText.SetText(product.metadata.localizedDescription);
    }

    public void Initialize(TextMeshProUGUI PriceText, TextMeshProUGUI TitleText, TextMeshProUGUI DescriptionText){
        priceText = PriceText;
        titleText = TitleText;
        descriptionText = DescriptionText;
        Initialize();
    }
}