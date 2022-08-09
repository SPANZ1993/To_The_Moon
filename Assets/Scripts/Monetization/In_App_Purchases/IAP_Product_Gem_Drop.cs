using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

using TMPro; // REMOVE

[CreateAssetMenu(fileName = "IAP_Gem_Drop", menuName = "ScriptableObjects/IAP_Product_Gem_Drop", order = 3)]
public class IAP_Product_Gem_Drop : IAP_Product_Scriptable_Object_Consumable
{
    public double NumGems { get { return numGems; } private set {numGems = value;}}
    
    [SerializeField]
    private double numGems;



    public override void OnPurchaseComplete(Product product){
        Game_Manager.instance.gems += NumGems;
        try{
            base.OnPurchaseComplete(product); // Do this at the end
        }
        catch(System.Exception e){
            if(GameObject.Find("App_State_Text") != null){
                GameObject.Find("App_State_Text").GetComponent<TextMeshProUGUI>().text = e.ToString();
            }
        }
    }

    public override void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        base.OnPurchaseFailed(product, reason);
    }
}
