/*using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public struct LoadGame
{
    public int level;
    public string environment;
    public string player;
    public int numEnemyTypes;
    public string[] enemies;
    public int numEnemies;
    public int clearCount;
    public int expCount;
    public int itemTierGen;
}


public class MapData
{
    LoadGame[] gameData;
    LoadGame ret;

    public MapData()
    {

    }


    public void load_dictionary()
    {
        //string fileName = "/MapData.txt";
        string fileName = "/MapData.txt";
        string path = Application.persistentDataPath + fileName;
        //string path = Application.dataPath + fileName;
        string rawFileData;

        using (StreamReader inFile = File.OpenText(path))
        {
            rawFileData = inFile.ReadLine();
            gameData = new LoadGame[Convert.ToInt32(rawFileData)];

            for (int ctr = 0; ctr < gameData.Length; ctr++)
            {
                rawFileData = inFile.ReadLine();
                gameData[ctr].level = Convert.ToInt32(rawFileData);
                rawFileData = inFile.ReadLine();
                gameData[ctr].environment = rawFileData;
                rawFileData = inFile.ReadLine();
                gameData[ctr].player = rawFileData;
                rawFileData = inFile.ReadLine();
                gameData[ctr].numEnemyTypes = Convert.ToInt32(rawFileData);
                gameData[ctr].enemies = new string[gameData[ctr].numEnemyTypes];
                for (int enemyCtr = 0; enemyCtr < gameData[ctr].numEnemyTypes; enemyCtr++)
                {
                    rawFileData = inFile.ReadLine();
                    gameData[ctr].enemies[enemyCtr] = rawFileData;
                }
                rawFileData = inFile.ReadLine();
                Debug.Log(rawFileData);
                gameData[ctr].numEnemies = Convert.ToInt32(rawFileData);
                rawFileData = inFile.ReadLine();
                gameData[ctr].clearCount = Convert.ToInt32(rawFileData);
                rawFileData = inFile.ReadLine();
                gameData[ctr].expCount = Convert.ToInt32(rawFileData);
                rawFileData = inFile.ReadLine();
                gameData[ctr].itemTierGen = Convert.ToInt32(rawFileData);
                inFile.ReadLine();
            }
        }
    }


    public void store_dictionary()
    {
        string fileName = "/MapData.txt";
        string path = Application.persistentDataPath + fileName;

        using (StreamWriter outfile = File.CreateText(path))
        {
            outfile.WriteLine(gameData.Length);

            for (int ctr = 0; ctr < gameData.Length; ctr++)
            {
                outfile.WriteLine(gameData[ctr].level);
                outfile.WriteLine(gameData[ctr].environment);
                outfile.WriteLine(gameData[ctr].player);
                outfile.WriteLine(gameData[ctr].numEnemyTypes);
                for (int ctr1 = 0; ctr1 < gameData[ctr].numEnemyTypes; ctr1++)
                    outfile.WriteLine(gameData[ctr].enemies[ctr1]);
                outfile.WriteLine(gameData[ctr].numEnemies);
                outfile.WriteLine(gameData[ctr].clearCount);
                outfile.WriteLine(gameData[ctr].expCount);
                outfile.WriteLine(gameData[ctr].itemTierGen);
                outfile.WriteLine("");
            }
        }
    }


    public void store_map_data(int acc)
    {
        string fileName = "/mapDataTransfer.txt";
        string path = Application.persistentDataPath + fileName;

        if (gameData == null)
        {
            this.load_dictionary();
        }

        using (StreamWriter outfile = File.CreateText(path))
        {
            outfile.WriteLine(gameData[acc].level);
            outfile.WriteLine(gameData[acc].environment);
            outfile.WriteLine(gameData[acc].player);
            outfile.WriteLine(gameData[acc].numEnemyTypes);
            for (int ctr = 0; ctr < gameData[acc].numEnemyTypes; ctr++)
                outfile.WriteLine(gameData[acc].enemies[ctr]);
            outfile.WriteLine(gameData[acc].numEnemies);
            outfile.WriteLine(gameData[acc].clearCount);
            outfile.WriteLine(gameData[acc].expCount);
            outfile.WriteLine(gameData[acc].itemTierGen);
        }
    }


    public void map_cleared(int acc)
    {
        gameData[acc].clearCount++;
    }


    public LoadGame get_cur_map_data()
    {
        return ret;
    }

    public LoadGame get_map_data(int acc)
    {
        if (gameData == null)
        {
            this.load_dictionary();
        }
        return gameData[acc - 1];
    }


    public LoadGame load_map_data()
    {
        string fileName = "/mapDataTransfer.txt";
        string path = Application.persistentDataPath + fileName;
        ret = new LoadGame();
        string rawFileData;

        using (StreamReader inFile = File.OpenText(path))
        {
            rawFileData = inFile.ReadLine();
            ret.level = Convert.ToInt32(rawFileData);
            rawFileData = inFile.ReadLine();
            ret.environment = rawFileData;
            rawFileData = inFile.ReadLine();
            ret.player = rawFileData;
            rawFileData = inFile.ReadLine();
            ret.numEnemyTypes = Convert.ToInt32(rawFileData);
            ret.enemies = new string[ret.numEnemyTypes];
            for (int enemyCtr = 0; enemyCtr < ret.numEnemyTypes; enemyCtr++)
            {
                rawFileData = inFile.ReadLine();
                ret.enemies[enemyCtr] = rawFileData;
            }
            rawFileData = inFile.ReadLine();
            ret.numEnemies = Convert.ToInt32(rawFileData);
            rawFileData = inFile.ReadLine();
            ret.clearCount = Convert.ToInt32(rawFileData);
            rawFileData = inFile.ReadLine();
            ret.expCount = Convert.ToInt32(rawFileData);
            rawFileData = inFile.ReadLine();
            ret.itemTierGen = Convert.ToInt32(rawFileData);
        }
        return ret;
    }

    public int LevelsCompleted()
    {
        int sum = 0;
        for (int i = 0; i < gameData.Length; i++)
        {
            if (gameData[i].clearCount > 0)
            {
                sum++;
            }
            else
            {
                break;
            }
        }
        return sum;
    }
}
*/