using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public enum IAP_Product_Type{
    Consumable = 0,
    Non_Consumable = 1,
    Subscription = 2
}


public abstract class IAP_Product_Scriptable_Object : ScriptableObject
{
    public string ProductId { get { return productId; } private set { productId = value; } }
    public string ProductTitle { get { return productTitle; } private set {productTitle = value;}}
    public string ProductDescription { get { return productDescription; } private set {productDescription = value;}}
    public bool ConsumePurchase {get {return consumePurchase; } private set {consumePurchase = value;}}
    public IAP_Product_Type ProductType { get { return productType; } private set {productType = value;}}

    [SerializeField]
    private string productId;
    [SerializeField]
    private string productTitle;
    [SerializeField]
    private string productDescription;
    [SerializeField]
    private bool consumePurchase;
    [SerializeField]
    private IAP_Product_Type productType;


    public virtual void OnPurchaseComplete(Product product){
        Debug.Log("We bought " + ProductId + " ... " + product.definition.id);
    }

    public virtual void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        Debug.Log("PURCHASE OF: " + ProductId + " ... " + product.definition.id + " FAILED DUE TO... " + reason);
    }
}
