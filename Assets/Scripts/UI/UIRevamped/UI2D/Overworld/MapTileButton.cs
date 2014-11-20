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
    public GameObject sceneTransitionLoading;
    SpriteRenderer mySprite;
    public int mapTileAccessNum;
    public int[] unlockedTiles;
    public string mapID;

    public override void button_released_action()
    {
        string mapIDPath = Application.persistentDataPath + "MapTransferData.txt";
        if (mapID.Length > 0)
        {
            using (StreamWriter outfile = File.CreateText(mapIDPath))
            {
                outfile.WriteLine(mapID);
            }
        }
        sceneTransitionLoading.SetActive(true);
    }

	// Use this for initialization
	public void initialize_tile (bool unlocked) {
        if (unlocked)
            mySprite.sprite = tileTypeData
                [System.Convert.ToInt32(myTileType)].unlockedTileSprite;
        else
            mySprite.sprite = tileTypeData
                [System.Convert.ToInt32(myTileType)].lockedTileSprite;
	}
}
