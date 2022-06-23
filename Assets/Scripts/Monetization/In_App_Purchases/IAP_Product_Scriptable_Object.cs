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

    [SerializeField]
    private string productId;
    [SerializeField]
    private string productTitle;
    [SerializeField]
    private string productDescription;
    [SerializeField]
    private bool consumePurchase;


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
}
