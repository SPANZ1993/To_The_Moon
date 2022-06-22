using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IAP_Product_Type{
    Consumable = 0,
    Non_Consumable = 1,
    Subscription = 2
}


public class IAP_Product_Scriptable_Object : ScriptableObject
{
    public string ProductId { get { return productId; } private set { productId = value; } }
    public string ProductTitle { get { return productTitle; } private set {productTitle = value;}}
    public string ProductDescription { get { return productDescription; } private set {productDescription = value;}}
    public IAP_Product_Type ProductType { get { return productType; } private set {productType = value;}}

    [SerializeField]
    private string productId;
    [SerializeField]
    private string productTitle;
    [SerializeField]
    private string productDescription;
    [SerializeField]
    private IAP_Product_Type productType;



}
