using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Linq;

[CreateAssetMenu(fileName = "IAP_Ship_Skin", menuName = "ScriptableObjects/IAP_Product_Ship_Skin", order = 2)]
public class IAP_Product_Ship_Skin : IAP_Product_Scriptable_Object_Nonconsumable
{
    public Ship_Skin_Scriptable_Object ShipSkin { get { return shipSkin; } private set { shipSkin = value; } }

    [SerializeField]
    private Ship_Skin_Scriptable_Object shipSkin;


    public override void OnPurchaseComplete(Product product){
        base.OnPurchaseComplete(product); // Do this at the end
    }

    public override void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        base.OnPurchaseFailed(product, reason);
    }

    public override void _onEquip(){
        if(Ship_Skin_Manager.instance.CurSkinID != ShipSkin.SkinId){
            // TODO: Happy doot
            Ship_Skin_Manager.instance.setCurShipSkinId(ShipSkin.SkinId);

            // Unequip the rest of the skins
            foreach(IAP_Product_Scriptable_Object_Nonconsumable product in IAP_Manager.instance.ownedNonConsumableProductsIds.Select(id => IAP_Manager.instance.getProductObjectByID(id))){
                if(typeof(IAP_Product_Ship_Skin).IsAssignableFrom(product.GetType()) && product.ProductId != ProductId){
                    ((IAP_Product_Ship_Skin)(System.Object)product).OnUnequip();
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