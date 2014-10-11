using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EquipLogic : MonoBehaviour {	
    //Test Variables
    public GameObject[] testItems;

    //Required Variables
	Inventory inventory;
	HeroStats heroStats;
    public Camera GUICam;
    public GameObject heroMech;
    public GameObject scrollCam;
   

    public GameObject inventoryTexture;
    SpriteRenderer inventorySprite;
    public GameObject equipTexture;
    SpriteRenderer equipSprite;
    public GameObject characterTexture;
    SpriteRenderer characterSprite;
    public GameObject recyclerTexture;
    SpriteRenderer recyclerRenderer;
    public GameObject recycleBarTexture;
    SpriteRenderer recycleBarRenderer;
    public GameObject recycleButtonTexture;
    SpriteRenderer recycleButtonRenderer;
    public GameObject exitButtonTexture;
    SpriteRenderer exitButtonRenderer;
    public GameObject statsBoxTexture;
    SpriteRenderer statsBoxRenderer;
    public GameObject equipButtonTexture;
    SpriteRenderer equipButtonRenderer;
    public GameObject itemSlotTexture;
    SpriteRenderer itemSlotRenderer;
    public GameObject mapOverlay;
    GameObject recycleBar;

    GameObject[] statsBox;

    GameObject[] itemSlot;

    GameObject[] equipSlots;
    /*
     [0] = Weapon
     [1] = Body
     [2] = Helm*/

    //Tracker variables
    GameObject selectedSlot;
    GameObject selectedItem;
    Item selectedItemScript;

    //Item Dictionary
    IDictionary<string, GameObject> itemLookUp;

    //Item generator
    ItemGeneration itemGen;

    //Recycling variables
    float curProgress;

    //Scroll
    float scrollTrackerMin;
    float scrollTrackerMax;

    //Button Names
    string exitButtonName;
    string recycleButtonName;
    string inventoryName;

    PlayerMasterData playerMasterData;

    //Target size is relative to viewport determined by upperleft corner being (0, 0)
    void texture_resize(SpriteRenderer target, Rect targetSize)
    {
        Vector3 targetPos = new Vector3(targetSize.center.x, 1.0f - targetSize.center.y, 10.0f);
        target.transform.position = GUICam.ViewportToWorldPoint(targetPos);
        Vector3 xMin = GUICam.WorldToViewportPoint(target.bounds.min);
        Vector3 xMax = GUICam.WorldToViewportPoint(target.bounds.max);
        Vector3 curSize = xMax - xMin;
        
        float xScale = targetSize.width / curSize.x;
        float yScale = targetSize.height / curSize.y;
        Vector3 scaleFactor = new Vector3(xScale, yScale, 1.0f);
        target.transform.localScale = Vector3.Scale(target.transform.localScale,  scaleFactor);
    }


    Rect view_to_screen(Rect input)
    {
        Rect ret;
        Vector3 rightCorner = Camera.main.ViewportToScreenPoint(new Vector3 (input.x, input.y, 0.0f));
        Vector3 size = Camera.main.ViewportToScreenPoint(new Vector3(input.width, input.height, 0.0f));
        ret = new Rect(rightCorner.x, rightCorner.y, size.x, size.y);
        return ret;
    }


    void draw_equip_UI () {
        Rect ret = new Rect(0.15f, 0.0f, 0.25f, 0.75f);
        inventoryTexture = (GameObject)Instantiate(inventoryTexture, Vector3.zero, Quaternion.identity); 
        inventorySprite = inventoryTexture.GetComponent<SpriteRenderer>();
        texture_resize(inventorySprite, ret);
        scrollCam = inventoryTexture.transform.GetChild(0).gameObject;
        inventoryName = inventoryTexture.name;


        ret = new Rect(0.0f, 0.0f, 0.1f, 0.85f);
        recyclerTexture = (GameObject)Instantiate(recyclerTexture, Vector3.zero, Quaternion.identity);
        recyclerRenderer = recyclerTexture.GetComponent<SpriteRenderer>();
        texture_resize(recyclerRenderer, ret);

        ret = new Rect(0.008f, 0.1f, 0.06f, 0.7f);
        recycleBarTexture = (GameObject)Instantiate(recycleBarTexture, Vector3.zero, Quaternion.identity);
        recycleBarRenderer = recycleBarTexture.GetComponent<SpriteRenderer>();
        texture_resize(recycleBarRenderer, ret);

        recycleBar = recycleBarTexture.transform.GetChild(0).gameObject;

        ret = new Rect(0.5f, 0.0f, 0.25f, 0.75f);
        equipTexture = (GameObject)Instantiate(equipTexture, Vector3.zero, Quaternion.identity);
        equipSprite = equipTexture.GetComponent<SpriteRenderer>();
        texture_resize(equipSprite, ret);

        ret = new Rect(0.8f, 0.0f, 0.2f, 0.85f);
        characterTexture = (GameObject)Instantiate(characterTexture, Vector3.zero, Quaternion.identity);
        characterSprite = characterTexture.GetComponent<SpriteRenderer>();
        texture_resize(characterSprite, ret);

        ret = new Rect(0.0f, 0.9f, 0.1f, 0.1f);
        recycleButtonTexture = (GameObject)Instantiate(recycleButtonTexture, Vector3.zero, Quaternion.identity);
        recycleButtonRenderer = recycleButtonTexture.GetComponent<SpriteRenderer>();
        texture_resize(recycleButtonRenderer, ret);
        recycleButtonName = recycleButtonRenderer.name;

        ret = new Rect(0.8f, 0.9f, 0.2f, 0.1f);
        exitButtonTexture = (GameObject)Instantiate(exitButtonTexture, Vector3.zero, Quaternion.identity);
        exitButtonRenderer = exitButtonTexture.GetComponent<SpriteRenderer>();
        texture_resize(exitButtonRenderer, ret);
        exitButtonName = exitButtonTexture.name;

        statsBox = new GameObject[2];
        ret = new Rect(0.15f, 0.8f, 0.25f, 0.2f);
        statsBox[0] = (GameObject)Instantiate(statsBoxTexture, Vector3.zero, Quaternion.identity);
        statsBoxRenderer = statsBox[0].GetComponent<SpriteRenderer>();
        texture_resize(statsBoxRenderer, ret);

        ret = new Rect(0.5f, 0.8f, 0.25f, 0.2f);
        statsBox[1] = (GameObject)Instantiate(statsBoxTexture, Vector3.zero, Quaternion.identity);
        statsBoxRenderer = statsBox[1].GetComponent<SpriteRenderer>();
        texture_resize(statsBoxRenderer, ret);

        ret = new Rect(0.425f, 0.3f, 0.05f, 0.15f);
        equipButtonTexture = (GameObject)Instantiate(equipButtonTexture, Vector3.zero, Quaternion.identity);
        equipButtonRenderer = equipButtonTexture.GetComponent<SpriteRenderer>();
        texture_resize(equipButtonRenderer, ret);

        ret = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        mapOverlay = (GameObject)Instantiate(mapOverlay, Vector3.zero, Quaternion.identity);
        SpriteRenderer mapSprite = mapOverlay.GetComponent<SpriteRenderer>();
        texture_resize(mapSprite, ret);

        //scrollCam.transform.position = new Vector3(inventoryTexture.transform.position.x, inventoryTexture.transform.position.y, scrollCam.transform.position.z);
        

        float vOffset = 0.12f;
        float hOffset = 0.065f;
        int row = 10;
        int col = 3;
        itemSlot = new GameObject[row * col];
        for (int ctr1 = 0; ctr1 < row; ctr1++)
        {
            for (int ctr2 = 0; ctr2 < col; ctr2++)
            {
                ret = new Rect(0.18f + hOffset * (float)ctr2, 0.125f + vOffset * (float)ctr1, 0.053f, 0.1f);
                int acc = ctr1 * col + ctr2;
                itemSlot[acc] = (GameObject)Instantiate(itemSlotTexture, Vector3.zero, Quaternion.identity);
                itemSlotRenderer = itemSlot[acc].GetComponent<SpriteRenderer>();
                texture_resize(itemSlotRenderer, ret);
            }
        }
    }

    /*Adds item to the inventory*/
    void add_item(GameObject itemToAdd)
    {
        int inventoryAcc = 0;
        while (itemSlot[inventoryAcc].transform.childCount != 0)
        {
            inventoryAcc++;
        }
        itemToAdd = (GameObject)Instantiate(itemToAdd, Vector3.zero, Quaternion.identity);
        itemToAdd.layer = 10;
        itemToAdd.transform.position = itemSlot[inventoryAcc].transform.position;
        itemToAdd.transform.parent = itemSlot[inventoryAcc].transform;
    }

    void add_item_string(string itemToAdd)
    {
        if (itemLookUp.ContainsKey(itemToAdd))
        {
            add_item(itemLookUp[itemToAdd]);
        }
        else
        {
            GameObject getItem = (GameObject)Resources.Load(itemToAdd);
            SpriteRenderer spriteAccess = getItem.GetComponent<SpriteRenderer>();
            texture_resize(spriteAccess, new Rect(0.0f, 0.0f, 0.053f, 0.1f));
            itemLookUp[itemToAdd] = getItem;
            Debug.Log("Item Added");
            add_item(itemLookUp[itemToAdd]);
        }
    }


    void setup_inventory(string[] inventoryList)
    {
        for (int ctr = 0; ctr < inventoryList.Length; ctr++)
        {
            GameObject getItem = (GameObject)Resources.Load(inventoryList[ctr]);
            SpriteRenderer spriteAccess = getItem.GetComponent<SpriteRenderer>();
            texture_resize(spriteAccess, new Rect(0.0f, 0.0f, 0.053f, 0.1f));
            itemLookUp[inventoryList[ctr]] = getItem;
            add_item(itemLookUp[inventoryList[ctr]]);
        }
    }

    /*Add equip. If equip slot is empty equip and return true. If not empty, don't equip
     and return false*/
    bool add_equip(GameObject itemToEquip)
    {
        itemToEquip = (GameObject)Instantiate(itemToEquip, Vector3.zero, Quaternion.identity);
        itemToEquip.layer = 3;
        Item itemScript = itemToEquip.GetComponent<Item>();
        if (itemScript.itemID[2] == 'W')
        {
            if (equipSlots[0].transform.childCount == 0)
            {
                itemToEquip.transform.position = equipSlots[0].transform.position;
                itemToEquip.transform.parent = equipSlots[0].transform;
                itemToEquip.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
        else if (itemScript.itemID[2] == 'B')
        {
            if (equipSlots[1].transform.childCount == 0)
            {

                itemToEquip.transform.position = equipSlots[1].transform.position;
                itemToEquip.transform.parent = equipSlots[1].transform;
                itemToEquip.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
        else if (itemScript.itemID[2] == 'H')
        {
            if (equipSlots[2].transform.childCount == 0)
            {

                itemToEquip.transform.position = equipSlots[2].transform.position;
                itemToEquip.transform.parent = equipSlots[2].transform;
                itemToEquip.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
        return false;
    }


    void setup_equipment(string[] equipmentList)
    {
        for (int ctr = 0; ctr < equipmentList.Length; ctr++)
        {
            if (equipmentList[ctr] != "000000")
            {
                if (!itemLookUp.ContainsKey(equipmentList[ctr]))
                {
                    GameObject getItem = (GameObject)Resources.Load(equipmentList[ctr]);
                    SpriteRenderer spriteAccess = getItem.GetComponent<SpriteRenderer>();
                    texture_resize(spriteAccess, new Rect(0.0f, 0.0f, 0.053f, 0.1f));
                    itemLookUp[equipmentList[ctr]] = getItem;
                    add_equip(itemLookUp[equipmentList[ctr]]);
                }
                else
                {
                    add_equip(itemLookUp[equipmentList[ctr]]);
                }
            }
        }
    }


    void modify_text()
    {
        if (selectedItem != null)
        {
            statsBox[0].GetComponent<StatDisplay>().setText(selectedItem.GetComponent<Item>());
            if (selectedItem.GetComponent<Item>().itemID[2] == 'W')
            {
                if (equipSlots[0].transform.childCount == 1)
                {
                    Item curEquipped = equipSlots[0].transform.GetChild(0).gameObject.GetComponent<Item>();
                    statsBox[1].GetComponent<StatDisplay>().setText(curEquipped);
                }
                else
                {
                    statsBox[1].GetComponent<StatDisplay>().setEmpty();
                }
            }
            else if (selectedItem.GetComponent<Item>().itemID[2] == 'B')
            {
                if (equipSlots[1].transform.childCount == 1)
                {
                    Item curEquipped = equipSlots[1].transform.GetChild(0).gameObject.GetComponent<Item>();
                    statsBox[1].GetComponent<StatDisplay>().setText(curEquipped);
                }
                else
                {
                    statsBox[1].GetComponent<StatDisplay>().setEmpty();
                }
            }
            else if (selectedItem.GetComponent<Item>().itemID[2] == 'H')
            {
                if (equipSlots[2].transform.childCount == 1)
                {
                    Item curEquipped = equipSlots[2].transform.GetChild(0).gameObject.GetComponent<Item>();
                    statsBox[1].GetComponent<StatDisplay>().setText(curEquipped);
                }
                else
                {
                    statsBox[1].GetComponent<StatDisplay>().setEmpty();
                }
            }
        }
        else
        {
            statsBox[0].GetComponent<StatDisplay>().setEmpty();
            statsBox[1].GetComponent<StatDisplay>().setEmpty();
        }
        characterTexture.GetComponent<StatDisplay>().setText(heroStats.get_current_stats());
    }


    // Use this for initialization
    void Start()
    {
        draw_equip_UI ();
        
        Vector3 heroMechPos = Camera.main.ViewportToWorldPoint(new Vector3(0.9f, 0.5f, 9.0f));
        heroMech = (GameObject)Instantiate(heroMech, heroMechPos, Quaternion.identity);
        heroMech.transform.Rotate(0.0f, 180.0f, 0.0f);
        
        
        equipSlots = equipTexture.GetComponent<EquipTextureScript>().equipSlots;
        /*
        for (int ctr = 0; ctr < testItems.Length; ctr ++)
            add_item(testItems[ctr]);
        */
        

        itemLookUp = new Dictionary<string, GameObject>();
        inventory = new Inventory();
        string[] inventoryList = inventory.get_inventory();
        setup_inventory(inventoryList);

        //heroStats = new HeroStats();
        //heroStats.load_data();
        string[] hero_items = heroStats.get_equipped_item();
        setup_equipment(hero_items);

        itemGen = this.GetComponent<ItemGeneration>();
        curProgress = inventory.get_recycle_progress();
        scrollTrackerMax = scrollCam.transform.position.y;
        scrollTrackerMin = scrollTrackerMin + (itemSlot[3].transform.position.y - itemSlot[0].transform.position.y) * (itemSlot.Length / 3.0f - 5);
        Debug.Log("ScrollTrackerMin: " + scrollTrackerMin);
        Debug.Log("ScrollTrackerMax: " + scrollTrackerMax);
    }


    void storeInventory()
    {
        ArrayList remainingItem = new ArrayList();
        Item[] itemInInventory;
        for (int ctr = 0; ctr < itemSlot.Length; ctr++)
        {
            if (itemSlot[ctr].transform.childCount == 1)
            {
                remainingItem.Add(itemSlot[ctr].transform.GetChild(0).GetComponent<Item>());
            }
        }
        itemInInventory = new Item[remainingItem.Count];
        remainingItem.CopyTo(itemInInventory);
        inventory.store_inventory(itemInInventory);
        Debug.Log(itemInInventory.Length);
    }

    

	void equip_item() {
	    
	}


    void inventory_slot_touch()
    {
        int layerMask = 1 << 0;
        layerMask = ~layerMask;
        Camera scrollCamAcc = scrollCam.GetComponent<Camera>();
        RaycastHit2D touchGUIInteraction;
        int i = 0;
        while (i < Input.touchCount)
        {
            Vector3 touchPos = scrollCamAcc.ScreenToViewportPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0.0f));
            if (touchPos.x >= 0.0f && touchPos.x <= 1.0f && touchPos.y >= 0.0f && touchPos.y <= 1.0f)
            {
                
                touchPos = Camera.main.ScreenPointToRay(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0.0f)).origin;
                Vector2 conv2D = new Vector2(touchPos.x, touchPos.y);
                touchGUIInteraction = Physics2D.Raycast(conv2D, Vector2.zero, 100.0f, layerMask);
                if (touchGUIInteraction.collider == null)
                {
                    Debug.Log("No InventorySlot selected");
                    //does nothing
                }
                else if (touchGUIInteraction.collider.tag == "InventorySlot" && Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    Debug.Log("InventorySlot selected");

                    selectedSlot = touchGUIInteraction.collider.gameObject;
                    if (selectedSlot.transform.childCount == 1)
                    {
                        selectedItem = selectedSlot.transform.GetChild(0).gameObject;
                        selectedItemScript = selectedItem.GetComponent<Item>();
                        Debug.Log("Item " + selectedItem.name + " selected.");
                    }
                    else
                    {
                        selectedItem = null;
                        Debug.Log("No Item selected");
                    }
                }
                else
                {
                    Debug.Log(touchGUIInteraction.collider.gameObject.name);
                }
            }
            i++;
        }
    }

	// Update is called once per frame
	void Update () {
        
        //Mobile Input Controller
        int i = 0;
        RaycastHit2D touchGUIInteraction;
        heroMech.transform.Rotate(10.0f * Vector3.up * Time.deltaTime);
        heroMech.animation.Play("groundstart");
        
        while (i < Input.touchCount) {
            Vector3 touchPos = Camera.main.ScreenPointToRay(new Vector3 (Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0.0f)).origin;
            Vector2 conv2D = new Vector2(touchPos.x, touchPos.y);
            touchGUIInteraction = Physics2D.Raycast(conv2D, Vector2.zero);
            if (touchGUIInteraction.collider == null)
            {
                //does nothing
            }
                /*
            else if (touchGUIInteraction.collider.tag == "InventorySlot" && Input.GetTouch(i).phase == TouchPhase.Began)
            {
                Debug.Log("InventorySlot selected");
                selectedSlot = touchGUIInteraction.collider.gameObject;
                if (selectedSlot.transform.childCount == 1)
                {
                    selectedItem = selectedSlot.transform.GetChild(0).gameObject;
                    selectedItemScript = selectedItem.GetComponent<Item>();
                    Debug.Log("Item " + selectedItem.name + " selected.");
                }
                else
                {
                    selectedItem = null;
                    Debug.Log("No Item selected");
                }
            }
                 * */


            else if (touchGUIInteraction.collider.tag == "Swap" && Input.GetTouch(i).phase == TouchPhase.Began)
            {
                Debug.Log("Swap");
                Item previouslyEquipped;
                if (selectedItem != null)
                {
                    GameObject curItemSlot = selectedItem.transform.parent.gameObject;
                    //1
                    if (selectedItemScript.itemID[2] == 'B')
                    {
                        if (equipSlots[1].transform.childCount == 1)
                        {
                            previouslyEquipped = equipSlots[1].transform.GetChild(0).gameObject.GetComponent<Item>();
                            previouslyEquipped.transform.position = curItemSlot.transform.position;
                            previouslyEquipped.transform.parent = curItemSlot.transform;
                            previouslyEquipped.gameObject.layer = 10;
                            heroStats.remove_item(previouslyEquipped);
                        }
                        selectedItem.transform.parent = equipSlots[1].transform;
                        selectedItem.gameObject.layer = 0;
                        selectedItem.transform.position = equipSlots[1].transform.position;
                        heroStats.equip_item(selectedItem.GetComponent<Item>());
                        Debug.Log("Item Swapped!");
                    }
                    //0
                    if (selectedItemScript.itemID[2] == 'W')
                    {
                        if (equipSlots[0].transform.childCount == 1)
                        {
                            previouslyEquipped = equipSlots[0].transform.GetChild(0).gameObject.GetComponent<Item>();
                            previouslyEquipped.transform.position = curItemSlot.transform.position;
                            previouslyEquipped.transform.parent = curItemSlot.transform;
                            previouslyEquipped.gameObject.layer = 10;
                            heroStats.remove_item(previouslyEquipped);
                        }
                        selectedItem.transform.parent = equipSlots[0].transform;
                        selectedItem.transform.position = equipSlots[0].transform.position;
                        selectedItem.gameObject.layer = 0;
                        heroStats.equip_item(selectedItem.GetComponent<Item>());
                        Debug.Log("Item Swapped!");
                    }
                    //2
                    if (selectedItemScript.itemID[2] == 'H')
                    {
                        if (equipSlots[2].transform.childCount == 1)
                        {
                            previouslyEquipped = equipSlots[2].transform.GetChild(0).gameObject.GetComponent<Item>();
                            previouslyEquipped.transform.position = curItemSlot.transform.position;
                            previouslyEquipped.transform.parent = curItemSlot.transform;
                            previouslyEquipped.gameObject.layer = 10;
                            heroStats.remove_item(previouslyEquipped);
                        }
                        selectedItem.transform.parent = equipSlots[2].transform;
                        selectedItem.transform.position = equipSlots[2].transform.position;
                        selectedItem.gameObject.layer = 0;
                        heroStats.equip_item(selectedItem.GetComponent<Item>());
                        Debug.Log("Item Swapped!");
                    }
                }
                else
                {
                    Debug.Log("No selected item");
                }
                selectedItem = null;
                
            }
            else if (touchGUIInteraction.collider.gameObject.name == exitButtonName &&
                Input.GetTouch(i).phase == TouchPhase.Began)
            {
                storeInventory();
                playerMasterData.save_hero_equip_data();
                //heroStats.save_data();
                
                Debug.Log("Exit");
                Application.LoadLevel(0);
            }
            else if (touchGUIInteraction.collider.gameObject.name == recycleButtonName &&
                Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (selectedItem != null)
                {
                    curProgress += Convert.ToInt32(selectedItemScript.itemID.Remove(2)) * 15.0f;
                    Destroy(selectedItem);
                    if (curProgress >= 100.0f)
                    {
                        add_item_string(itemGen.gen_item(playerMasterData.get_combined_stats().level));
                        if (curProgress > 100.0f)
                        {
                            curProgress = curProgress - 100.0f;
                        }
                        else
                        {
                            curProgress = 0.0f;
                        }
                    }
                }
            }
            else if (touchGUIInteraction.collider.gameObject.name == "Inventory_body(Clone)")
            {
                if (Input.GetTouch(i).deltaPosition.y > 0.0f && 
                    scrollCam.transform.position.y <= scrollTrackerMax)
                {
                    scrollCam.transform.Translate(Vector3.up * Time.deltaTime * 15.0f);
                }
                if (Input.GetTouch(i).deltaPosition.y < 0.0f &&
                    scrollCam.transform.position.y >= scrollTrackerMin)
                {
                    scrollCam.transform.Translate(Vector3.up * -15.0f * Time.deltaTime); 
                }
            }
            i++;
        }
        inventory_slot_touch();
        Debug.Log("Number of touches this frame: " + Input.touchCount);
        modify_text();
        storeInventory();
        //heroStats.save_data();
        playerMasterData.save_hero_equip_data();
        inventory.save_recycle_progress(curProgress);
        recycleBar.transform.localScale = new Vector3(1.0f, curProgress / 100.0f, 1.0f);
	}

}
