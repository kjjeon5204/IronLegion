using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO;

public struct PlayerMasterStat
{
    public int level;
    public int curExp;
    public int expRequired;
    public int hp;
    public int energy;
    public int armor;
    public int damage;
    public float penetration;
    public int luck;
    public string[] equipment;
}





public class PlayerMasterData : MonoBehaviour {
    string heroMechID;
    public enum SceneType
    {
        HANGER,
        BATTLE,
        OVERWORLD,
        RMT
    }

    class PlayerRecord
    {
        int consecutiveWinData;

        public PlayerRecord()
        {
            string dataPath = Application.persistentDataPath + "/ASD123ID";
            if (!File.Exists(dataPath))
            {
                create_new_data();
            }

            using (StreamReader infile = File.OpenText(dataPath))
            {
                consecutiveWinData = System.Convert.ToInt32(infile.ReadLine());
            }
        }

        void create_new_data()
        {
            string dataPath = Application.persistentDataPath + "/ASD123ID";

            using (StreamWriter outfile = File.CreateText(dataPath))
            {
                outfile.WriteLine("0");
            }
        }

        void save_data()
        {
            string dataPath = Application.persistentDataPath + "/ASD123ID";

            using (StreamWriter outfile = File.CreateText(dataPath))
            {
                outfile.WriteLine(consecutiveWinData);
            }
        }

        public void reset_counter()
        {
            consecutiveWinData = 0;
            save_data();
        }

        public void increment_win()
        {
            consecutiveWinData++;
            save_data();
        }

        public int get_cur_win_count()
        {
            return consecutiveWinData;
        }
    }

    public SceneType curSceneType;

    //Hanger data types
    Inventory inventoryData;



    //Hero Data
    HeroStats heroItemStats;
    HeroData heroAbilityData;
    UpgradeData upgradeData;
    PlayerLevelBackEnd playerLevelData;

    //Map Data
    MapData mapData;
    PlayerRecord playerConsecutiveWin;

    //Event Progression
    PlayerDataReader eventRecord;

    /*******PUBLIC FUNCTIONS******************************/
    //Hanger data functions
    //Inventory
    public Inventory access_inventory_data() 
    {
        return inventoryData;
    }

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

    public void save_currency(int credit, int cogentum)
    {
        inventoryData.store_currency(credit, cogentum);
        store_inventory();
    }

    //HERO MECH INFO
    //Skill
    public HeroData access_hero_ability_data() 
    {
        return heroAbilityData;
    }

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
        return heroAbilityData.CheckAbilityList();
    }

    public bool set_ability(int index, int id)
    {
        return heroAbilityData.SetAbility(index, id);
    }

    public string return_ability_name(int index)
    {
        return heroAbilityData.ReturnAbilityName(index);
    }

    public int return_ability_ID(int index)
    {
        return heroAbilityData.ReturnAbilityID(index);
    }

    //level data
    public int get_player_level()
    {
        if (playerLevelData == null)
            Debug.Log("Null player!");
        return playerLevelData.get_player_level();
    }

    public int get_player_experience()
    {
        return playerLevelData.get_player_experience();
    }

    public void save_player_level(int level, int experience)
    {
        playerLevelData.save_file(level, experience, heroMechID);
    }

    //Upgrade Stats
    public UpgradeData access_upgrade_data()
    {
        return upgradeData;
    }

    public void save_upgrade_data()
    {
        upgradeData.save_upgrade_data();
    }

    //stat data
    public HeroStats access_equipment_data()
    {
        return heroItemStats;
    }

    public void save_hero_equip_data(Stats inputStat)
    {
        heroItemStats.save_data(inputStat, heroMechID);
    }

    public void save_hero_equip_data()
    {
        heroItemStats.save_data(heroMechID);
    }

    //Map Data
    public void set_map_chapter(int chapter, MapTileData[] mapTileData) 
    {
        mapData.initialize_map_progress(chapter, mapTileData);
    }

    public int clear_level(int chapter, int tileNum)
    {
        return mapData.clear_level(chapter, tileNum);
    }

    public IList<int> get_unlocked_levels(int chapter)
    {
        return mapData.return_unlocked_levels(chapter);
    }

    public IList<int> get_latest_levels(int chapter)
    {
        return mapData.return_latest_levels(chapter);
    }

    public PlayerDataReader access_player_event_record()
    {
        return eventRecord;
    }

    public PlayerMasterStat get_combined_stats()
    {
        PlayerMasterStat playerMasterStat = new PlayerMasterStat();
        if (playerLevelData != null) {
            playerMasterStat.level = playerLevelData.get_player_level();
            playerMasterStat.curExp = playerLevelData.get_player_experience();
        }

        if (heroItemStats != null)
        {
            playerMasterStat.hp = heroItemStats.get_current_stats().hp;
            playerMasterStat.energy = heroItemStats.get_current_stats().energy;
            playerMasterStat.damage = heroItemStats.get_current_stats().damage;
            playerMasterStat.armor = heroItemStats.get_current_stats().armor;
            playerMasterStat.luck = heroItemStats.get_current_stats().luck;
            playerMasterStat.penetration = heroItemStats.get_current_stats().penetration;
            playerMasterStat.equipment = heroItemStats.get_equipped_item();
        }

        if (upgradeData != null)
        {
            UpgradeStats myUpgradeStats = upgradeData.get_upgrade_data();
            playerMasterStat.hp += myUpgradeStats.HP.statValue;
            playerMasterStat.damage += myUpgradeStats.damage.statValue;
            playerMasterStat.energy += myUpgradeStats.energy.statValue;
        }
        return playerMasterStat;
    }

    public int get_player_consecutive_win()
    {
        return playerConsecutiveWin.get_cur_win_count();
    }

    public void player_win_increment()
    {
        playerConsecutiveWin.increment_win();
    }

    public void reset_win_ctr()
    {
        playerConsecutiveWin.reset_counter();
    }

    /*******Private internal functions********************/
    void load_player_master_data()
    {
        inventoryData.load_inventory();
        heroAbilityData.load_data(heroMechID);
        playerLevelData.load_file(heroMechID);
        heroItemStats.load_data(heroMechID);
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
        heroMechID = "heroMech"; //Initialize heromech ID later replaced with new data

        if (!Directory.Exists(Application.persistentDataPath + "/" + heroMechID)) 
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + heroMechID);
        }

        //load files
        playerConsecutiveWin = new PlayerRecord();
        mapData = new MapData();
        playerLevelData = new PlayerLevelBackEnd();
        heroAbilityData = new HeroData();
        inventoryData = new Inventory();
        heroItemStats = new HeroStats();
        upgradeData = new UpgradeData(heroMechID);
        eventRecord = new PlayerDataReader();
        load_player_master_data();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
