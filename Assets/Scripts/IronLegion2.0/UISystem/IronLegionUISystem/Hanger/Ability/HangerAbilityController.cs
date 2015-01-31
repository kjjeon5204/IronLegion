using UnityEngine;
using System.Collections;


/*Description: This function handles the overall ability script and its functions*/
public class HangerAbilityController : MonoBehaviour {
    public AbilityDictionary abilityDictionary;

    public GameObject hangerAbilityInfoPrefab;

    /*Populates skill table with player abilities input game object representing skill type and
     ability names representing list of abilities. This function must be called after ability info has been set*/
    public void populate_ability_table(GameObject abilityHolder, string[] abilityNames)
    {
        Vector3 abilityWindowPos = abilityHolder.transform.position;
        float yOffset = 1.5f;
        for (int ctr = 0; ctr < abilityNames.Length; ctr++)
        {
            //Get ability data of referenced ability.
            PlayerAbilityData tempAbilityData = abilityDictionary.find_ability_data(abilityNames[ctr]);
            bool isEquipped = false;
            if (abilityNames[ctr][0] == 'C') {
                if (find_equipped_ability(abilityNames[ctr], closeRangeAbilities) != -1)
                {
                    isEquipped = true;
                }
            }
            if (abilityNames[ctr][0] == 'F')
            {
                if (find_equipped_ability(abilityNames[ctr], farRangeAbilities) != -1)
                {
                    isEquipped = true;
                }
            }
            GameObject tempWindow = (GameObject)Instantiate(hangerAbilityInfoPrefab, abilityWindowPos, Quaternion.identity);
            abilityWindowPos.y += yOffset;
            HangerAbilityInfoWindow tempInfoWindowAcc = tempWindow.GetComponent<HangerAbilityInfoWindow>();


            //Initialize ability windows with ability
            tempInfoWindowAcc.ability_window_initialize(tempAbilityData, isEquipped);
        }
    }

    //This struct is used to represent the skills equipped in slot
    public struct AbilitySlot
    {
        public SpriteRenderer abilitySlot;
        public string abilityID;
    }



    //This struct is used to display the skill description slots.
    public struct AbilityInfo
    {
        public string abilityID;
        public HangerAbilityInfoWindow abilityInfoWindow;
        public bool unlocked;
        public bool equipped;
    }

    AbilityInfo[] closeRangeAbilityList; //List of all player's close range ability list
    AbilityInfo[] farRangeAbilityList; //List of all player's far range ability list

    public HangerAbilityInfoWindow abilityInfoWindowPrefab;

    public Sprite emptySlot;
    public AbilitySlot[] closeRangeAbilities; //4 slots
    public AbilitySlot[] farRangeAbilities; //4 slots

    /*Used to initialize skills into the skill slot. The size of two input arrays should be 4.*/
    public void initialize_ability_equipment(GameObject abilityHolder, string[] abilityEquipped, AbilitySlot[] abilitySlots)
    {
        for (int ctr = 0; ctr < 4; ctr++)
        {
            //Set ability into ability slot
            abilitySlots[ctr].abilityID = abilityEquipped[ctr];
            if (abilityEquipped[ctr] != "000000")
            {
                //Get ability data from dictionary
                PlayerAbilityData tempData = abilityDictionary.find_ability_data(abilityEquipped[ctr]);
                //Set ability into ability slot
                if (tempData != null)
                    abilitySlots[ctr].abilitySlot.sprite = tempData.skillSprite;
            }
            else
            {
                //set empty item sprite
                abilitySlots[ctr].abilitySlot.sprite = emptySlot;
            }
        }
    }

    public void ability_slot_switched()
    {
        curSelectedAbilitySlot = -1;
    }

    int curSelectedAbilitySlot;

    /*This function is called when player selects the */
    public void select_ability(int selectedAbility)
    {
        curSelectedAbilitySlot = selectedAbility;
    }


    string[] closeRangeAbilityRef;
    string[] farRangeAbilityRef;
    /*Equip Ability - equips ability into a slot*/
    public void equip_ability(string abilityToEquip, int slotAcc)
    {
        //Check if ability slot contains an equipped ability
        string abilityEquipped = "000000";
        abilityEquipped = UserData.userDataContainer.get_current_mech().itemsEquipped[slotAcc];
        //unequip ability if there is an equipped ability
        if (abilityEquipped[0] == 'C')
        {
            unequip_ability (abilityEquipped, closeRangeAbilities);

        }
        else
        {
            unequip_ability (abilityEquipped, farRangeAbilities);

        }
        
        //equip ability
        PlayerAbilityData tempAbility = abilityDictionary.find_ability_data(abilityToEquip);
        if (abilityEquipped[0] == 'C')
        {
            closeRangeAbilities[slotAcc].abilityID = abilityToEquip;
            closeRangeAbilities[slotAcc].abilitySlot.sprite = tempAbility.skillSprite;
            int abilityListAcc = find_ability(abilityToEquip, closeRangeAbilityList);
            closeRangeAbilityList[abilityListAcc].equipped = true;
            //closeRangeAbilityList[abilityListAcc].abilityInfoWindow.ability_equipped();
        }
        else
        {
            farRangeAbilities[slotAcc].abilityID = abilityToEquip;
            farRangeAbilities[slotAcc].abilitySlot.sprite = tempAbility.skillSprite;
            int abilityListAcc = find_ability(abilityToEquip, farRangeAbilityList);
            farRangeAbilityList[abilityListAcc].equipped = true;
            //farRangeAbilityList[abilityListAcc].abilityInfoWindow.ability_equipped();
        }

        UserData.userDataContainer.equip_ability(abilityToEquip, slotAcc);
    }


    /*Unequip abiltiy - removes ability from the slot. This function is for */
    public void unequip_ability (string abilityToUnequip, AbilitySlot[] unequipAbilityList)
    {
        //Get the slot ability is equipped in

        //find the equipped slot
        int equipSlot = find_equipped_ability(abilityToUnequip, unequipAbilityList);
        int abilityListAcc = 0;
        if (abilityToUnequip[0] == 'C')
        {
            abilityListAcc = find_ability(abilityToUnequip, closeRangeAbilityList);
            closeRangeAbilityList[abilityListAcc].equipped = false;
            //closeRangeAbilityList[abilityListAcc].abilityInfoWindow.unequip_ability();
        }
        else
        {
            abilityListAcc = find_ability(abilityToUnequip, farRangeAbilityList);
            farRangeAbilityList[abilityListAcc].equipped = false;
            //closeRangeAbilityList[abilityListAcc].abilityInfoWindow.unequip_ability();
        }

        //Unequip from both equip slot and ability list
        unequipAbilityList[equipSlot].abilityID = "000000";
        
    }

    /*Search whether or not ability is equipped. If ability is equipped, return the slot number
     that the ability is equipped in. If ability is not found, return -1*/
    public int find_equipped_ability(string abilityName, string[] abilityNameList)
    {
        int abilitySlotRef = -1;
        int searchCtr = 0;
        //Linear search cause linear search is awesome!
        while (abilitySlotRef == -1 && searchCtr < abilityNameList.Length) 
        {
            if (abilityNameList[searchCtr] == abilityName)
            {
                abilitySlotRef = searchCtr;
                return abilitySlotRef;
            }
            searchCtr++;
        }
        return abilitySlotRef;
    }

    /*Ability search list that contains ability slots */
    public int find_equipped_ability(string abilityName, AbilitySlot[] abilityNameList)
    {
        int abilitySlotRef = -1;
        int searchCtr = 0;
        //Linear search cause linear search is awesome!
        while (abilitySlotRef == -1 && searchCtr < abilityNameList.Length)
        {
            if (abilityNameList[searchCtr].abilityID == abilityName)
            {
                abilitySlotRef = searchCtr;
                return abilitySlotRef;
            }
            searchCtr++;
        }
        return abilitySlotRef;
    }


    /*Ability search list composed of list of all abilities that includes both equipped and non equipped
     skill.*/
    public int find_ability(string abilityName, AbilityInfo[] abilityNameList)
    {
        int abilitySlotRef = -1;
        int searchCtr = 0;
        //Linear search cause linear search is awesome!
        while (abilitySlotRef == -1 && searchCtr < abilityNameList.Length)
        {
            if (abilityNameList[searchCtr].abilityID == abilityName)
            {
                abilitySlotRef = searchCtr;
                return abilitySlotRef;
            }
            searchCtr++;
        }
        return abilitySlotRef;
    }

    public GameObject closeRangeListHolder;
    public GameObject farRangeListHolder;
    public GameObject closeRangeEquipHolder;
    public GameObject farRangeEquipHolder;

    public PlayerMechDictionary playerMechDictionary;

    /*This function runs at the start of the ability frame.
     Following process should occur in order
     1. Initialize equipped slot
     2. Initialize ability list*/
    void Start()
    {
        string[] closeRangeEquippedAbility = new string[4];
        string[] farRangeEquippedAbility = new string[4];
        //Get player mech
        PlayerMechData playerMech = UserData.userDataContainer.get_current_mech();
        //Copy currently equipped ability
        for (int ctr = 0; ctr < 4; ctr++)
        {
            //set close range ability
            closeRangeEquippedAbility[ctr] = playerMech.equippedSkill[ctr];
            //set far range ability
            farRangeEquippedAbility[ctr] = playerMech.equippedSkill[ctr + 4];
        }

        initialize_ability_equipment(closeRangeEquipHolder, closeRangeEquippedAbility, closeRangeAbilities);
        initialize_ability_equipment(farRangeEquipHolder, farRangeEquippedAbility, farRangeAbilities);
        //populate_ability_table(closeRangeListHolder, playerMechDictionary.find_player_mech_data(playerMech.mechID).closeRangeAbility);
        //populate_ability_table(farListHolder, playerMechDictionary.find_player_mech_data(playerMech.mechID).farRangeAbility);
    }
}
