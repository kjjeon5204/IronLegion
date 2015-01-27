using UnityEngine;
using System.Collections;
using System.IO;

[System.Serializable] 
public struct TileTypeData
{
    public MapTileButton.TileType tileType;
    public Sprite lockedTileSprite;
    public Sprite unlockedTileSprite;
}

public class MapTileButton : BaseUIButton {
    public enum TileType
    {
        FACTION1_NORMAL,
        FACTION1_BOSS
    }
    public TileType myTileType;
    public TileTypeData[] tileTypeData;

    public bool tileCleared = false;
    SpriteRenderer mySprite;
    public int mapTileAccessNum;
    public int[] unlockedTiles;
    public string mapID;

    public UIDialogueData tileDialogueData;
    public UITextDialogueData tileTextDialogueData; //If both UIDialogueData and UITextDialogueData are assigned, UITextDialogueData overrides UIDialogueData

    OverworldSceneControls ovSceneControls;

    public override void button_released_action()
    {
        if (tileCleared == true)
        {
            UserData.nextTargetScene = mapID;
            ovSceneControls.set_ui_state(OverworldSceneControls.UIState.INITIALIZE_COMBAT);
        }
    }

	// Use this for initialization
	public void initialize_tile (bool unlocked, OverworldSceneControls mySceneControl) {
        tileCleared = unlocked;
        if (unlocked)
            mySprite.sprite = tileTypeData
                [System.Convert.ToInt32(myTileType)].unlockedTileSprite;
        else
            mySprite.sprite = tileTypeData
                [System.Convert.ToInt32(myTileType)].lockedTileSprite;
	}
}
