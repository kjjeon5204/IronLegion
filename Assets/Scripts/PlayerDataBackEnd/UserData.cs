using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using System.IO;
using System;

public struct PlayerMechData
{
    public string mechID;
    public int level;
    public int curExp;
    public int health;
    public int damage;
    public int armor;
    public int pentration;
    public int luck;
    public int energy;
    public string[] equippedSkill;
    public IList<string> itemsEquipped;
}

public struct OfflinePlayerData
{
    public int expGained;
    public int goldGained;
    public int cogentumGained;
    public string[] itemsObtained;
}

public struct MissionData
{
    public int mission;
    public int clearCount;
}

public struct UserMapData
{
    public int chapter;
    public IList<MissionData> clearedMissionList;
}

public class UserDataContainer
{
    
    public enum PlayerLoginStatus
    {
        GOOGLEPLUS,
        GUEST
    }
    #region USER_DATA
    string playerID;
    public PlayerLoginStatus loginStatus;
    float totalMinutesPlayed;
    int numberOfTilesCleared;
    #endregion

    #region USER_PROGRESS
    public IList<string> clearedStoryDialogues;
    public IList<UserMapData> currentlyClearedMaps;
    #endregion

    IList<string> itemInventory;
    public int creditOwned;
    public int cogentumOwned;

    #region USER_MECH_DATA
    int currentlyEquippedMech;
    IList<PlayerMechData> mechCollection;
    #endregion

    public void equip_ability(string abilityName, int abilitySlot)
    {
        if (abilityName[0] == 'C')
        {
            mechCollection[currentlyEquippedMech].equippedSkill[abilitySlot] = abilityName;
        }
        else if (abilityName[0] == 'F')
        {
            mechCollection[currentlyEquippedMech].equippedSkill[abilitySlot + 4] = abilityName;
        }
    }

    public void add_item_to_inventory(string itemID)
    {
        itemInventory.Add(itemID);
    }

    public IList<string> get_item_inventory()
    {
        return itemInventory;
    }

    public void set_item_inventory(IList<string> inputInventory)
    {
        itemInventory = inputInventory;
    }

    public PlayerMechData get_current_mech()
    {
        return mechCollection[currentlyEquippedMech];
    }

    public void set_current_mech(PlayerMechData newMechData)
    {
        mechCollection[currentlyEquippedMech] = newMechData;
    }

    public byte[] encode_data_to_byte()
    {
        using (MemoryStream m = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                #region USER_DATA_STORE_METHOD
                writer.Write(playerID);
                writer.Write((double)totalMinutesPlayed);
                #endregion

                #region USER_PROGRESS_STORE_METHOD
                //store currently cleared tiles
                writer.Write(currentlyClearedMaps.Count);
                for (int ctr = 0; ctr < currentlyClearedMaps.Count; ctr++)
                {
                    for (int missionCtr = 0; missionCtr < currentlyClearedMaps[ctr].
                        clearedMissionList.Count; missionCtr++)
                    {
                        writer.Write(currentlyClearedMaps[ctr].clearedMissionList[missionCtr].mission);
                        writer.Write(currentlyClearedMaps[ctr].clearedMissionList[missionCtr].clearCount);
                    }
                }

                //current seen dialogues
                writer.Write(clearedStoryDialogues.Count);
                for (int ctr = 0; ctr < clearedStoryDialogues.Count; ctr++)
                {
                    writer.Write(clearedStoryDialogues[ctr]);
                }
                #endregion

                #region USER_ITEM_STORE_METHOD
                //store inventory
                writer.Write(itemInventory.Count);
                for (int ctr = 0; ctr < itemInventory.Count; ctr++)
                {
                    writer.Write(itemInventory[ctr]);
                }
                #endregion

                #region USER_MECH_DATA_STORE_METHOD
                //currently equipped mech
                writer.Write(currentlyEquippedMech);
                writer.Write(mechCollection.Count);
                for (int mechCtr = 0; mechCtr < mechCollection.Count; mechCtr++)
                {
                    writer.Write(mechCollection[mechCtr].mechID);
                    writer.Write(mechCollection[mechCtr].health);
                    writer.Write(mechCollection[mechCtr].damage);
                    writer.Write(mechCollection[mechCtr].armor);
                    writer.Write(mechCollection[mechCtr].pentration);
                    writer.Write(mechCollection[mechCtr].luck);
                    writer.Write(mechCollection[mechCtr].itemsEquipped.Count);
                    for (int ctr = 0; ctr < mechCollection[mechCtr].itemsEquipped.Count; ctr++)
                    {
                        writer.Write(mechCollection[mechCtr].itemsEquipped[ctr]);
                    }
                    for (int ctr = 0; ctr < 8; ctr++)
                    {
                        writer.Write(mechCollection[mechCtr].equippedSkill[ctr]);
                    }
                }
                #endregion
            }
            //Store to local dataPath
//string dataPath = Application.dataPath + "UserData.txt";
            //File.WriteAllBytes(dataPath, m.ToArray());
            return m.ToArray();
        }
    }

    public void decode_byte_to_data(byte[] userRawData)
    {
        using (MemoryStream m = new MemoryStream(userRawData))
        {
            using (BinaryReader reader = new BinaryReader(m))
            {
                #region USER_DATA_LOAD_METHOD
                playerID = reader.ReadString();
                totalMinutesPlayed = (float)reader.ReadDouble();
                #endregion

                #region USER_PROGRESS_LOAD_METHOD
                int clearedMapCount = reader.ReadInt32();
                int chapterMissionClearCount = 0;
                currentlyClearedMaps = new List<UserMapData>();
                for (int ctr = 0; ctr < clearedMapCount; ctr++)
                {
                    //Save chapter data
                    UserMapData tempChapter = new UserMapData();
                    tempChapter.chapter = ctr;
                    tempChapter.clearedMissionList = new List<MissionData>();
                    chapterMissionClearCount = reader.ReadInt32();
                    for (int missionCtr = 0; missionCtr < chapterMissionClearCount; missionCtr++)
                    {
                        //Save cleared mission per chapter
                        MissionData temp = new MissionData();
                        temp.mission = reader.ReadInt32();
                        temp.clearCount = reader.ReadInt32();
                        tempChapter.clearedMissionList.Add(temp);
                    }
                    //Add chapter progress to object
                    currentlyClearedMaps.Add(tempChapter);
                }

                int sceneViewCount = reader.ReadInt32();
                for (int ctr = 0; ctr < sceneViewCount; ctr++)
                {
                    clearedStoryDialogues.Add(reader.ReadString());
                }
                #endregion

                #region USER_ITEM_LOAD_METHOD
                int numItems = reader.ReadInt32();
                for (int ctr = 0; ctr < numItems; ctr++)
                {
                    itemInventory.Add(reader.ReadString());
                }
                #endregion

                #region USER_MECH_DATA_LOAD_METHOD
                currentlyEquippedMech = reader.ReadInt32();
                int numMechs = reader.ReadInt32();
                for (int mechCtr = 0; mechCtr < numMechs; mechCtr++)
                {
                    PlayerMechData tempMechData = new PlayerMechData();
                    tempMechData.mechID = reader.ReadString();
                    tempMechData.health = reader.ReadInt32();
                    tempMechData.damage = reader.ReadInt32();
                    tempMechData.armor = reader.ReadInt32();
                    tempMechData.pentration = reader.ReadInt32();
                    tempMechData.luck = reader.ReadInt32();
                    int equippedItemCount = reader.ReadInt32();
                    for (int ctr = 0; ctr < equippedItemCount; ctr++)
                    {
                        tempMechData.itemsEquipped.Add(reader.ReadString());
                    }
                    tempMechData.equippedSkill = new string[8];
                    for (int ctr = 0; ctr < 8; ctr++)
                    {
                        tempMechData.equippedSkill[ctr] = reader.ReadString();
                    }
                   
                }
                #endregion
            }
        }
    }

    public void create_new_user_data(string userID, PlayerMechData starterMech)
    {
        //Initialize player ID
        playerID = userID;
        totalMinutesPlayed = 0;
        currentlyEquippedMech = 0;
        clearedStoryDialogues = new List<string>();
        currentlyClearedMaps = new List<UserMapData>();
        itemInventory = new List<string>();
        mechCollection = new List<PlayerMechData>();
        mechCollection.Add(starterMech);
    }
}



public enum UserGameMode 
{
    ONLINE,
    OFFLINE
}

public class UserData : MonoBehaviour
{

    #region STATIC VARIABLES
    public static UserGameMode curGameMode;
    public static byte[] playerRawData;
    public static UserDataContainer userDataContainer;
    public static string nextTargetScene;
    #endregion



    #region STATIC FUNCTIONS
    #region UTILITY_FUNCTION
    public static void initialize_user_data()
    {
        userDataContainer = new UserDataContainer();
    }

    public static void initialize_player(byte[] rawData)
    {
        playerRawData = rawData;
        userDataContainer.decode_byte_to_data(playerRawData);
    }



    public static void create_new_user_data(string userID, PlayerMechData starterMech)
    {
        userDataContainer.create_new_user_data(userID, starterMech);
    }

    public static UserDataContainer get_user_data()
    {
        return userDataContainer;
    }

    public static byte[] get_byte_data()
    {
        playerRawData = userDataContainer.encode_data_to_byte();
        return playerRawData;
    }
    #endregion

    #region USER_GAME_MODE
    public static void switch_to_offline_mode()
    {
        curGameMode = UserGameMode.OFFLINE;
    }

    public static void switch_to_online_mode()
    {
        curGameMode = UserGameMode.ONLINE;
    }

    public static UserGameMode get_cur_mode()
    {
        return curGameMode;
    }
    #endregion

    #region USER_DATA_FUNCTIONS
    #endregion
    #endregion


}
