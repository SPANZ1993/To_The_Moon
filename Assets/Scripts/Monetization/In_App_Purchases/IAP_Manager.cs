using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;



// NOTE TO SELF: DO NOT USE CODELESS IAP NEXT TIME
public class IAP_Manager : MonoBehaviour
{

    [SerializeField]
    private IAP_Product_Scriptable_Object[] allProducts; // All products including ones that aren't currently available for sale... defines order in UI
    [SerializeField]
    private IAP_Product_Scriptable_Object[] activeProducts;

    [SerializeField]
    private GameObject shopPanelPrefab;
    
    public List<string> ownedNonConsumableProductsIds; // Non-Consumable Products That We Own.. We don't want to sell these twice, but allow them to be equipped instead
    public double lastNonConsumableProductIdBuyTime = -1; // Used to update the menu to prevent double buys of non-consumable items


    public Dictionary<IAP_Product_Scriptable_Object, GameObject> activeProductsToShopPanel {get; private set;}

    private Dictionary<IAP_Product_Scriptable_Object, GameObject> inactiveProductsToShopPanel;

    public GameObject TMP_Button_Panel_Holder_Obj;

    public GameObject tmpIAPButtonsObj; // Definitiely not the optimal way to do this... but use this to hold an IAP button for every single product so that the restore button can call the OnPurchaseComplete

    // TODO... MAKE SURE WE ADD THE PRODUCTS WE ALREADY OWN TO THE ACTIVE PRODUCTS WHEN WE INITIALIZE THIS FROM A LOADED GAME


    public bool initialized = false; // Will be set by Game_Manager upon initialization

    public static IAP_Manager instance;

    
    void Awake()
    {
        if (!instance){
            instance = this;
            activeProductsToShopPanel = new Dictionary<IAP_Product_Scriptable_Object, GameObject>();
            inactiveProductsToShopPanel = new Dictionary<IAP_Product_Scriptable_Object, GameObject>();
            ownedNonConsumableProductsIds = new List<string>();
            // Add owned by default products to this
            foreach(IAP_Product_Scriptable_Object product in activeProducts){
                Debug.Assert(Array.Exists(allProducts, p => p.Equals(product)));
                if(typeof(IAP_Product_Scriptable_Object_Nonconsumable).IsAssignableFrom(product.GetType()) && ((IAP_Product_Scriptable_Object_Nonconsumable)(System.Object)product).OwnedByDefault){
                    ownedNonConsumableProductsIds.Add(product.ProductId);
                }
            }

            TMP_Button_Panel_Holder_Obj = new GameObject("TMP_Button_Panel_Holder_Obj");
            TMP_Button_Panel_Holder_Obj.transform.SetParent(gameObject.transform);

            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }

    // NEED TO DO THIS TO ENSURE PANELS ARE ALWAYS LOADED WITH IAP BUTTONS IN CASE USER ON APPLE TRIES TO RESTORE PURHCASES
    // void OnLevelWasLoaded(){
    //     // tmpIAPButtonsObj = new GameObject("TMP_IAP_BUTTONS_OBJ");
    //     // tmpIAPButtonsObj.transform.SetParent(gameObject.transform);
    //     TMP_Button_Panel_Holder_Obj = new GameObject();
    //     TMP_Button_Panel_Holder_Obj.transform.SetParent(gameObject.transform);
    //     IEnumerator waitThenAddPanels(){
    //         yield return new WaitForSeconds(.1f);
    //         if(SceneManager.GetActiveScene().name=="Main_Area"){
    //             foreach(IAP_Product_Scriptable_Object product in allProducts){
    //                 UI_Controller.instance.addShopPanel(product, parent: TMP_Button_Panel_Holder_Obj);
    //             }
    //         }
    //     }
    //     StartCoroutine(waitThenAddPanels());
    // }

    private List<IAP_Product_Scriptable_Object> tmpinactiveProducts;
    void Update(){
        tmpinactiveProducts = new List<IAP_Product_Scriptable_Object>(allProducts.Where(product => !activeProductsToShopPanel.Keys.Contains(product)));
        foreach(IAP_Product_Scriptable_Object product in tmpinactiveProducts){
            if(!inactiveProductsToShopPanel.Keys.Contains(product)){
                inactiveProductsToShopPanel[product] = Instantiate(shopPanelPrefab, new Vector3(0, 0, 0), Quaternion.identity, parent:TMP_Button_Panel_Holder_Obj.transform);
                inactiveProductsToShopPanel[product].transform.localScale = new Vector3(0,0,0);
                GameObject buyButtonObj = Object_Finder.findChildObjectByName(inactiveProductsToShopPanel[product], "Shop_Buy_Button");
                initializeIAPButton(buyButtonObj, product);
            }
        }
        foreach(IAP_Product_Scriptable_Object product in activeProductsToShopPanel.Keys){
            if(inactiveProductsToShopPanel.Keys.Contains(product) && inactiveProductsToShopPanel[product] != null){
                Destroy(inactiveProductsToShopPanel[product].gameObject);
                inactiveProductsToShopPanel[product] = null;
            }
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
        
        // Remove all panels first in case we need to reorder
        foreach(IAP_Product_Scriptable_Object product in allProducts){
            if(activeProductsToShopPanel.Keys.Contains(product) && activeProductsToShopPanel[product] != null){
                activeProductsToShopPanel[product].transform.SetParent(null);
            }
        }

        // Do it this way to maintain ordering in the UI
        // Pull out the products which we OWN, or are available for purchase
        foreach(IAP_Product_Scriptable_Object product in allProducts.Where(product => (activeProducts.Contains(product) || ownedNonConsumableProductsIds.Contains(product.ProductId)) && IAP_Product_Scriptable_Object.SpecialRequirementsMet(product))){//.Where(product => IAP_Product_Scriptable_Object.SpecialRequirementsMet(product) || ownedNonConsumableProductsIds.Contains(product.ProductId))){
            if(!activeProductsToShopPanel.Keys.Contains(product) || activeProductsToShopPanel[product] == null){
                //Debug.Log("ADDING SHOP NEW PANEL: " + product.ProductTitle);
                GameObject panel = UI_Controller.instance.addShopPanel(product);
                activeProductsToShopPanel[product] = panel;
            }
            else{
                //Debug.Log("ADDING SHOP OLD PANEL: " + product.ProductTitle);
                GameObject panel = UI_Controller.instance.addShopPanel(activeProductsToShopPanel[product]);
                activeProductsToShopPanel[product] = panel;
            }
        }
    }


    public void initializeIAPButton(GameObject buttonObj, IAP_Product_Scriptable_Object product){
        initializeIAPButton(buttonObj.GetComponent<IAPButton>(), product);
    }


    public void initializeIAPButton(IAPButton button, IAP_Product_Scriptable_Object product){
        button.productId = product.ProductId;
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



    public void updateAllShopPanels(){
        try{
            foreach(GameObject panel in activeProductsToShopPanel.Values){
                UI_Controller.instance.updateShopPanel(panel);
            }
        }
        catch(Exception e){
            Debug.LogError("COULDN'T UPDATE SHOP PANELS");
        }
    }

    public bool checkNonConsumableProductForOwnership(IAP_Product_Scriptable_Object product){
        bool owned = false;
        if(typeof(IAP_Product_Scriptable_Object_Nonconsumable).IsAssignableFrom(product.GetType()) && ownedNonConsumableProductsIds.Contains(product.ProductId)){
            owned = true;
        }
        return owned;
    }


    // public void clearActiveProductsToShopPanel(){
    //     activeProductsToShopPanel = new Dictionary<IAP_Product_Scriptable_Object, GameObject>();
    // }

}

