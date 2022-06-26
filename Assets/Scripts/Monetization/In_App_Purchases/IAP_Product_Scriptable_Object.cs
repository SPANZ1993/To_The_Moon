using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;



public abstract class IAP_Product_Scriptable_Object : ScriptableObject, System.IEquatable<IAP_Product_Scriptable_Object>
{
    public string ProductId { get { return productId; } private set { productId = value; } }
    public string ProductTitle { get { return productTitle; } private set {productTitle = value;}}
    public string ProductDescription { get { return productDescription; } private set {productDescription = value;}}
    public bool ConsumePurchase {get {return consumePurchase; } private set {consumePurchase = value;}}
    public Sprite PreviewSprite { get { return previewSprite; } private set { previewSprite = value; } }






    [SerializeField]
    private string productId;
    [SerializeField]
    private string productTitle;
    [SerializeField]
    private string productDescription;
    [SerializeField]
    private bool consumePurchase;
    [SerializeField]
    private Sprite previewSprite;



    public virtual void OnPurchaseComplete(Product product){
        Debug.Log("We bought " + ProductId + " ... " + product.definition.id);
        IAP_Manager.instance.updateAllShopPanels();
    }

    public virtual void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        Debug.Log("PURCHASE OF: " + ProductId + " ... " + product.definition.id + " FAILED DUE TO... " + reason);
    }


    public bool Equals(IAP_Product_Scriptable_Object other){
        return ProductId == other.productId;
    }

    // If we have any products that have specific requirements in order for them to be active, put them here
    // For example, we only want to display Patron-Only Products if the User is a Patron
    public static bool SpecialRequirementsMet(IAP_Product_Scriptable_Object product){
        if((product.ProductId == "com.eggkidgames.blockchainblastoff.robotOutfitPatron" || product.ProductId == "cacapeepee") && !Game_Manager.instance.isPatron){
            Debug.Log("IN HERE");
            return false;
        }

        return true;
    }
}
