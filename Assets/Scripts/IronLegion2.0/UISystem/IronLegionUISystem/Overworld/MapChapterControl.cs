using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapChapterControl : MonoBehaviour {
    public int chapterAccNum; //0 to n
    MapTileButton[] mapTiles;
    IList<MissionData> chapterTileClearCount;
    public GameObject mapTileHolder;
    public OverworldSceneControls ovSceneControls;
    public GameObject mLeftLimit;
    public GameObject mRightLimit;
    public GameObject mTopLimit;
    public GameObject mBottomLimit;



   


    public void initialize_cur_chapter(OverworldSceneControls inputControl)
    {
        ovSceneControls = inputControl;
        mapTiles = new MapTileButton[mapTileHolder.transform.childCount];
        for (int ctr = 0; ctr < mapTileHolder.transform.childCount; ctr++)
        {
            GameObject tempChild = mapTileHolder.transform.GetChild(ctr).gameObject;
            MapTileButton tempButton = tempChild.GetComponent<MapTileButton>();
            mapTiles[tempButton.mapTileAccessNum] = tempChild.GetComponent<MapTileButton>();
        }
        mapTiles[0].gameObject.SetActive(true);
        mapTiles[0].initialize_tile(true, this);
        //chapterTileClearCount = UserData.userDataContainer.currentlyClearedMaps[chapterAccNum].clearedMissionList;
        //unlock_tiles_traverse(0);
    }

    void unlock_tiles_traverse(int tileToCheck)
    {
        //If mission is cleared
        if (chapterTileClearCount[tileToCheck].clearCount > 0)
        {
            mapTiles[tileToCheck].gameObject.SetActive(true);
            mapTiles[tileToCheck].initialize_tile(true, this);
            for (int ctr = 0; ctr < mapTiles[tileToCheck].unlockedTiles.Length; ctr++)
            {
                unlock_tiles_traverse(mapTiles[tileToCheck].unlockedTiles[ctr]);
            }
        }
        //If mission is not cleared
        else
        {
            //Display up to next boss tile
            for (int ctr = 0; ctr < mapTiles[tileToCheck].unlockedTiles.Length; ctr++)
            {
                enable_display_tiles(mapTiles[tileToCheck].unlockedTiles[ctr]);
            }
            return;
        }
    }

    void enable_display_tiles(int tileToCheck)
    {
        if (mapTiles[tileToCheck].myTileType == MapTileButton.TileType.FACTION1_BOSS)
        {
            mapTiles[tileToCheck].initialize_tile(false, this);
            mapTiles[tileToCheck].gameObject.SetActive(true);
            for (int ctr = 0; ctr < mapTiles[tileToCheck].unlockedTiles.Length; ctr++)
            {
                enable_display_tiles(mapTiles[tileToCheck].unlockedTiles[ctr]);
            }
        }
    }

    

    public void receive_tile_pressed_action(CombatDataBlock combatDataBlock)
    {
        ovSceneControls.set_ui_to_initalize_combat(combatDataBlock);

        
    }
}


