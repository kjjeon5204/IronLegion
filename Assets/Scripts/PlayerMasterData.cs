using UnityEngine;
using System.Collections;
using System.IO;

public class PlayerMasterData : MonoBehaviour {
    string heroMechID;

    public enum SceneType
    {
        HANGER,
        BATTLE,
        OVERWORLD,
        RMT
    }

    public SceneType curSceneType;

    //Hanger data types
    Inventory inventoryData;


    
    //Hero Data
    HeroStats heroItemStats;
    HeroData heroAbilityData;
    UpgradeData upgradeData;
    PlayerLevelBackEnd playerBackEnd;

    //Map Data
    MapData mapData;

    /*******PUBLIC FUNCTIONS******************************/
    //Hanger data functions
    //Inventory
    public void store_inventory(Item[] itemList)
    {
        inventoryData.store_inventory(itemList);
    }

    public void store_inventory()
    {
        inventoryData.store_inventory();
    }

    public void add_item(string itemID)
    {
        inventoryData.add_item(itemID);
    }

    public void swap_item(int index, string itemID)
    {
        inventoryData.swap_item(index, itemID);
    }

    public string[] get_inventory() 
    {
        return inventoryData.get_inventory();
    }

    public void set_inventory(string[] newInv)
    {
        inventoryData.set_inventory(newInv);
    }

    public int get_currency()
    {
        return inventoryData.get_currency();
    }

    public int get_paid_currency()
    {
        return inventoryData.get_paid_currency();
    }

    public void add_currency(int num)
    {
        inventoryData.change_currency(num);
    }

    public void add_paid_currency(int num)
    {
        inventoryData.change_paid_currency(num);
    }

    //HERO MECH INFO
    //Skill
    /*
    public void save_ability_data()
    {
        heroAbilityData.save_data(heroMechID);
    }

    public void create_ability_data()
    {
        heroAbilityData.create_data(heroMechID);
    }

    public string[] load_ability_data()
    {
        return heroAbilityData.load_data(heroMechID);
    }
    
    public bool check_ability_list()
    {

    }

    public bool set_ability(int index, int id)
    {

    }

    public string return_ability_name(int index)
    {

    }

    public int return_ability_name(int index)
    {

    }
    */


    /*******Private internal functions********************/
    void load_player_master_data()
    {

    }

    void initialize_hanger_data()
    {

    }

    void initialize_battle_data()
    {

    }

    void initialize_map_data()
    {

    }

    void initialize_ally_data()
    {

    }


	// Use this for initialization
	void Start () {
        if (curSceneType == SceneType.HANGER)
        {

        }
        else if (curSceneType == SceneType.BATTLE)
        {

        }
        else if (curSceneType == SceneType.OVERWORLD)
        {

        }
        else if (curSceneType == SceneType.RMT)
        {

        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
