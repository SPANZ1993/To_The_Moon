using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


[CreateAssetMenu(fileName = "IAP_Unlockable", menuName = "ScriptableObjects/IAP_Product_Unlockable", order = 3)]
public class IAP_Product_Unlockable : IAP_Product_Scriptable_Object_Nonconsumable
{
    public int UnlockableId { get { return unlockableId; } private set { unlockableId = value; } }

    [SerializeField]
    private int unlockableId;


    public override void OnPurchaseComplete(Product product){
        Debug.Log("UNLOCKED: " + UnlockableId);
        base.OnPurchaseComplete(product); // Do this at the end
    }

    public override void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        base.OnPurchaseFailed(product, reason);
    }
}
