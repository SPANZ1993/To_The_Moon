using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[CreateAssetMenu(fileName = "Gem_Drop", menuName = "ScriptableObjects/IAP_Product_Gem_Drop", order = 3)]
public class IAP_Product_Gem_Drop : IAP_Product_Scriptable_Object
{
    public double NumGems { get { return numGems; } private set {numGems = value;}}
    
    [SerializeField]
    private double numGems;



    public override void OnPurchaseComplete(Product product){
        base.OnPurchaseComplete(product);
    }

    public override void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        base.OnPurchaseFailed(product, reason);
    }
}
