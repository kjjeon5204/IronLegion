using UnityEngine;
using System.Collections;

public class HangerRandomItemBuy : BaseUIButton {
    public HangerRandomItemConfirm hangerArmoryConfirmWindow;
    public ItemDictionary itemDictionary;
    public float commonItemChance;
    public float uncommonItemChance;
    public float rareItemChance;
    public int minLevelPoll;
    public int maxLevelPoll;

    int maxLevel = 16; //When level cap is released, set this valueto the new level cap.

    public override void button_released_action()
    {
        //Get player's current level
        int playerLevel = UserData.userDataContainer.get_current_mech().level;
        //Calculate min and max item poll
        int lowerLimit = playerLevel - minLevelPoll;
        if (lowerLimit < 0) lowerLimit = 0;
        int upperLimit = playerLevel + maxLevelPoll;
        if (upperLimit > maxLevel) upperLimit = maxLevel;
        
        //Poll item generated
        Item itemPolled = itemDictionary.generate_random_item(lowerLimit, upperLimit).GetComponent<Item>();
        //Activate confirmation window
        hangerArmoryConfirmWindow.gameObject.SetActive(true);
        //Set armory confirm window with item generated.
        hangerArmoryConfirmWindow.set_random_item_confirm(itemPolled);
    }

	
}
