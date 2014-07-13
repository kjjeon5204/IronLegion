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

public class MapData {
	int chapter;
    int mapCount;
	MapTileData[] tilesData;
    
	public MapData(MapTileData[] inputMapData, int inChapter) {
		tilesData = inputMapData;
		chapter = inChapter;

		read_map_progress();
        store_map_progress();
	}


    public MapData(int inChapter)
    {
        chapter = inChapter;
        read_map_progress();
    }
    
	public void clear_level(int level) {
        //Debug.Log(level);
        Debug.Log(tilesData.Length);
		tilesData[level - 1].clearCount++;

		store_map_progress();
	}

	//modifies tilesData to include progress
	void read_map_progress() {
		string fileName = "/MapProgress/Chapter" + chapter + ".txt";
		string dataPath = Application.persistentDataPath + fileName;
        if (!File.Exists(dataPath))
        {
            store_map_progress();
        }
		string readString = null;

		using (StreamReader inFile = File.OpenText (dataPath)) {
            //Read total map count
            readString = inFile.ReadLine();
            mapCount = Convert.ToInt32(readString);
            if (tilesData == null)
            {
                tilesData = new MapTileData[Convert.ToInt32(readString)];
            }
			for ( int ctr = 0; ctr < tilesData.Length; ctr ++) {
				if (readString != "#") {
					readString = inFile.ReadLine();
                    if (readString != "#")
					    tilesData[ctr].clearCount = Convert.ToInt32(readString);
				}
				else
					tilesData[ctr].clearCount = 0;
			}
		}
	}

	void store_map_progress() {
        if (!Directory.Exists(Application.persistentDataPath + "/MapProgress"))
            Directory.CreateDirectory(Application.persistentDataPath + "/MapProgress");
		string fileName = "/MapProgress/Chapter" + chapter + ".txt";
		string dataPath = Application.persistentDataPath + fileName;

		using (StreamWriter outFile = File.CreateText (dataPath)) {
            if (tilesData != null)
                outFile.WriteLine(tilesData.Length);
            if (tilesData == null)
                outFile.WriteLine(mapCount);

			for ( int ctr = 0; ctr < tilesData.Length; ctr ++ ) {
				outFile.WriteLine(tilesData[ctr].clearCount);
			}
			outFile.WriteLine("#");
		}
	}

	public IList<int> get_unlocked_levels() {
		IList<int> curUnlocked = new List<int>();

		curUnlocked.Add (0);

		return recursively_check_levels (curUnlocked, 0);
	}


	IList<int> recursively_check_levels(IList<int> inputLevels, int checkChapter) {
		if (tilesData[checkChapter].clearCount > 0) {
			for (int ctr = 0; ctr < tilesData[checkChapter].adjLevelList.Length; ctr ++) {
				inputLevels.Add(tilesData[checkChapter].adjLevelList[ctr]);
				recursively_check_levels(inputLevels, tilesData[checkChapter].adjLevelList[ctr]);
			}
			return inputLevels;
		}
		else {
			return inputLevels;
		}
	}
}
