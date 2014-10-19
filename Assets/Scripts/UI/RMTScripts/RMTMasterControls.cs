using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;


public struct EXPBooster{
    float expBoost;
    float expBoostCount;
}

public struct RMTItemData {
    public string itemID;
    public int cogentum;
    public EXPBooster expBoost;
}

public class RMTMasterControls : MonoBehaviour {
    public PlayerMasterData masterData;
    public GameObject purchaseFailWindow;
    public GameObject purchaseSuccessWindow;
    public GameObject storeInitFailedWindow;
    bool storeSuccesfullyInitialized = false;
    string[] skusData;
    GooglePurchase[] purchaseHistory;
    GoogleSkuInfo[] inventoryList;
    IDictionary<string, GoogleSkuInfo> inventoryDictionary = new Dictionary<string, GoogleSkuInfo>();
    IDictionary<string, RMTItemData> itemDictionary = new Dictionary<string, RMTItemData>();

    //Google In-App billing functions
    public void item_bought_successfully(GooglePurchase myPurchase)
    {
        RMTItemData curItem = itemDictionary[myPurchase.productId];
        if (myPurchase.purchaseState == GooglePurchase.GooglePurchaseState.Purchased) {
            //action for successful purchase
            masterData.add_currency(curItem.cogentum);
        }
        if (myPurchase.purchaseState == GooglePurchase.GooglePurchaseState.Purchased)
        {
            //Do Nothing
        }
    }

    public void item_bought_failed(string failedMessage)
    {

    }

    public void billing_supported()
    {
        //Runs continues with store
        storeSuccesfullyInitialized = true;
    }

    public void billing_not_supported(string failedOption)
    {
        //Prints error window and exits scene when user presses the button on window
    }

    public void query_inventory_succeeded(List<GooglePurchase> purchaseList, List<GoogleSkuInfo> googleSkuList) 
    {
        //Initialize purchase history and inventory list
        purchaseHistory = purchaseList.ToArray();
        inventoryList = googleSkuList.ToArray();
        for (int ctr = 0; ctr < inventoryList.Length; ctr++)
        {
            inventoryDictionary[inventoryList[ctr].productId] = inventoryList[ctr];
        }
    }


    public void query_inventory_failed(string failedOption)
    {

    }

    // Use this for initialization
    void Start()
    {
        //Initialize event that billing is supported or not
        GoogleIABManager.billingSupportedEvent += billing_supported;
        GoogleIABManager.billingNotSupportedEvent += billing_not_supported;

        //initialize store
        GoogleIAB.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAt7NKJ2icrvvuMIutLh0AFHe6D4Szcna1NMslMdZnYJSeuu6lq/W+6ACCZt85NdpxxoZHdzm0CTB8TcXJyUnwonIFywuq/kgVFRRlCnUSOxUTDUbAQbrKTPmZ3vIlQYwe7dbpTu8IBjDGy+PbpkAG7OnyKRNdNSp7bKI+1iIX0Z71fEIszQtmkfIExjw2QMm7ej4eUP62nq3N23JFe6NyYE8g26f3tteud/Rsxb5z2E7v8K0GPpWdyVen/saf8GiloPvoFax3RPQdros08Oz0XdLUL0uCxYiZWieIGt+0AR2RUhQk3mEszb49DAvr3NX8ZZSq1E6sw68mjDreg3qt4wIDAQAB");
        GoogleIAB.enableLogging(true);

        if (storeSuccesfullyInitialized)
        {
            GoogleIABManager.queryInventorySucceededEvent += query_inventory_succeeded;
            GoogleIABManager.queryInventoryFailedEvent += query_inventory_failed;
            GoogleIABManager.purchaseSucceededEvent += item_bought_successfully;
            GoogleIABManager.purchaseFailedEvent += item_bought_failed;
            GoogleIAB.queryInventory(skusData);
        }
    }

    public bool check_item_exists (string productID)
    {
        return inventoryDictionary.ContainsKey(productID);
    }



    public void purchase_item(string productID)
    {
        GoogleIAB.purchaseProduct(inventoryDictionary[productID].ToString());
    }
}
