using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;
using System.Linq;

public class IAP_Manager : MonoBehaviour
{
    [SerializeField]
    private IAP_Product_Scriptable_Object[] activeProducts;

    //private IAP_Product_Scriptable_Object[] purchasedProducts;
    //private IAP_Product_Scriptable_Object[] inactiveProducts; // Products that exist, but are not currently for sale


    public Dictionary<IAP_Product_Scriptable_Object, GameObject> activeProductsToShopPanel {get; private set;}






    public static IAP_Manager instance;
    
    void Awake()
    {
        if (!instance){
            instance = this;
            activeProductsToShopPanel = new Dictionary<IAP_Product_Scriptable_Object, GameObject>();
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }



    public void OnPurchaseComplete(Product product){
        IAP_Product_Scriptable_Object product_scriptable_obj = getProductObjectByID(product.definition.id);
        Debug.Log("PRODUCT IS: " + product_scriptable_obj.ProductTitle);
        
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        Debug.Log("PURCHASE OF: " + product.definition.id + " FAILED DUE TO... " + reason);

    }


    public IAP_Product_Scriptable_Object getProductObjectByID(string productId){
        IAP_Product_Scriptable_Object product = Array.Find(activeProducts, p => p.ProductId == productId);
        return product;
    }

    public void addPanelsToShop(){
        foreach(IAP_Product_Scriptable_Object product in activeProducts){
            if(!activeProductsToShopPanel.Keys.Contains(product) || activeProductsToShopPanel[product] == null){
                Debug.Log("ADDING NEW PANEL: " + product.ProductTitle);
                GameObject panel = UI_Controller.instance.addShopPanel(product);
                activeProductsToShopPanel[product] = panel;
            }
            else{
                Debug.Log("ADDING OLD PANEL: " + product.ProductTitle);
                GameObject panel = UI_Controller.instance.addShopPanel(activeProductsToShopPanel[product]);
                activeProductsToShopPanel[product] = panel;
            }
        }
    }


    public void initializeIAPButton(GameObject buttonObj, IAP_Product_Scriptable_Object product){
        initializeIAPButton(buttonObj.GetComponent<IAPButton>(), product);
    }


    public void initializeIAPButton(IAPButton button, IAP_Product_Scriptable_Object product){
        Debug.Log("A " + product.ProductId);
        button.productId = product.ProductId;
        Debug.Log("B");
        button.consumePurchase = product.ConsumePurchase;

        button.onPurchaseComplete.RemoveAllListeners();
        button.onPurchaseComplete.AddListener(product.OnPurchaseComplete);

        button.onPurchaseFailed.RemoveAllListeners(); 
        button.onPurchaseFailed.AddListener(product.OnPurchaseFailed);

        // If we start with the button enabled, but we don't have the productId set yet, Unity doesn't like that.
        // So start with the button off, then turn it on after initialization
        if(!button.enabled){
            button.enabled = true;
        }
    }

}
