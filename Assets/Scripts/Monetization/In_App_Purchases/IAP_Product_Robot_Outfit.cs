using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[CreateAssetMenu(fileName = "IAP_Robot_Outfit", menuName = "ScriptableObjects/IAP_Product_Robot_Outfit", order = 2)]
public class IAP_Product_Robot_Outfit : IAP_Product_Scriptable_Object_Nonconsumable
{
    public Robot_Outfit_Scriptable_Object RobotOutfit { get { return robotOutfitSprite; } private set { robotOutfitSprite = value; } }

    [SerializeField]
    private Robot_Outfit_Scriptable_Object robotOutfitSprite;


    public override void OnPurchaseComplete(Product product){
        base.OnPurchaseComplete(product); // Do this at the end
    }

    public override void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        base.OnPurchaseFailed(product, reason);
    }

    public override void _onEquip(){
        if(Robot_Outfit_Manager.instance.CurOutfitID != RobotOutfit.OutfitId){
            // TODO: Happy doot
            Robot_Outfit_Manager.instance.setCurRobotOutfitId(RobotOutfit.OutfitId);
            Equipped = true;
        }
        else{
            // TODO: Nothing happened doot
        }
    }

    public override void OnUnequip(){
    
    }

}
