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

        map = new MapData(mapTileHolder, chapter);
        IList<int> unlockedLevels = new List<int>();
        unlockedLevels = map.get_unlocked_levels();

        for (int ctr = 0; ctr < unlockedLevels.Count; ctr++)
        {
            tileDataAccess[unlockedLevels[ctr]].TileOn();
            //Hero.transform.position = tileDataAccess[unlockedLevels[ctr]].gameObject.transform.position;
			Camera.main.transform.position = new Vector3(tileDataAccess[unlockedLevels[ctr]].gameObject.transform.position.x,tileDataAccess[unlockedLevels[ctr]].gameObject.transform.position.y,-10f);
			
			if (true)//tileDataAccess[unlockedLevels[ctr]].level == 3)
			{
				ui.ActivateHangar();
				tutorial.tutorials_on[0] = true;
			}
			if (tileDataAccess[unlockedLevels[ctr]].level == 6)
			{
				tutorial.tutorials_on[1] = true;
			}
			if (tileDataAccess[unlockedLevels[ctr]].level == 7)
			{
				ui.ActivateStore();
				tutorial.tutorials_on[2] = true;
			}
        }
		tutorial.ActivateTutorials();
        /*
		for (int i = 0; i <= levelsCompleted; i++)
		{
			string name;
			if (i < 10)
			{
				name = "00"+i.ToString();
			}
			else
			{
				name = "0"+i.ToString();
			}
			GameObject tile = GameObject.Find(name);
			TileActive activate = tile.GetComponent<TileActive>();
			activate.TileOn();
			Hero.transform.position = tile.transform.position;
			Camera.main.transform.position = new Vector3(tile.transform.position.x,tile.transform.position.y,-10f);
		}
         */ 
	}
}
