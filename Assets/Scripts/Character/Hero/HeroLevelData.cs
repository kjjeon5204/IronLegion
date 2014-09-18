/*using UnityEngine;
using System.Collections;
using System.IO;
using System;

[System.Serializable]
public struct PlayerLevelData
{
    public int HP;
    public float damage;
    public int experience;
}

public class HeroLevelData : MonoBehaviour {
    public PlayerLevelData[] levelData;
    int playerLevel = 0;
    int playerExperience;



    public int get_player_level()
    {
        return playerLevel;
    }

    public int get_player_experience()
    {
        return playerExperience;
    }

    public int get_experience_required()
    {
        return levelData[playerLevel - 1].experience;
    }



    //returns true if level up
    //false if not level up
    public bool add_experience(int expGain)
    {
        //player hit max level
        if (levelData[playerLevel - 1].experience == 0 || playerLevel == levelData.Length)
        {
            playerExperience = -1;
            return false;
        }
        playerExperience += expGain;
        if (playerExperience >= levelData[playerLevel-1].experience)
        {
            playerExperience -= levelData[playerLevel-1].experience;
            playerLevel++;
            return true;
        }
        return false;
    }
	
	public Stats get_player_stat_all () {
		Stats baseStats = new Stats();
		baseStats.level = playerLevel;
		baseStats.curExp = playerExperience;
		baseStats.totalExp = levelData[playerLevel - 1].experience;
		baseStats.baseHp = levelData[playerLevel - 1].HP;
		baseStats.item_energy = 100.0f;
		baseStats.equipment = null;
		baseStats.item_hp = 0;
		baseStats.item_armor = 0f;
		baseStats.item_damage = levelData[playerLevel - 1].damage;
		baseStats.item_penetration = 0.0f;
		baseStats.item_luck = 0.0f;
		return baseStats;
	}

    public PlayerLevelData get_player_stat()
    {
        PlayerLevelData temp;
        
        temp = levelData[playerLevel - 1];
        return temp;
    }
}
 */

using UnityEngine;
using System.Collections;
using System.IO;
using System;

[System.Serializable]
public struct PlayerLevelData
{
    public int HP;
    public float damage;
    public int experience;
}

public class HeroLevelData : MonoBehaviour
{
    public PlayerLevelData[] levelData;
    int playerLevel = 0;
    int playerExperience;


    public int get_player_level()
    {
        return playerLevel;
    }

    public int get_player_experience()
    {
        return playerExperience;
    }

    public int get_experience_required()
    {
        return levelData[playerLevel - 1].experience;
    }

    public void create_new_player()
    {
        string fileName = "/HeroLevelData.txt";
        string dataPath = Application.persistentDataPath + fileName;

        using (StreamWriter outFile = File.CreateText(dataPath))
        {
            outFile.WriteLine("1");
            outFile.WriteLine("0");
        }
    }

    public void load_file()
    {

        string fileName = "/HeroLevelData.txt";
        string dataPath = Application.persistentDataPath + fileName;

        if (!File.Exists(dataPath))
        {
            create_new_player();
        }

        using (StreamReader fileAcc = File.OpenText(dataPath))
        {
            playerLevel = Convert.ToInt32(fileAcc.ReadLine());
            playerExperience = Convert.ToInt32(fileAcc.ReadLine());
        }
    }

    public void save_file()
    {
        string fileName = "/HeroLevelData.txt";
        string dataPath = Application.persistentDataPath + fileName;

        using (StreamWriter outFile = File.CreateText(dataPath))
        {
            outFile.WriteLine(playerLevel);
            outFile.WriteLine(playerExperience);
        }
    }

    //returns true if level up
    //false if not level up
    public bool add_experience(int expGain)
    {
        //player hit max level
        if (levelData[playerLevel - 1].experience == 0 || playerLevel == levelData.Length)
        {
            playerExperience = -1;
            return false;
        }
        playerExperience += expGain;
        if (playerExperience >= levelData[playerLevel - 1].experience)
        {
            playerExperience -= levelData[playerLevel - 1].experience;
            playerLevel++;
            return true;
        }
        return false;
    }

    public Stats get_player_stat_all()
    {
        Stats baseStats = new Stats();
        baseStats.level = playerLevel;
        baseStats.curExp = playerExperience;
        baseStats.totalExp = levelData[playerLevel - 1].experience;
        baseStats.baseHp = levelData[playerLevel - 1].HP;
        baseStats.item_energy = 100.0f;
        baseStats.equipment = null;
        baseStats.item_hp = 0;
        baseStats.item_armor = 0f;
        baseStats.item_damage = levelData[playerLevel - 1].damage;
        baseStats.item_penetration = 0.0f;
        baseStats.item_luck = 0.0f;
        return baseStats;
    }

    public PlayerLevelData get_player_stat()
    {
        PlayerLevelData temp;
        if (playerLevel == 0)
        {
            load_file();
        }
        temp = levelData[playerLevel - 1];
        return temp;
    }
}
 
