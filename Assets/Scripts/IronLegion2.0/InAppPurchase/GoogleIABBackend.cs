using UnityEngine;
using System.Collections;
using Prime31;
using System.Collections.Generic;

public class GoogleIABBackend : BaseUIButton {
    bool initializationComplete = false;
    public TextMesh textDisplay;

    public override void button_released_action()
    {
        initialize_IAB();
    }

    //This function is used to initialize in app purchases. This function returns true if successful 
    public void initialize_IAB()
    {
        textDisplay.text = "Start IAB initialization process!\n";
        GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
        GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
        //GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        //GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
        //GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
        //GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
        //GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
        //GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
        //GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;

        GoogleIAB.enableLogging(true);
        textDisplay.text += "Logging enabled\n";
        GoogleIAB.setAutoVerifySignatures(true);
        textDisplay.text += "Auto Verify Signature Enabled\n";

        textDisplay.text += "Starting IABInit\n";
        GoogleIAB.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAt7NKJ2icrvvuMIutLh0AFHe6D4Szcna1NMslMdZnYJSeuu6lq/W+6ACCZt85NdpxxoZHdzm0CTB8TcXJyUnwonIFywuq/kgVFRRlCnUSOxUTDUbAQbrKTPmZ3vIlQYwe7dbpTu8IBjDGy+PbpkAG7OnyKRNdNSp7bKI+1iIX0Z71fEIszQtmkfIExjw2QMm7ej4eUP62nq3N23JFe6NyYE8g26f3tteud/Rsxb5z2E7v8K0GPpWdyVen/saf8GiloPvoFax3RPQdros08Oz0XdLUL0uCxYiZWieIGt+0AR2RUhQk3mEszb49DAvr3NX8ZZSq1E6sw68mjDreg3qt4wIDAQAB");
        textDisplay.text += "IAB initialization\n";
        
        
    }



    public void disable_IAB()
    {
        GoogleIAB.unbindService();
    }


    string currentIABStatus;
    int errorCode = 0;
    string[] itemList = {"test_item_1",
                            "test_item2",
                            "test_item3"};
    void billingSupportedEvent()
    {
        textDisplay.text += "Initialization successful!\n";
        textDisplay.text += "Starting item loading process...\n";
        GoogleIAB.queryInventory(itemList);
    }

    void billingNotSupportedEvent(string errorStatus)
    {

        textDisplay.text = "Initialization failed \n";
        textDisplay.text = errorStatus + "\n";
    }

    public string get_current_IABState()
    {
        return currentIABStatus;
    }

    List<GoogleSkuInfo> skuData;
    void queryInventorySucceededEvent(List<GooglePurchase> googlePurchases, List<GoogleSkuInfo> skuInfos) 
    {
        skuData = skuInfos;
    }

    public List<GoogleSkuInfo> get_sku_data()
    {
        return skuData;
    }

    void queryInventoryFailedEvent(string errorStatus)
    {
        currentIABStatus = errorStatus;
    }

}
