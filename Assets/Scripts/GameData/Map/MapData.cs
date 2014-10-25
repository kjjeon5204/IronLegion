using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


public struct MapTileData
{
    public int level;
	public string mapID;
    public int clearCount;
    public int[] adjLevelList;
}

public struct ChapterData
{
    public int chapterNum;
    public MapTileData[] tilesData;
    public IList<int> unlockedTiles;
    public IList<int> latestTiles;
}

public class MapData {
    int maxChapterCount = 2; //This value must be modified in order for new chapters to work
    int mapCount;
    ChapterData[] myData;
    IList<int> lastestTilesUnlocked = new List<int>();
    
    //Number of tiles per chapter
    int chapter1TileCount = 35;
    int chapter2TileCount = 29;

    //Obsolete! Do not use this constructor format! 99% will break
	public MapData(MapTileData[] inputMapData, int inChapter) {
        load_full_data();
		//read_map_progress(inChapter);
        save_full_map_data();
	}

    public MapData(ChapterData[] inputChapterData)
    {
        myData = inputChapterData;
        load_full_data();
        save_full_map_data();
    }

    public void initialize_map_progress(int chapter, MapTileData[] mapTiles)
    {
        myData[chapter - 1].chapterNum = chapter;
        myData[chapter - 1].tilesData = mapTiles;
        read_map_progress(chapter);
        myData[chapter - 1].unlockedTiles = get_unlocked_levels(chapter - 1);
        myData[chapter - 1].latestTiles = get_latest_levels();
        store_map_progress(chapter);
    }

    public MapData()
    {
        myData = new ChapterData[maxChapterCount];
        //load_full_data();
    }
    
	public int clear_level(int chapter, int level) {
        read_map_unitialized(chapter);
		myData[chapter - 1].tilesData[level - 1].clearCount++;
        store_map_progress(chapter);
        return myData[chapter - 1].tilesData[level - 1].clearCount;
	}

    void load_full_data()
    {
        myData = new ChapterData[maxChapterCount];
        for (int ctr = 0; ctr < maxChapterCount; ctr++)
        {
            myData[ctr].chapterNum = ctr + 1;
            read_map_progress(ctr + 1);
            //myData[ctr].unlockedTiles = get_unlocked_levels(ctr);
            //myData[ctr].latestTiles = get_latest_levels();
        }
    }

    void save_full_map_data()
    {
        for (int ctr = 0; ctr < maxChapterCount; ctr++)
        {
            store_map_progress(ctr + 1);
        }
    }


    //modifies tilesData to include progress
    void read_map_progress(int chapter)
    {
		string fileName = "/MapProgress/Chapter" + chapter + ".txt";
		string dataPath = Application.persistentDataPath + fileName;
        if (!File.Exists(dataPath))
        {
            create_map_file(chapter);
        }
		string readString = null;
        //MapTileData[] tilesData;
		using (StreamReader inFile = File.OpenText (dataPath)) {
            //Read total map count
            readString = inFile.ReadLine();
            mapCount = Convert.ToInt32(readString);
            //tilesData = new MapTileData[Convert.ToInt32(readString)];
			for ( int ctr = 0; ctr < myData[chapter - 1].tilesData.Length; ctr ++) {
				if (readString != "#") {
					readString = inFile.ReadLine();
                    if (readString != "#")
                        myData[chapter - 1].tilesData[ctr].clearCount = Convert.ToInt32(readString);
				}
				else
                    myData[chapter - 1].tilesData[ctr].clearCount = 0;
			}
		}
	}

    void read_map_unitialized(int chapter)
    {
        string fileName = "/MapProgress/Chapter" + chapter + ".txt";
        string dataPath = Application.persistentDataPath + fileName;
        if (!File.Exists(dataPath))
        {
            create_map_file(chapter);
        }
        string readString = null;
        //MapTileData[] tilesData;
        using (StreamReader inFile = File.OpenText(dataPath))
        {
            //Read total map count
            readString = inFile.ReadLine();
            mapCount = Convert.ToInt32(readString);
            //tilesData = new MapTileData[Convert.ToInt32(readString)];
            myData[chapter - 1].tilesData = new MapTileData[mapCount];
            for (int ctr = 0; ctr < myData[chapter - 1].tilesData.Length; ctr++)
            {
                if (readString != "#")
                {
                    readString = inFile.ReadLine();
                    if (readString != "#")
                        myData[chapter - 1].tilesData[ctr].clearCount = Convert.ToInt32(readString);
                }
                else
                    myData[chapter - 1].tilesData[ctr].clearCount = 0;
            }
        }
    }

    void create_map_file(int chapter)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/MapProgress"))
            Directory.CreateDirectory(Application.persistentDataPath + "/MapProgress");
        string fileName = "/MapProgress/Chapter" + chapter + ".txt";
        string dataPath = Application.persistentDataPath + fileName;

        using (StreamWriter outFile = File.CreateText(dataPath))
        {
            outFile.WriteLine("0");
            outFile.WriteLine("#");
        }
    }

	void store_map_progress(int chapter) {
        if (!Directory.Exists(Application.persistentDataPath + "/MapProgress"))
            Directory.CreateDirectory(Application.persistentDataPath + "/MapProgress");
		string fileName = "/MapProgress/Chapter" + chapter + ".txt";
		string dataPath = Application.persistentDataPath + fileName;

		using (StreamWriter outFile = File.CreateText (dataPath)) {
            if (myData[chapter - 1].tilesData != null)
                outFile.WriteLine(myData[chapter - 1].tilesData.Length);
            if (myData[chapter - 1].tilesData == null)
                outFile.WriteLine(mapCount);

			for ( int ctr = 0; ctr < myData[chapter - 1].tilesData.Length; ctr ++ ) {
				outFile.WriteLine(myData[chapter - 1].tilesData[ctr].clearCount);
			}
			outFile.WriteLine("#");
		}
	}


    public IList<int> return_unlocked_levels(int chapter)
    {
        return myData[chapter - 1].unlockedTiles;
    }

	IList<int> get_unlocked_levels(int chapter) {
		IList<int> curUnlocked = new List<int>();
        lastestTilesUnlocked = new List<int>();
		curUnlocked.Add (0);

		return recursively_check_levels (curUnlocked, 0, chapter);
	}

    public IList<int> return_latest_levels(int chapter)
    {
        return myData[chapter - 1].latestTiles;
    }


    IList<int> get_latest_levels()
    {
        return lastestTilesUnlocked;
    }


	IList<int> recursively_check_levels(IList<int> inputLevels, int checkTile, int chapter) {
		if (myData[chapter].tilesData[checkTile].clearCount > 0) {
			for (int ctr = 0; ctr < myData[chapter].tilesData[checkTile].adjLevelList.Length; ctr ++) {
				inputLevels.Add(myData[chapter].tilesData[checkTile].adjLevelList[ctr]);
				recursively_check_levels(inputLevels, myData[chapter].tilesData[checkTile].adjLevelList[ctr], chapter);
			}
			return inputLevels;
		}
		else {
            lastestTilesUnlocked.Add(checkTile);
			return inputLevels;
		}
	}
}
