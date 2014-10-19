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



public class HeroLevelData : MonoBehaviour {
    public PlayerLevelData[] levelData;
    public PlayerMasterData playerMasterData;
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

    public void save_data()
    {
        playerMasterData.save_player_level(playerLevel, playerExperience);
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
        playerMasterData.save_player_level(playerLevel, playerExperience);
        return false;
    }

    public PlayerLevelData get_player_level_data(int inputLevel, int experience)
    {
        playerLevel = inputLevel;
        playerExperience = experience;
        if (inputLevel <= 0)
            inputLevel = 1;
        return levelData[inputLevel - 1];
    }

    
	public PlayerMasterStat get_player_stat_all () {
        PlayerMasterStat temp = playerMasterData.get_combined_stats();
        playerLevel = temp.level;
        playerExperience = temp.curExp;
        temp.damage += (int)levelData[playerLevel - 1].damage;
        temp.hp += (int)levelData[playerLevel - 1].HP;
        return temp;
	}

    public PlayerLevelData get_player_stat()
    {
        PlayerLevelData temp;
        
        temp = levelData[playerLevel - 1];
        return temp;
    }
}
