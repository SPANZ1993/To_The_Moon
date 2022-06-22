using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAP_Manager : MonoBehaviour
{
    [SerializeField]
    private IAP_Product_Scriptable_Object[] activeProducts;


    public static IAP_Manager instance;
    
    void Awake()
    {
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }



    public void OnPurchaseComplete(Product product){
        IAP_Product_Scriptable Object product_scriptable_obj = getProductObjectByID(product.definition.id);
        Debug.Log("PRODUCT IS: " + product_scriptable_obj.ProductTitle);
        
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        Debug.Log("PURCHASE OF: " + product.definition.id + " FAILED DUE TO... " + reason);

    }


    public IAP_Product_Scriptable_Object getProductObjectByID(string productId){
        IAP_Product_Scriptable_Object product = Array.Find(activeProducts, p => p.ProductId == productId);
        return product;
    }

}
