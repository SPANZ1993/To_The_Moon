using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public abstract class IAP_Product_Scriptable_Object_Consumable : IAP_Product_Scriptable_Object
{

    public override void OnPurchaseComplete(Product product){
        base.OnPurchaseComplete(product);
    }

    public override void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        base.OnPurchaseFailed(product, reason);
    }
}
