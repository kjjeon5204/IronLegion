using UnityEngine;
using System.Collections;

public class MapEngageWindow : MonoBehaviour {
    public TextMesh[] messageDisplay;
    public SpriteRenderer mySpriteRederer;
    public SpriteRenderer[] specialItemDropDisplay;

    public void reset_map_confirm_window()
    {
        for (int ctr = 0; ctr < specialItemDropDisplay.Length; ctr++)
        {
            specialItemDropDisplay[ctr].gameObject.SetActive(false);
        }
    }

    /*First slot of messageList array refers to */
    public void set_engage_window(CombatDataBlock combatDataBlock)
    {
        messageDisplay[0].text = combatDataBlock.mapName;
        messageDisplay[1].text = "Credit: " + combatDataBlock.creditDrop.ToString();
        messageDisplay[2].text = "Exp: " + combatDataBlock.experienceDrop.ToString();
        //Set combat map picture
        mySpriteRederer.sprite = combatDataBlock.mapPicRef;
        BaseDropItem[] itemToDisplay = combatDataBlock.mapDropInfo.get_base_drop_item();
        for (int ctr = 0; ctr < itemToDisplay.Length; ctr++)
        {
            specialItemDropDisplay[ctr].sprite = itemToDisplay[ctr].spriteToDisplay;
            if (ctr >= specialItemDropDisplay.Length) break;
        }
    }

    
}
