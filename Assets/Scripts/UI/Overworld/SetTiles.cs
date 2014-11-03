using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetTiles : MonoBehaviour {
	public SetOverworldUI ui;
	public TutorialControls tutorial;
	
	private MapData map;
	public int levelsCompleted;
	//private GameObject Tiles;
	private GameObject Hero;


    //new MapData implementation variables
    public GameObject[] tileList;
    TileActive[] tileDataAccess;
    public int chapter;

    //map progression add on
    public int keyTile;
    public SetTiles nextMap;

    public PlayerMasterData playerMasterData;

    void activate_remaining_tiles(int curAcc)
    {
        int ctr = curAcc;
        while (tileDataAccess[ctr].unlockedLevels.Length != 0 &&
            tileDataAccess[ctr].isBoss == false)
        {
            tileDataAccess[ctr].gameObject.SetActive(true);
            ctr++;
        }
        if (ctr < tileDataAccess.Length && tileDataAccess[ctr].isBoss == true)
        {
            tileDataAccess[ctr].gameObject.SetActive(true);
        }
    }

	// Use this for initialization
	void Start () {
		Hero = GameObject.Find("Hero Location");
        /*
		map = new MapData();
		map.load_dictionary();
		levelsCompleted = map.LevelsCompleted();
         */ 
        //Initialize automated tile reading
        MapTileData[] mapTileHolder = new MapTileData[tileList.Length];
        tileDataAccess = new TileActive[tileList.Length];
        for (int ctr = 0; ctr < tileList.Length; ctr++)
        {
            tileDataAccess[ctr] = tileList[ctr].GetComponent<TileActive>();
            mapTileHolder[ctr].level = tileDataAccess[ctr].level;
            mapTileHolder[ctr].mapID = tileDataAccess[ctr].mapID;
            mapTileHolder[ctr].clearCount = 0;
            mapTileHolder[ctr].adjLevelList = tileDataAccess[ctr].unlockedLevels;
        }
        playerMasterData.set_map_chapter(chapter, mapTileHolder);
        IList<int> unlockedLevels = playerMasterData.get_unlocked_levels(chapter);
        IList<int> latestUnlocked = playerMasterData.get_latest_levels(chapter);

        
        for (int ctr = 0; ctr < latestUnlocked.Count; ctr++)
        {

        }
		
		bool done_once = false; //So that you only ActivateArrows once
        for (int ctr = 0; ctr < unlockedLevels.Count; ctr++)
        {
            tileDataAccess[unlockedLevels[ctr]].gameObject.SetActive(true);
            tileDataAccess[unlockedLevels[ctr]].TileOn();
            /*add on*/
            if (unlockedLevels[ctr] == keyTile && !latestUnlocked.Contains(unlockedLevels[ctr]))
            {
                if (nextMap != null)
                    nextMap.enabled = true;
                Debug.Log("Next map enabled");
            }

            //Hero.transform.position = tileDataAccess[unlockedLevels[ctr]].gameObject.transform.position;
			Camera.main.transform.position = new Vector3(tileDataAccess[unlockedLevels[ctr]].gameObject.transform.position.x,tileDataAccess[unlockedLevels[ctr]].gameObject.transform.position.y,-10f);
			
			if (chapter == 1 && tileDataAccess[unlockedLevels[ctr]].level == 3)
			{
				ui.ActivateHangar();
				tutorial.tutorials_on[0] = true;
			}
			else if (chapter == 1 && tileDataAccess[unlockedLevels[ctr]].level == 6)
			{
				tutorial.tutorials_on[1] = true;
			}
			else if (chapter == 1 && tileDataAccess[unlockedLevels[ctr]].level == 7)
			{
				ui.ActivateStore();
				tutorial.tutorials_on[2] = true;
			}
			else if (chapter == 1 &&  tileDataAccess[unlockedLevels[ctr]].level == 0)
			{
				tutorial.tutorials_on[3] = true;
			}
			else if (chapter == 1 &&  tileDataAccess[unlockedLevels[ctr]].level == 1)
			{
				tutorial.tutorials_on[4] = true;
			}
			
			if (chapter != 1 && !done_once) //done_once is so you only ActivateArrows once
			{
				ui.ActivateArrows();
				done_once = true;
			}
        }

        for (int ctr = 0; ctr < latestUnlocked.Count; ctr++)
        {
            activate_remaining_tiles(latestUnlocked[ctr]);
        }
		tutorial.ActivateTutorials();
	}
}
