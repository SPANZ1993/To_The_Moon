using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[CreateAssetMenu(fileName = "Robot_Outfit", menuName = "ScriptableObjects/IAP_Product_Robot_Outfit", order = 2)]
public class IAP_Product_Robot_Outfit : IAP_Product_Scriptable_Object
{
    public Sprite RobotOutfitSprite { get { return robotOutfitSprite; } private set { robotOutfitSprite = value; } }

    [SerializeField]
    private Sprite robotOutfitSprite;


    public override void OnPurchaseComplete(Product product){
        base.OnPurchaseComplete(product);
    }

    public override void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        base.OnPurchaseFailed(product, reason);
    }


}
