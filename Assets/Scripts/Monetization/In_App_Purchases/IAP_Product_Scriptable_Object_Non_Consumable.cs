using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

using System;
using System.Linq;

public abstract class IAP_Product_Scriptable_Object_Nonconsumable : IAP_Product_Scriptable_Object
{

    public bool OwnedByDefault {get {return ownedByDefault; } private set {ownedByDefault = value;} }
    public bool Equippable {get {return equippable; } private set {equippable = value;} }
    public bool Equipped {get {return equipped; } protected set {equipped = value;} }
    public string EquipButtonString {get {return equipButtonString; } private set {equipButtonString = value;}}
    public string EquippedButtonString {get {return equippedButtonString; } private set {equippedButtonString = value;}}

    [SerializeField]
    private bool ownedByDefault;
    [SerializeField]
    private bool equippable;
    [SerializeField]
    private bool equipped;
    [SerializeField]
    private string equipButtonString;
    [SerializeField]
    private string equippedButtonString;

    public override void OnPurchaseComplete(Product product){
        if(!IAP_Manager.instance.ownedNonConsumableProductsIds.Contains(base.ProductId)){
            Debug.Log("ADDING: " + base.ProductId + " TO LIST OF OWNED NONCONSUMABLES");
            IAP_Manager.instance.ownedNonConsumableProductsIds.Add(base.ProductId);
            IAP_Manager.instance.lastNonConsumableProductIdBuyTime = Game_Manager.instance.gameTimeUnix;
        }
        else{
            Debug.Log("Oh shit... we double bought");
        }
        Debug.Log("IS THIS PRODUCT " + base.ProductId + " OWNED NOW? : " + IAP_Manager.instance.ownedNonConsumableProductsIds.Contains(base.ProductId));
        base.OnPurchaseComplete(product);
    }

    public override void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        base.OnPurchaseFailed(product, reason);
    }

    public virtual void _onEquip(){
        if(equippable){
            throw new NotImplementedException("Must implement equip function for equippable nonconsumable product" + ProductId);
        }
        else{
            throw new InvalidOperationException("Attempting to equip nonconsumable nonequippable product " + ProductId);
        }
    }

    public void OnEquip(){
        _onEquip();
        Equipped = true;
        Debug.Log("A ARE WE EQUIPPED? " + Equipped + " " + ProductId);
        IAP_Manager.instance.updateAllShopPanels();
    }

    public virtual void OnUnequip(){
        if(equippable){
            throw new NotImplementedException("Must implement equip function for equippable nonconsumable product" + ProductId);
        }
        else{
            throw new InvalidOperationException("Attempting to equip nonconsumable nonequippable product " + ProductId);
        }
    }
}

