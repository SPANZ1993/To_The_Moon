using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Linq;

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

            // Unequip the rest of the outfits
            foreach(IAP_Product_Scriptable_Object_Nonconsumable product in IAP_Manager.instance.ownedNonConsumableProductsIds.Select(id => IAP_Manager.instance.getProductObjectByID(id))){
                if(typeof(IAP_Product_Robot_Outfit).IsAssignableFrom(product.GetType()) && product.ProductId != ProductId){
                    ((IAP_Product_Robot_Outfit)(System.Object)product).OnUnequip();
                }
            }
        }
        else{
            if(!Audio_Manager.instance.IsPlaying("UI_Button_No_Effect")){
                Audio_Manager.instance.Play("UI_Button_No_Effect");
            }
        }
    }

    public override void OnUnequip(){
        base.Equipped = false;
    }

}
